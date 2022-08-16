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
    }
}
