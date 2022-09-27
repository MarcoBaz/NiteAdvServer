using System;
using NiteAdvServerCore.Entities;

namespace NiteAdvServerCore.Generic
{
    public class FilterEvent
    {
        public FilterEvent()
        {
            Where = "";
        }

        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int Offset { get; set; }
        public string Where { get; set; }
        public string City { get; set; }
        public List<Event> CheckedEvents { get; set; }
        public Event EventToSave { get; set; }
    }
}

