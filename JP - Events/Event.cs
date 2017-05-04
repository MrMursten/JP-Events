using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JP___Events
{

    public class Rootobject
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int last_change { get; set; }
        public Event[] events { get; set; }
    }

    public class Event
    {
        public string category { get; set; }
        public string category_id { get; set; }
        public Datelist[] datelist { get; set; }
        public string description { get; set; }
        public string description_english { get; set; }
        public string description_german { get; set; }
        public string eventgroup { get; set; }
        public string eventid { get; set; }
        public string family_friendly { get; set; }
        public int last_updated { get; set; }
        public string location_address { get; set; }
        public string location_city { get; set; }
        public string location_id { get; set; }
        public object location_latitude { get; set; }
        public object location_longitude { get; set; }
        public string location_name { get; set; }
        public string location_postcode { get; set; }
        public string organizer_email { get; set; }
        public string organizer_name { get; set; }
        public string organizer_phone { get; set; }
        public string picture_name { get; set; }
        public string price { get; set; }
        public string subcategory { get; set; }
        public string subcategory_id { get; set; }
        public string subtitle { get; set; }
        public string subtitle_english { get; set; }
        public string subtitle_german { get; set; }
        public string tags { get; set; }
        public string tickets_url { get; set; }
        public string title { get; set; }
        public string title_english { get; set; }
        public string title_german { get; set; }
        public string url { get; set; }
        public string user_id { get; set; }
        public string video_url { get; set; }

        public string dateStart {
            get
            {
                DateTime start = new DateTime(this.datelist[0].start * 1000);
                return start.ToString();
            }
        }
    }

    public class Datelist
    {
        public int start { get; set; }
        public int end { get; set; }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

}
