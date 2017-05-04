using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace JP_Events
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

    public class EventClient
    {
        public string Url { get; set; }
        public static readonly string CACHE_FILE = "Events.json";

        public EventClient(string url)
        {
            this.Url = url;
        }

        public Event[] GetEvents()
        {
            long cacheLastChanged = ExtractLastChange(CACHE_FILE);
            long remoteLastChanged = ExtractLastChange(new Uri(this.Url));

            string jsonString = null;
            if (cacheLastChanged == remoteLastChanged)
            {
                //Use Cache
                jsonString = File.ReadAllText(CACHE_FILE);
            }
            else
            {
                //Use remote
                WebClient client = new WebClient();
                jsonString = client.DownloadString(this.Url);
                CacheEvents(jsonString);
            }
            
            JsonSerializer serializer = new JsonSerializer();
            Rootobject root = JsonConvert.DeserializeObject<Rootobject>(jsonString);

            return root.events;
        }

        private void CacheEvents(string json)
        {
            File.WriteAllText(CACHE_FILE, json);
        }

        private long ExtractLastChange(Stream stream)
        {
            long lastChange = -1;
            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
            {
                while (jsonReader.Read() && lastChange == -1)
                {
                    JsonToken tokenType = jsonReader.TokenType;
                    if (tokenType == JsonToken.PropertyName && jsonReader.Value.Equals("last_change"))
                    {
                        jsonReader.Read();
                        lastChange = (long)jsonReader.Value;
                    }
                }
            }
            return lastChange;
        }

        private long ExtractLastChange(Uri uri)
        {
            WebClient client = new WebClient();
            using (var stream = client.OpenRead(uri))
            {
                return ExtractLastChange(stream);
            }
            
        }

        private long ExtractLastChange(string path)
        {
            long result = -1;
            if (File.Exists(path))
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    result = ExtractLastChange(stream);
                }
            }
            return result;
        }
    }

}
