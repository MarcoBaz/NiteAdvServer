using NiteAdvServerCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities;

public abstract class VertexEntity
{
    public string id { get; set; }
    public abstract string label { get; }
    public string partitionKey => "pk";
    public double LastSyncDate { get; set; }

}
