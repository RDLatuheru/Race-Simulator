using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DriverStatistics _driverStatisticsWindow;
        private RaceStatistics _raceStatisticsWindow;

        public MainWindow()
        {
            Data.Initialize(false);
            Data.CurrentRace.DriversChanged += DriversChangedHandler;
            Data.CurrentRace.NextRace += NextRaceHandler;

            InitializeComponent();
        }

        public void DriversChangedHandler(object s, DriversChangedEventArgs e)
        {
            Track.Dispatcher.BeginInvoke(
            DispatcherPriority.Render,
            new Action(() =>
            {
                Track.Source = null;
                Track.Source = WpfVisualisatie.DrawTrack(e.Track);
            }));
        }

        public void NextRaceHandler(object s, NextRaceEventsArgs e)
        {
            Data.CurrentRace.DriversChanged -= DriversChangedHandler;
            Data.CurrentRace.NextRace -= NextRaceHandler;
            ImageProcessor.ClearCache();
            Data.NextRace();
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += DataContextMain.DriversChangedHandler;
                Data.CurrentRace.DriversChanged += DriversChangedHandler;
                Data.CurrentRace.NextRace += NextRaceHandler;
                Data.CurrentRace.Start();
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_DriverStatistics_Click(object sender, RoutedEventArgs e)
        {
            _driverStatisticsWindow = new DriverStatistics();
            _driverStatisticsWindow.Show();
        }

        private void MenuItem_RaceStatistics_Click(object sender, RoutedEventArgs e)
        {
            _raceStatisticsWindow = new RaceStatistics();
            _raceStatisticsWindow.Show();
        }
    }
}
