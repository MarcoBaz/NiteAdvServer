using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NiteAdvServerCore.Managers;
using NiteAdvServerCore.Util;

namespace NiteAdvServerCore.Entities
{
    public class Event:VertexEntity
    {
        public Event() : base("", "event")
        { }
        public Event(string id) : base(id, "event")
        {
        }


        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double StartTimestamp { get; set; }
        public double EndTimestamp { get; set; }
        public string Image { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string CategoryType { get; set; }
        public string DiscoveryCategories { get; set; }
        public string Place { get; set; }
        public string TicketingContext { get; set; }
        public string TicketUrl { get; set; }
        public double UsersGoing { get; set; }
        public double UsersInterested { get; set; }
        public bool AllDay { get; set; }
        public bool IsInBlackList { get; set; }
        public bool Deleted { get; set; }
    }


}

