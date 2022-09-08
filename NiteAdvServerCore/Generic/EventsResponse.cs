using System;
using NiteAdvServerCore.Entities;

namespace NiteAdvServerCore.Generic
{
    public class EventsResponse
    {
 
        public EventsResponse()
        {
            EventsList = new List<Event>();
        }
        public List<Event> EventsList { get; set; }
        public int ItemsCount { get; set; }
    }
}

