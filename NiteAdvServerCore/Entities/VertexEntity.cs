using Gremlin.Net.Structure;
using Newtonsoft.Json;
using NiteAdvServerCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace NiteAdvServerCore.Entities;

public abstract class VertexEntity
{
    protected VertexEntity(string id, string label) 
    {
        this.id = id;
        this.label = label;
    }
    public string partitionKey => "pk";
    // [System.Text.Json.Serialization.JsonConverter(typeof(ArrayToSingleDoubleConverter))]
    // public abstract VertexEntityProperites properties { get; set; }
    public string id { get; set; }
    public string label { get; set; }
    public double LastSyncDate { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
