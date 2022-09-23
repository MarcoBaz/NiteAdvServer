using NiteAdvServerCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Generic
{
    

    public class EventResponse
    {
        public EventResponse()
        {
            EventList = new List<Event>();
        }
        public List<Event> EventList { get; set; }
        public int ItemsCount { get; set; }
    }
}
