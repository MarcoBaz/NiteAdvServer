using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities
{
    public class Event:VertexEntity
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public double StartDate { get; set; }
        //public double EndDate { get; set; }
        public double StartTimestamp { get; set; }
        public double EndTimestamp { get; set; }
        public string Image { get; set; }
        public override string label { get => "event"; }
    }
}


