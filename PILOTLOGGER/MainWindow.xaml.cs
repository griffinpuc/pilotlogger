﻿using LiveCharts.Wpf;
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

        SerialPort serialPort;
        string workingDirectory;
        string userDocumentsPath;
        string schemaCode;
        int schemaValueCount;

        //Output queue for file writeout
        private BlockingCollection<string> OutputQueue;

        //Keep track of threads for serial monitor
        Dictionary<string, CancellationTokenSource> threads = new Dictionary<string, CancellationTokenSource>();

        public MainWindow()
        {
            InitializeComponent();
            applicationStartup();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void applicationStartup()
        {
            workingDirectory = Directory.GetCurrentDirectory();
            userDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //set box with path docs

            loadSchemas();
            monitorCOM();
        }

        /* LOAD SCHEMAS FROM SCHEMA DIRECTORY */
        private void loadSchemas()
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
        private void startComMonitor(object sender, RoutedEventArgs e)
        {
            //Set serialport options for PILOT RC
            serialPort = new SerialPort()
            {
                PortName = comcombo.SelectedItem.ToString(),
                BaudRate = 115200,
                DtrEnable = true,
                NewLine = "\r" //Newline character (Test script uses /r)
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

            await Task.Run(() =>
            {

                serialPort.Open();

                while (true)
                {
                    if (!token.IsCancellationRequested)
                    {
                        try
                        {
                            string readLine = serialPort.ReadLine();
                            string parsedLine = readLine.Replace("\t", ",");

                            Console.WriteLine(readLine);
                            OutputQueue.Add(parsedLine);
                        }
                        catch
                        {
                            Console.WriteLine("Thread exited");
                        }
                    }
                    else
                    {
                        OutputQueue.CompleteAdding();
                        outputTask.Wait();
                        break;
                    }
                }
            }); 
        }

        /* Load the output directory and set the label */
        public void loadOutputDir()
        {
            outputBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        /* Update schema combobox on selection */
        private void selectSchema(object sender, RoutedEventArgs e)
        {
            schemacombo.SelectedItem = sender;
            schemacombo.IsDropDownOpen = false;
        }

        /* Open the output folder */
        private void openFolder(object sender, RoutedEventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        /* Open schema folder and reload files */
        private void uploadSchema(object sender, RoutedEventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + "\\schemas");
            loadSchemas();
        }

        /* CSV File output method */
        private void writeFile(string fileName)
        {
            using (var strm = File.AppendText(Path.Combine(userDocumentsPath, fileName)))
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
                            if (comcombo.SelectedItem != null)
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

        /* Set status label */
        private void setStatus(string status)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                statusLabel.Content = (status);

            }));
        }
    }
}
