using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Linq;

namespace DarwinFeed
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            Departures = new ObservableCollection<DestinationRowViewModel>();
            message = "Not Yet Updated";
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
#endif
            guiThreadDispatcher = Dispatcher.CurrentDispatcher;
            updateTimer = new System.Threading.Timer(UpdateDisplay, this, 1000, Properties.Settings.Default.UpdateFequency);
            tmrCurrentTimeDisplay = new DispatcherTimer();
            tmrCurrentTimeDisplay.Interval = TimeSpan.FromSeconds(1);
            tmrCurrentTimeDisplay.Tick += tmrCurrentTimeDisplay_Tick;
            tmrCurrentTimeDisplay.Start();
        }

        void tmrCurrentTimeDisplay_Tick(object sender, EventArgs e)
        {
            OnPropertyChanged("CurrentTime");
            OnPropertyChanged("TimeSinceUpdate");
        }

        public void UpdateDisplay(object state)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
#endif
            DarwinDataStore.TheDataStore.UpdateDepartureBoard();
            List<DestinationRowViewModel> railservices = DarwinDataStore.TheDataStore.TheDepartureBoard.GetRailServicesVM();
            guiThreadDispatcher.BeginInvoke(new Action(() => guiThreadUpdate(railservices)));
            
        }

        private void guiThreadUpdate(List<DestinationRowViewModel> toAdd)
        {            
            buildMessages(DarwinDataStore.TheDataStore.TheDepartureBoard.NrccMessages);
            AreServicesAvailable = DarwinDataStore.TheDataStore.TheDepartureBoard.AreServicesAvailable;
            Departures.Clear();
            IEnumerable<DestinationRowViewModel> departuresInTheFure = toAdd.Where(x => ((x.LikelyDeptarureTime.HasValue) && (x.LikelyDeptarureTime.Value > DateTime.Now)));
            foreach (DestinationRowViewModel vm in departuresInTheFure)            
                Departures.Add(vm);
            timeOfLastUpdate = DateTime.Now;            
        }
        private void buildMessages(List<string> messages)
        {
            StringBuilder sbMessages = new StringBuilder();
            foreach (string line in messages)            
                sbMessages.AppendLine(line);            
            Message = sbMessages.ToString();
        }

        public ObservableCollection<DestinationRowViewModel> Departures { get; private set; }

        private string message;
        public string Message
        {
            get 
            {
                return message;
            }
            set 
            {
                if (value != message)
                {
                    message = value;
                    OnPropertyChanged("Message");
                }
            }
        }
        private bool areServicesAvailable;
        public bool AreServicesAvailable
        {
            get
            {
                return areServicesAvailable;
            }
            set
            {
                if (value != areServicesAvailable)
                {
                    areServicesAvailable = value;
                    OnPropertyChanged("AreServicesAvailable");
                }
            }
        }

        private DateTime? timeOfLastUpdate;
        public string TimeSinceUpdate
        {
            get
            {
                if (timeOfLastUpdate == null)
                    return "Never Updated";
                return string.Format("Updated {0} seconds ago", (DateTime.Now - timeOfLastUpdate).Value.Seconds);
            }
        }

        public string CurrentTime
        {
            get
            {
                return DateTime.Now.ToLongTimeString();
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly System.Threading.Timer updateTimer;
        private readonly Dispatcher guiThreadDispatcher;
        private readonly DispatcherTimer tmrCurrentTimeDisplay;



    }
}
