using System;

namespace Umfrage_Tool
{
    public class Message
    {
        public string siteTitle { get; set; }
        public string mainMessage { get; set; }
        public string additionalInformation { get; set; }
        public bool useLayout { get; set; }
        public bool allowReturn { get; set; }
    }
}