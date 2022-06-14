using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities
{
    public class City : BaseEntity
    {

        public City()
        {
        }
        public string Description { get; set; }
        public int Deep { get; set; }

    }
}
