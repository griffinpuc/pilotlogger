using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PILOTLOGGER {

    public partial class MainWindow : Window
    {
        LogBox logbox;
        static SerialPort _serialPort = new SerialPort();
        private BlockingCollection<string> OutputQueue = new BlockingCollection<string>();
        Dictionary<string, CancellationTokenSource> threads = new Dictionary<string, CancellationTokenSource>();

        public MainWindow()
        {

            //MessageBox.Show("PILOT LOGGER \nv1.0");

            logbox = new LogBox();

            InitializeComponent();
            loadSchemas();
            loadOutputDir();
            monitorCOM();
        }

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

            while (true)
            {
                if (!token.IsCancellationRequested)
                {
                    serialline = _serialPort.ReadExisting();
                    OutputQueue.Add(serialline);

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
                foreach(var p in o) { schema += p.Value + ","; }

                strm.WriteLine(schema);
                foreach (var s in OutputQueue.GetConsumingEnumerable())
                {
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

                        System.Threading.Thread.Sleep(1000);
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
