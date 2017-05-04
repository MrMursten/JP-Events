using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace JP___Events
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Event[] events;
        public string selectedEventPeriod;

        public MainWindow()
        {
            InitializeComponent();
            string jsonString = new WebClient().DownloadString("http://events.makeable.dk/api/getEvents");

            JsonSerializer serializer = new JsonSerializer();

            Rootobject root = JsonConvert.DeserializeObject<Rootobject>(jsonString);

            events = root.events;

            listBox.ItemsSource = events;
            listBox.DisplayMemberPath = "title";

            ComboboxItem s1 = new ComboboxItem();
            s1.Text = "Last Added";
            s1.Value = "Last";

            ComboboxItem s2 = new ComboboxItem();
            s2.Text = "Start Time";
            s2.Value = "Time";

            ComboboxItem s3 = new ComboboxItem();
            s3.Text = "Alphabetically";
            s3.Value = "Title";

            cbSort.Items.Add(s1);
            cbSort.Items.Add(s2);
            cbSort.Items.Add(s3);

            ComboboxItem c1 = new ComboboxItem();
            c1.Text = "Any";
            c1.Value = "Any";

            ComboboxItem c2 = new ComboboxItem();
            c2.Text = "Music";
            c2.Value = "Musik";

            ComboboxItem c3 = new ComboboxItem();
            c3.Text = "City Life";
            c3.Value = "Byliv";

            ComboboxItem c4 = new ComboboxItem();
            c4.Text = "Exhibition";
            c4.Value = "Udstilling";

            cbCategory.Items.Add(c1);
            cbCategory.Items.Add(c2);
            cbCategory.Items.Add(c3);
            cbCategory.Items.Add(c4);

            cbSort.SelectedIndex = 0;
            cbCategory.SelectedIndex = 0;
        }

        public void updateListBox()
        {
            Event[] newEvents = events;

            if (cbCategory.SelectedItem != null &&  cbSort.SelectedItem != null)
            {
                //Filter
                string filterString = (cbCategory.SelectedItem as ComboboxItem).Value;
                if (filterString == "Musik")
                {
                    List<Event> listEvents = new List<Event>();
                    foreach (Event e in newEvents)
                    {
                        if (e.category == "Musik")
                        {
                            listEvents.Add(e);
                        }
                    }
                    newEvents = listEvents.ToArray<Event>();
                }
                else if (filterString == "Byliv")
                {
                    List<Event> listEvents = new List<Event>();
                    foreach (Event e in newEvents)
                    {
                        if (e.category == "Byliv")
                        {
                            listEvents.Add(e);
                        }
                    }
                    newEvents = listEvents.ToArray<Event>();
                }
                else if (filterString == "Udstilling")
                {
                    List<Event> listEvents = new List<Event>();
                    foreach (Event e in newEvents)
                    {
                        if (e.category == "Udstilling")
                        {
                            listEvents.Add(e);
                        }
                    }
                    newEvents = listEvents.ToArray<Event>();
                }

                //Sort
                string sortString = (cbSort.SelectedItem as ComboboxItem).Value;
                if (sortString == "Time")
                {
                    newEvents = newEvents.OrderBy(x => x.datelist.Length != 0 ? x.datelist[0].start : long.MinValue).ToArray();
                }
                else if (sortString == "Title")
                {
                    newEvents = newEvents.OrderBy(x => x.title).ToArray();
                }

                listBox.ItemsSource = newEvents;
                listBox.DisplayMemberPath = "title";
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                Event ev = (Event)listBox.SelectedItem;
                long startMilis = ev.datelist.Length != 0 ? (ev.datelist[0].start) : 0;
                long endMilis = ev.datelist.Length != 0 ? (ev.datelist[0].end) : 0;

                String start = startMilis > 0 ? new DateTime(startMilis * 10000).ToString() : "";
                String end = endMilis > 0 ? new DateTime(endMilis * 10000).ToString() : "";
                selectedEventPeriod = start + " - " + end;
                txtTime.Text = selectedEventPeriod;
            }
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateListBox();
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateListBox();
        }
    }
}
