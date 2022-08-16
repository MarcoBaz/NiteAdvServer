using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO
{
    public class CompanyDTO:VertexDTO
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool HasClaimed { get; set; }
        public string ImageUrl { get; set; }
        public string GooglePlaceId { get; set; }
        public string Type { get; set; }
        public string GoogleTypes { get; set; }
        public double Rating { get; set; }
        public string Reviews { get; set; }
        public string GoogleUrl { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string OpeningHours { get; set; }
        public double RatingTotal { get; set; }
    }
}
