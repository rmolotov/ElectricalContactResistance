using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ECR.StaticData.Converters
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return Vector3.zero;
            
            var jt = JToken.Load(reader);
            var parsedResult = JsonConvert.DeserializeObject<float[]>(jt.ToString());
            
            if (parsedResult != null) 
                return new Vector3(parsedResult[0], parsedResult[1], parsedResult[2]);
            
            Debug.LogError($"JSON Extensions: Failed to create Vector3: parsedResult is NULL");
            return Vector3.zero;
        }

        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            var tmp = new float[3]
            {
                value.x,
                value.y,
                value.z
            };
            serializer.Serialize(writer, tmp);
        }
    }
}