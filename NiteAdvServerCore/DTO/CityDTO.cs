using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO
{
    public class CityDTO
    {
        public CityDTO(int id, string description, int deep)
        {
            Id = id;
            Description = description;
            Deep = deep;
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int Deep { get; set; }
    }
}
