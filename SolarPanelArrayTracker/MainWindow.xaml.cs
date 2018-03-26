using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SolarPanelArrayTracker.GPS;
using SolarPanelArrayTracker.Scan;

namespace SolarPanelArrayTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Variables

        private SynchronizationContext currentContext;
        private TaskManager<GPSService, GPSPositionChangedEventArgs> gpsTracker;
        private TaskManager<ScanService, ScanReceivedEventArgs> scanTracker;
        private Random randomIntervalValueGenerator;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            this.Title.Text = @"Solar Panel Tracking ""Buggy"" Application";
            this.Width = 900;
            this.Height = 700;
            this.currentContext = SynchronizationContext.Current;
            this.randomIntervalValueGenerator = new Random();
        }

        #endregion

        #region Event Handlers

        private void scanTracker_NotifyEvent(object sender, ScanReceivedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(
               () =>
               {
                   ScansReadings.Items.Add(e.ScanType + " : " + e.ScanValue + " - " + e.Defect);
               }));
        }

        private void gpsTracker_NotifyEvent(object sender, GPSPositionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(
                () =>
                {
                    GPSReadings.Items.Add(String.Format("Latitude: {0} Longitude: {1}", e.Latitude, e.Longitude));
                }));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Start();
        }

        private void GPSSlider_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.gpsTracker.Interval = this.GetIntervalValue((Slider)sender);
        }

        private int GetIntervalValue(Slider sender)
        {
            return this.randomIntervalValueGenerator.Next(Convert.ToInt32(sender.Minimum), Convert.ToInt32(sender.Value));
        }

        private void ScansSlider_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.scanTracker.Interval = this.GetIntervalValue((Slider)sender);
        }

        #endregion

        #region Common

        public async void Start()
        {
            try
            {
                // Create Tasks
                this.gpsTracker = new TaskManager<GPSService, GPSPositionChangedEventArgs>(interval: Convert.ToInt32(this.GPSSlider.Value));
                this.scanTracker = new TaskManager<ScanService, ScanReceivedEventArgs>(interval: Convert.ToInt32(this.ScansSlider.Value));

                #region Standard Events

                // Attach Events
                //gpsTracker.NotifyEvent += gpsTracker_NotifyEvent;
                //scanTracker.NotifyEvent += scanTracker_NotifyEvent; 

                #endregion

                #region Rx

                // Create Observables
                var gpsCoordinates = Observable
                                        .FromEventPattern<GPSPositionChangedEventArgs>(h => gpsTracker.NotifyEvent += h, h => gpsTracker.NotifyEvent -= h)
                                        .Select(ev => new { Latitude = ev.EventArgs.Latitude, Longitude = ev.EventArgs.Longitude })
                                        .ObserveOn(this.currentContext);

                var scans = Observable
                                .FromEventPattern<ScanReceivedEventArgs>(h => scanTracker.NotifyEvent += h, h => scanTracker.NotifyEvent -= h)
                                .Select(ev => new { ScanType = ev.EventArgs.ScanType, Value = ev.EventArgs.ScanValue, Defect = ev.EventArgs.Defect })
                                .ObserveOn(this.currentContext);

                // Join
                var dataEntryEvents = gpsCoordinates
                                                .Join(scans,                            // Join on Solar Panel scans stream
                                                        _ => gpsCoordinates,            // Closing gpsCoordinates window             
                                                        _ => Observable.Empty<Unit>(),  // Closing scans window
                                                        (coordinate, scan) => Tuple.Create(coordinate, scan));  //Result selector

                // Project Data to be saved in buffers
                var dataToSave = dataEntryEvents.Buffer(5);

                // Subscribe
                gpsCoordinates.Subscribe(position =>
                {
                    this.AddItemToListBox(GPSReadings, position.ToString());
                });

                scans.Subscribe(scan =>
                {
                    this.AddItemToListBox(ScansReadings, scan.ToString());
                });

                dataEntryEvents.Subscribe(output =>
                {
                    this.AddItemToListBox(DataEntryList, String.Format("{0} ==== {1}", output.Item1, output.Item2));
                });

                dataToSave.Subscribe(items => this.AddItemToListBox(DataSavedList, items.Count + " Items Saved!!! " + DateTime.Now.TimeOfDay.ToString()));

                #endregion

                //Start & Await
                await Task.WhenAll(new Task[] 
                { 
                    gpsTracker.Start(gpsTracker.Context.GetGPSPosition),
                    scanTracker.Start(scanTracker.Context.GetScan)
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void AddItemToListBox(ListBox listBox, string text)
        {
            ListBoxItem item = new ListBoxItem();
            item.Content = text;
            listBox.Items.Add(item);
            listBox.ScrollIntoView(item);
        }

        #endregion
    }
}
