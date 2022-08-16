using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO
{
    public class LogDTO
    {
        public string DockerID { get; set; }
        public string Message { get; set; }
        public string LogType { get; set; }
    }
}
