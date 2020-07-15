﻿using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PILOTLOGGER
{

    public partial class Monitor : Window
    {

        private List<string> schemaCodeList;
        private LineSeries[] serialValues;
        public SeriesCollection chartSeries;

        public Monitor()
        {
            InitializeComponent();

            schemaCodeList = new List<string>();
            serialValues = new LineSeries[schemaCodeList.Count];

        }

        public void initChart()
        {
            chartSeries = new SeriesCollection();
            chart.LegendLocation = LegendLocation.Right;
            chart.ChartLegend.Visibility = Visibility.Visible;
        }

        public void setValues(string schemaCode)
        {
            int index = 0;
            string[] schemaCodes = schemaCode.Split(',');
            serialValues = new LineSeries[schemaCodes.Length];

            foreach (string code in schemaCodes)
            {
                schemaCodeList.Add(code);

                LineSeries newSeries = new LineSeries();
                newSeries.Name = code;
                newSeries.Values = new ChartValues<double>() { 0 };
                newSeries.Title = code;

                serialValues[index] = newSeries;

                Dispatcher.Invoke(new Action(() =>
                {
                    MenuItem item = new MenuItem { Header = code };
                    item.Click += setGraphValue;
                    graphcombo.Items.Add(item);
                }));

                index++;
            }
        }

        public void modifyContents(string labelText)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                testlabel.Content = labelText.Replace(",", "\n");
            }));
        }

        public void setChartDefault()
        {
            chartSeries.Add(serialValues[0]);
        }

        public void setGraphValue(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                graphcombo.SelectedItem = sender;
                graphcombo.IsDropDownOpen = false;
                chartSeries.Clear();
                chartSeries.Add(serialValues[graphcombo.SelectedIndex]);
            }));
        }

        private void selectValue(object sender, RoutedEventArgs e)
        {
            graphcombo.SelectedItem = sender;
            graphcombo.IsDropDownOpen = false;
        }

        public void addValues(string values)
        {
            int x = values.Split(',').Length;
            int y = serialValues.Length;

            if (values.Split(',').Length == serialValues.Length)
            {
                int index = 0;
                foreach (string value in values.Split(','))
                {
                    IChartValues chartValues = serialValues[index].Values;
                    chartValues.Add(double.Parse(value));

                    if (chartValues.Count > 80)
                    {
                        chartValues.RemoveAt(0);
                    }

                    index++;
                }

                Dispatcher.Invoke(new Action(() =>
                {
                    chart.Series = chartSeries;
                }));
            }

            Console.WriteLine();
        }

    }
}
