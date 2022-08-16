using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities
{
    internal class NavigationObject:VertexEntity
    {
        public string type { get; set; }
        public string inVLabel { get; set; }
        public string outVLabel { get; set; }
        public string inV { get; set; }
        public string outV { get; set; }

        public override string label => "edge";
    }

    // "id": "83b59118-4926-40cd-99e1-ca59393d69e8",
    //"label": "organized_by",
    //"type": "edge",
    //"inVLabel": "company",
    //"outVLabel": "event",
    //"inV": "10852735578",
    //"outV": "3463333240411808" --company
}
