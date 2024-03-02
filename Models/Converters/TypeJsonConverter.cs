using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HRIS_Software.Models.Converters
{
    internal sealed class TypeJsonConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string name = reader.GetString();
            if (!string.IsNullOrEmpty(name))
                return Type.GetType(name);
            return default;
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            string name = value.AssemblyQualifiedName;
            if (!string.IsNullOrEmpty(name))
                writer.WriteStringValue(name);
            else
                writer.WriteNullValue();
        }
    }
}
