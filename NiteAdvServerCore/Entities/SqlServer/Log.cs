using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities
{
    public class Log : Entity
    {

        public Log()
        {
        }
        public string DockerID { get; set; }
        public DateTime Data { get; set; }
        public string Message { get; set; }
        public string LogType { get; set; }

    }
}
