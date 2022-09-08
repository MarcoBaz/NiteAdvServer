using System;
using NiteAdvServerCore.Entities;

namespace NiteAdvServerCore.Generic
{
    public class EventSaveViewModel
    {
        public EventSaveViewModel()
        {
        }
        public string IdUser { get; set; }
        public Event Event { get; set; }
    }
}

