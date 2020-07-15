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

namespace PILOTLOGGER {

    public partial class MainWindow : Window
    {
        LogBox logbox;
        static SerialPort _serialPort = new SerialPort();
        private BlockingCollection<string> OutputQueue;
        Dictionary<string, CancellationTokenSource> threads = new Dictionary<string, CancellationTokenSource>();

        public MainWindow()
        {
            InitializeComponent();
            loadSchemas();
            loadOutputDir();
            monitorCOM();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        //START OF NEW REFRACTORED CODE

        SerialPort serialPort;
        string workingDirectory;
        string userDocumentsPath;
        string schemaCode;
        int schemaValueCount;

        private void applicationStartup()
        {
            workingDirectory = Directory.GetCurrentDirectory();
            userDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //set box with path docs

            newloadSchemas();
            monitorCOM();
        }

        /* LOAD SCHEMAS FROM SCHEMA DIRECTORY */
        private void newloadSchemas()
        {
            //Find each schema file in directory
            foreach (string file in Directory.GetFiles(workingDirectory + "\\schemas"))
            {
                //Parse out filename
                string fileName = file.Split(new string[] { "\\" }, StringSplitOptions.None).Last();

                //Build menu item and add to COM combobox
                MenuItem item = new MenuItem { Header = fileName };
                item.Click += selectSchema;
                schemacombo.Items.Add(item);
            }
        }

        /* COM monitoring startup tasks (pre monitoring) */
        private void startComMonitor()
        {
            //Set serialport options for PILOT RC
            serialPort = new SerialPort()
            {
                PortName = comcombo.SelectedItem.ToString(),
                BaudRate = 11520,
                DtrEnable = true,
                NewLine = "\n"
            };

            //Write to file in background as data streams in
            string outputFilename = userDocumentsPath + "PILOTLOG_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var outputTask = Task.Factory.StartNew(() => writeFile(outputFilename), TaskCreationOptions.LongRunning);

            //Monitor the serial port in the background with cancel token
            CancellationTokenSource cts = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(new WaitCallback(monitorSerialPort), cts.Token);
            threads.Add(comcombo.SelectedItem.ToString(), cts);

        }

        //COM monitoring closing tasks (post monitoring)
        private void endComMonitor()
        {

        }

        /* CSV File output method */
        private async void writeFile(string fileName)
        {
            using (var strm = File.AppendText(fileName))
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

        //COM monitor background task
        private async void monitorCom(Object obj)
        {
            CancellationToken token = (CancellationToken)obj;

            while (true)
            {
                if (!token.IsCancellationRequested)
                {
                    string readLine = serialPort.ReadLine();
                    //if(readLine.Split())
                }
                else
                {
                    break;
                }
            }

            endComMonitor();

        }

        //END OF NEW REFRACTORED CODE

        public void loadSchemas()
        {
            foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\schemas"))
            {
                string x = file.Split(new string[] { "\\" }, StringSplitOptions.None).Last();
                MenuItem item = new MenuItem{ Header = x };
                item.Click += selectSchema;
                schemacombo.Items.Add(item);
            }
        }

        public void loadOutputDir()
        {
            outputBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void selectSchema(object sender, RoutedEventArgs e)
        {
            schemacombo.SelectedItem = sender;
            schemacombo.IsDropDownOpen = false;
        }

        private void startLog(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            comcombo.IsEnabled = false;

            OutputQueue = new BlockingCollection<string>();

            logbox = new LogBox();
            logbox.Show();
            startBut.IsEnabled = false;

            _serialPort.PortName = comcombo.SelectedItem.ToString();
            setStatus("Logging...");

            ThreadPool.QueueUserWorkItem(new WaitCallback(monitorSerialPort), cts.Token);
            threads.Add(comcombo.SelectedItem.ToString(), cts);

            stopBut.IsEnabled = true;
        }

        private void stopLog(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource cts = threads.Where(pair => pair.Key.Equals(comcombo.SelectedItem.ToString())).Select(pair => pair.Value).FirstOrDefault();
            try
            {
                logbox.Close();
                _serialPort.Close();
                cts.Cancel();
                Thread.Sleep(1500);
                cts.Dispose();
                threads.Remove(_serialPort.PortName);
                comcombo.IsEnabled = true;
                stopBut.IsEnabled = false;

                setStatus("Not logging");
                MessageBox.Show("Log saved!");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void openFolder(object sender, RoutedEventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        private void uploadSchema(object sender, RoutedEventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + "\\schemas");
        }


        private async void monitorSerialPort(Object obj)
        {
            var outputTask = Task.Factory.StartNew(() => logCOM("PILOTLOG_" + DateTime.Now.ToString("yyyyMMddHHmmss")), TaskCreationOptions.LongRunning);
            CancellationToken token = (CancellationToken)obj;
            string serialline;

            _serialPort.BaudRate = 115200;
            _serialPort.DtrEnable = true;
            _serialPort.Open();

            _serialPort.NewLine = "\n";
            _serialPort.ReadLine();

            while (true)
            {
                if (!token.IsCancellationRequested)
                {
                    serialline = _serialPort.ReadLine();

                    OutputQueue.Add(serialline.Replace('\t', ','));
                    Dispatcher.Invoke(new Action(() => {
                        logbox.consolelog.AppendText(serialline);
                        logbox.consolelog.ScrollToEnd();
                    }));

                }
                else
                {
                    OutputQueue.CompleteAdding();
                    outputTask.Wait();
                    break;
                }

                Thread.Sleep(50);
            }
        }

        void logCOM(string fname)
        {

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = System.IO.Path.Combine(path, fname + ".csv");
            string schemaname = "";

            Dispatcher.Invoke(new Action(() =>
            {
                schemaname = schemacombo.Text;
            }));

            using (var strm = File.AppendText(subFolderPath))
            {
                    JObject o = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\schemas\\" + schemaname));

                string schema = "";
                foreach(var p in o) {
                    schema += p.Value + ",";
                }

                logbox.setSeries(schema.Remove(schema.Length - 1));

                strm.WriteLine(schema);
                foreach (var s in OutputQueue.GetConsumingEnumerable())
                {
                    string[] vals = s.Split(',');
                    logbox.setSeriesLine(vals.Take(vals.Count() - 1).ToArray());

                    strm.WriteLine(s);

                    strm.Flush();
                }
            }
        }

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
                            foreach (string com in openComs)
                            {
                                if (!comcombo.Items.Contains(com))
                                {
                                    comcombo.Items.Add(com);
                                }
                            }
                            foreach (string com in comcombo.Items.OfType<string>().ToList())
                            {
                                if (!openComs.Contains(com))
                                {
                                    comcombo.Items.Remove(com);
                                }
                            }

                            if (comcombo.SelectedItem != null)
                            {

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

        private void setStatus(string status)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                statusLabel.Content = (status);

            }));
        }
    }
}
