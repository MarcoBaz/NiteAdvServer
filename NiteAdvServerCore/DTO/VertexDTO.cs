using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO
{
    public class VertexDTO
    {
        public string? OriginalId { get; set; }
        public string? Label { get; set; }
        public System.DateTime LastSyncDate { get; set; }
        public int ActionResult { get; set; }
        public string? ActionError { get; set; }
    }
}
