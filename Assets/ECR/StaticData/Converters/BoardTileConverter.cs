using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using ECR.StaticData.Board;

namespace ECR.StaticData.Converters
{
    public class BoardTileConverter : JsonConverter<BoardTileStaticData>
    {
        public override BoardTileStaticData ReadJson(JsonReader reader, Type objectType, BoardTileStaticData existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            
            var jt = JToken.Load(reader);
            var parsedResult = JsonConvert.DeserializeObject<int[]>(jt.ToString());

            if (parsedResult != null)
                return new BoardTileStaticData
                {
                    Tile = (BoardTileType)parsedResult[0],
                    TileRotation = (BoardTileRotation)parsedResult[1],
                    Position = new Vector2Int(parsedResult[2], parsedResult[3])
                };
            
            Debug.LogError($"JSON Extensions: Failed to create BoardTileStaticData: parsedResult is NULL");
            return null;    
        }


        public override void WriteJson(JsonWriter writer, BoardTileStaticData value, JsonSerializer serializer)
        {
            var tmp = new int[4]
            {
                (int) value.Tile,
                (int) value.TileRotation,
                value.Position.x, value.Position.y
            };
            serializer.Serialize(writer, tmp);
        }
    }
}