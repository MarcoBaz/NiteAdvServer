using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NiteAdvServerCore.Util;

public class ArrayToSingleStringConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        switch (token.Type)
        {
            case JTokenType.Array:
                return (string)token.Children<JValue>().FirstOrDefault();
            case JTokenType.String:
                return (string)token;
            case JTokenType.Null:
                return null;
            default:
                throw new JsonException("Unexpected token type: " + token.Type);
        }
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public class ArrayToSingleDoubleConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(double);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        switch (token.Type)
        {
            case JTokenType.Array:
                return (double)token.Children<JValue>().FirstOrDefault();
            case JTokenType.Integer:
                return (double)token;
            case JTokenType.Null:
                return null;
            default:
                throw new JsonException("Unexpected token type: " + token.Type);
        }
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public class ArrayToSingleBoolConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(bool);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        switch (token.Type)
        {
            case JTokenType.Array:
                return (bool)token.Children<JValue>().FirstOrDefault();
            case JTokenType.Boolean:
                return (bool)token;
            case JTokenType.Null:
                return null;
            default:
                throw new JsonException("Unexpected token type: " + token.Type);
        }
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}