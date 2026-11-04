using System.Text.Json;
using System.Text.Json.Serialization;

class JsonSupport
{
    public static JsonSerializerOptions Opzioni()
    {
        JsonSerializerOptions o = new JsonSerializerOptions();
        o.IncludeFields = true;
        o.PropertyNameCaseInsensitive = true;
        o.WriteIndented = true;
        o.Converters.Add(new JsonStringEnumConverter());
        return o;
    }
}
