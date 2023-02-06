using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ECR.StaticData.Converters
{
    public class Vector3Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            
            var jt = JToken.Load(reader);
            var parsedResult = JsonConvert.DeserializeObject<float[]>(jt.ToString());
            
            if (parsedResult != null) 
                return new Vector3(parsedResult[0], parsedResult[1], parsedResult[2]);
            
            Debug.LogError($"JSON Extensions: Failed to create Vector3: parsedResult is NULL");
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => 
            serializer.Serialize(writer, value);
    }
}