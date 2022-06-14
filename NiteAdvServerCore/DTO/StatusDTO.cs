using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO
{
    public  class StatusDTO:BaseDTO
    {
        public string DockerId { get; set; }
        public int IdStatusDescription { get; set; }
        public string DeviceIp { get; set; }
        public string Message { get; set; }
        public string City { get; set; }
    }
}
