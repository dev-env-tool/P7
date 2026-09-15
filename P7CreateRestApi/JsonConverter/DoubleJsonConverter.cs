//using System.Globalization;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Text.RegularExpressions;

//namespace P7CreateRestApi.JsonConverter
//{
//    public class DoubleJsonConverter : JsonConverter<double>
//    {
//        private static readonly Regex Pattern = new Regex(@"^(-?(\d+.?\d+|\d))+$", RegexOptions.Compiled); 
//        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            if (reader.TokenType == JsonTokenType.Number) { return reader.GetDouble(); } 
//            if (reader.TokenType == JsonTokenType.String) { var s = reader.GetString(); 
//            if (string.IsNullOrWhiteSpace(s)) throw new JsonException("STRICT_DOUBLE_INVALID_FORMAT"); 
//            if (!Pattern.IsMatch(s)) throw new JsonException("STRICT_DOUBLE_INVALID_FORMAT"); 
//            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var v)) 
//                    return v; 
//                throw new JsonException("STRICT_DOUBLE_INVALID_FORMAT"); } throw new JsonException("STRICT_DOUBLE_INVALID_FORMAT"); 
//            }
        
//            public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
//            => writer.WriteNumberValue(value);

//    }
//}