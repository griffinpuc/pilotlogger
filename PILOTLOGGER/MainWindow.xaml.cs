using AdonisUI;
using LiveCharts.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

/*

    PILOT RC SERIAL PORT MONITOR AND LOGGER
    MAIN CLASS

    BY GRIFFIN PUC 2020

 */

namespace PILOTLOGGER {

    public partial class MainWindow : Window
    {

        SerialPort serialPort;
        string schemaCode;
        string baseDirectory;
        bool isLogging;
        List<string> schemaNames = new List<string>();

        Monitor monitorWindow;

        private BlockingCollection<string> OutputQueue; //Output queue for file writeout
        Dictionary<string, CancellationTokenSource> threads; //Keep track of threads for serial monitor

        public MainWindow()
        {
            AdonisUI.ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);
            InitializeComponent();
            applicationStartup();

        }

        /* When X is pressed */
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        /* Application startup tasks */
        private void applicationStartup()
        {
            threads = new Dictionary<string, CancellationTokenSource>();
            baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PilotRC";
            outputBox.Text = baseDirectory + "\\logs";

            monitorCOM();
            monitorSchemas();
        }

        /* COM monitoring startup tasks (pre monitoring) */
        private void startComMonitor(object sender, RoutedEventArgs e)
        {
            //Set serialport options for PILOT RC
            serialPort = new SerialPort()
            {
                PortName = comcombo.SelectedItem.ToString(),
                BaudRate = 115200,
                DtrEnable = true,
                NewLine = "\r"
            };

            //Monitor the serial port in the background with cancel token
            CancellationTokenSource cts = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(new WaitCallback(monitorCom), cts.Token);
            threads.Add(comcombo.SelectedItem.ToString(), cts);

            //Modify user controls to reflect logging-in-progress
            Dispatcher.Invoke(new Action(() =>
            {
                startBut.IsEnabled = false;
                stopBut.IsEnabled = true;
            }));

            //Set labels and values
            isLogging = true;
            setStatus("Logging in progress...");

        }

        //COM monitoring closing tasks (post monitoring)
        private void endComMonitor(object sender, RoutedEventArgs e)
        {
            //Grab corresponding cancellation token
            CancellationTokenSource cts = threads.Where(pair => pair.Key.Equals(comcombo.SelectedItem.ToString())).Select(pair => pair.Value).FirstOrDefault();

            //Modify user controls to reflect end of logging
            Dispatcher.Invoke(new Action(() =>
            {
                startBut.IsEnabled = true;
                stopBut.IsEnabled = false;
            }));

            //Dispose of thread and cancellation token
            serialPort.Close();
            cts.Cancel();
            Thread.Sleep(1500);
            cts.Dispose();
            threads.Remove(serialPort.PortName);

            isLogging = false;
            monitorWindow.Close();
            setStatus("Logging completed!");
        }

        //COM monitor background task
        private async void monitorCom(Object obj)
        {
            CancellationToken token = (CancellationToken)obj;

            //Write to file in background as data streams in
            OutputQueue = new BlockingCollection<string>();
            string outputFilename =  "PILOTLOG_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            var outputTask = Task.Factory.StartNew(() => writeFile(outputFilename), TaskCreationOptions.LongRunning);

            //Create and open the monitor window
            Dispatcher.Invoke(new Action(() =>
            {
                //Launch and initialize monitor window
                monitorWindow = new Monitor();

                //Initialize charts
                try
                {
                    monitorWindow.initChart();
                    monitorWindow.setValues(schemaCode);
                    monitorWindow.setChartDefault();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error Initializing Live Chart \n" + ex.Message);
                }

                //Initialize models
                try
                {
                    monitorWindow.initMap();
                    monitorWindow.initModel();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error Initializing Live Model \n" + ex.Message);
                }

                //Show the monitor window
                monitorWindow.Show();

            }));

            await Task.Run(() =>
            {

                serialPort.Open();
                serialPort.ReadLine();

                int iter = 0;

                while (true)
                {
                    if (!token.IsCancellationRequested)
                    {
                        try
                        {
                            //Parse incoming serial data
                            string readLine = serialPort.ReadLine().Replace("\n", "");
                            string parsedLine = readLine.Replace("\t", ",");

                            //Write out to various processes
                            OutputQueue.Add(parsedLine);

                            //Every x cycles, modiy graph
                            if ((iter%10) == 0)
                            {
                                monitorWindow.addValues(parsedLine.Remove(parsedLine.Length - 1));
                                setLabels(parsedLine.Remove(parsedLine.Length - 1));
                            }
                            
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Error Reading Serial Port Stream \n" + ex.Message);
                            break;
                        }
                    }
                    else
                    {
                        //On end, clear up and finish with outputqueue
                        OutputQueue.CompleteAdding();
                        outputTask.Wait();
                        break;
                    }

                    iter++;
                }
            }); 
        }

        /* CSV File output method */
        private void writeFile(string fileName)
        {
            using (var strm = File.AppendText(baseDirectory + "\\logs\\" + fileName))
            {
                //Write schema code to first line for CSV
                strm.WriteLine(schemaCode);

                //As each new line comes in write to file
                foreach (var dataLine in OutputQueue.GetConsumingEnumerable())
                {
                    strm.WriteLine(dataLine);
                    strm.Flush();
                }
            }
        }

        /* Update schema combobox on selection */
        private void selectSchema(object sender, RoutedEventArgs e)
        {
            schemacombo.SelectedItem = sender;
            schemacombo.IsDropDownOpen = false;
            schemaCode = File.ReadAllText(baseDirectory + "\\schemas\\" + schemacombo.Text);
        }

        /* Open the output folder */
        private void openFolder(object sender, RoutedEventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        /* Open schema folder and reload files */
        private void uploadSchema(object sender, RoutedEventArgs e)
        {
            NewSchema newSchemaWindow = new NewSchema(baseDirectory + "\\schemas\\");
            newSchemaWindow.Show();
        }

        /* Monitor COM ports for any changes and update combo */
        private async void monitorCOM()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        string[] openComs = SerialPort.GetPortNames();

                        Dispatcher.Invoke(new Action(() => {

                            //Check for new COMS
                            foreach (string com in openComs)
                            {
                                if (!comcombo.Items.Contains(com))
                                {
                                    comcombo.Items.Add(com);
                                }
                            }

                            //Check for disconnected COMS
                            foreach (string com in comcombo.Items.OfType<string>().ToList())
                            {
                                if (!openComs.Contains(com))
                                {
                                    comcombo.Items.Remove(com);
                                }
                            }

                            //Check selected COM
                            if (comcombo.SelectedItem != null && !isLogging)
                            {
                                //Check if a schema is selected
                                if (schemacombo.SelectedItem != null)
                                {
                                    startBut.IsEnabled = true;
                                }
                                
                            }
                            else
                            {
                                startBut.IsEnabled = false;
                            }
                        }));

                        Thread.Sleep(1000);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /* LOAD SCHEMAS FROM SCHEMA DIRECTORY */
        private async void monitorSchemas()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {

                        string[] schemaFiles = Directory.GetFiles(baseDirectory + "\\schemas\\");

                        //Check for new schemas
                        foreach (string schema in schemaFiles)
                        {
                            //Parse out filename
                            string fileName = schema.Split(new string[] { "\\" }, StringSplitOptions.None).Last();

                            if (!schemaNames.Contains(fileName))
                            {
                                schemaNames.Add(fileName);

                                //Build menu item and add to COM combobox
                                MenuItem newItem = new MenuItem { Header = fileName };
                                newItem.Click += selectSchema;
                                schemacombo.Items.Add(newItem);
                            }
                        }
                    }));

                    Thread.Sleep(1000);
                }
            });
        }

        /* Set status label */
        private void setStatus(string status)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                statusLabel.Content = (status);

            }));
        }

        /* Push serial stream line to monitor */
        private void setLabels(string serialInput)
        {
            double velocity = 0;
            double acceleration = 0;
            double altitude = 0;
            double latitude = 0;
            double longitude = 0;
            double roll = 0;
            double pitch = 0;
            double yaw = 0;

            string[] codes = schemaCode.Split(',');
            string[] inputVals = serialInput.Split(',');

            for (int i=0; i<codes.Length; i++)
            {
                if (codes[i] == ("velocity"))
                {
                    velocity = double.Parse(inputVals[i]);
                }
                else if (codes[i] == ("acceleration"))
                {
                    acceleration = double.Parse(inputVals[i]);
                }
                else if (codes[i] == ("altitude"))
                {
                    altitude = double.Parse(inputVals[i]);
                }
                else if (codes[i] == ("latitude"))
                {
                    latitude = double.Parse(inputVals[i]);
                }
                else if (codes[i] == ("longitude"))
                {
                    longitude = double.Parse(inputVals[i]);
                }
                else if (codes[i] == ("roll"))
                {
                    roll = double.Parse(inputVals[i]);
                }
                else if (codes[i] == ("pitch"))
                {
                    pitch = double.Parse(inputVals[i]);
                }
                else if (codes[i] == ("yaw"))
                {
                    yaw = double.Parse(inputVals[i]);
                }
            }

            monitorWindow.modifyValues(velocity, acceleration, altitude, latitude, longitude, roll, pitch, yaw);
        }
    }
}
