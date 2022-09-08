using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO
{
    public class EventDTO:VertexDTO
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string ImageLink { get; set; }
        public string CompanyId { get; set; }
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
    }
}
