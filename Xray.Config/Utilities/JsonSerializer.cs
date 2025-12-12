using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xray.Config.Models;

namespace Xray.Config.Utilities;

public static class XrayConfigJsonSerializer
{
    private static JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static string Serialize(XRayConfig config)
    {
        return JsonSerializer.Serialize(config, _options);
    }

    public static XRayConfig Deserialize(string json)
    {
        return JsonSerializer.Deserialize<XRayConfig>(json)!;
    }
}

static class EnumMemberConvert
{
    public static string ToString<T>(T value) where T : struct, Enum
    {
        var field = typeof(T).GetField(value.ToString()!);
        var attr = field?.GetCustomAttribute<EnumMemberAttribute>();

        return attr?.Value ?? value.ToString();
    }

    public static T FromString<T>(string value) where T : struct, Enum
    {
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attr = field.GetCustomAttribute<EnumMemberAttribute>();
            if (attr?.Value == value)
                return (T)field.GetValue(null)!;
        }

        return Enum.Parse<T>(value!, true);
    }
}

public class EnumMemberConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return EnumMemberConvert.FromString<T>(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var field = typeof(T).GetField(value.ToString()!);
        var attr = field?.GetCustomAttribute<EnumMemberAttribute>();

        writer.WriteStringValue(attr?.Value ?? value.ToString());
    }
}

public class SplitEnumConverter<T> : JsonConverter<List<T>> where T : struct, Enum
{
    public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString()!.Split(",").Select(EnumMemberConvert.FromString<T>).ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(string.Join(",", value.Select(EnumMemberConvert.ToString<T>)));
    }
}