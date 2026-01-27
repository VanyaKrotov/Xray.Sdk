using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xray.Config.Utilities;

static class EnumPropertyConverter
{
    public static string ToString<T>(T value) where T : struct, Enum
    {
        var field = typeof(T).GetField(value.ToString());
        var attr = field?.GetCustomAttributes(typeof(EnumProperty), false).Cast<EnumProperty>()?.FirstOrDefault();

        return attr?.Alias ?? value.ToString();
    }

    public static T FromString<T>(string value) where T : struct, Enum
    {
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var field in fields)
        {
            var attributes = field.GetCustomAttributes(typeof(EnumProperty), false).Cast<EnumProperty>().Select(a => a.Alias);
            if (attributes != null && attributes.Contains(value))
            {
                return (T)field.GetValue(null)!;
            }
        }

        return Enum.Parse<T>(value, true);
    }
}

public class SplitEnumConverter<T> : JsonConverter<List<T>> where T : struct, Enum
{
    public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString()!.Split(",").Select(EnumPropertyConverter.FromString<T>).ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(string.Join(",", value.Select(EnumPropertyConverter.ToString)));
    }
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public sealed class EnumProperty : Attribute
{
    public string Alias { get; }

    public EnumProperty(string alias) => Alias = alias;
}

public class UniversalEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    private readonly Dictionary<string, T> _fromString;
    private readonly Dictionary<T, string> _toString;

    public UniversalEnumConverter()
    {
        _fromString = new(StringComparer.OrdinalIgnoreCase);
        _toString = new();

        foreach (var field in typeof(T).GetFields().Where(f => f.IsStatic))
        {
            var value = (T)field.GetValue(null)!;

            _fromString[field.Name] = value;

            var aliases = field.GetCustomAttributes(typeof(EnumProperty), false).Cast<EnumProperty>().Select(a => a.Alias);
            foreach (var alias in aliases)
            {
                _fromString[alias] = value;
            }

            _toString[value] = aliases.FirstOrDefault() ?? field.Name;
        }
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (str != null && _fromString.TryGetValue(str, out var value))
        {
            return value;
        }

        throw new JsonException($"Unknown value '{str}' for enum {typeof(T).Name}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_toString[value]);
    }
}

public class UniversalEnumConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsEnum;

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(UniversalEnumConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}
