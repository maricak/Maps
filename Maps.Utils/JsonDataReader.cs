using Maps.Data;
using Maps.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Maps.Utils
{
    /// <summary>
    /// Represents a data read from the JSON files.
    /// </summary>
    public class JsonDataReader : DataReader
    {
        public override bool LoadFile(Stream stream, Layer layer, ref IList<string> messages)
        {
            // Creates validation schema from the columns of the layer.
            JSchema schema = CreateSchema(layer.Id);
            if (schema == null)
            {
                return false;
            }

            using (StreamReader file = new StreamReader(stream))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JArray array = JToken.ReadFrom(reader) as JArray;
                // Check whether the data is valid.
                if (!array.IsValid(schema, out messages))
                {
                    return false;
                }
                StoreData(array, layer);
                CreateCharts(layer);
                CreateFilters(layer);
                return true;
            }
        }

        private static readonly Dictionary<UserDataType, string> UserTypeToJsonType = new Dictionary<UserDataType, string>
        {
            { UserDataType.STRING,  "string"},
            { UserDataType.NUMBER, "number"},
            { UserDataType.LONGITUDE, "number"},
            { UserDataType.LATITUDE, "number"},
        };

        private JSchema CreateSchema(Guid id)
        {
            using (var access = new DataAccess())
            {
                var columns = access.Columns.Get(c => c.Layer.Id == id).ToList();
                if (columns == null || columns.Count() == 0)
                {
                    return null;
                }

                string schemaString = @"{'type':'array', 'items': {'type': 'object', 'properties': {";
                foreach (var column in columns)
                {
                    schemaString += string.Format("'{0}':{{'type':'{1}'}},", column.Name, UserTypeToJsonType[column.DataType]);
                }

                schemaString += "},'required':[";
                foreach (var column in columns)
                {
                    schemaString += string.Format("'{0}',", column.Name);
                }

                schemaString += "]}}";

                // Since properties cannot be set programmatically the schema will be parsed from the created string.
                JSchema schema = JSchema.Parse(schemaString);
                return schema;
            }
        }

        private void StoreData(JArray array, Layer layer)
        {
            List<Entities.Data> data = new List<Entities.Data>();
            foreach (var item in array)
            {
                Entities.Data d = new Entities.Data
                {
                    Id = Guid.NewGuid(),
                    Layer = layer,
                    Values = JObject.Parse(item.ToString())
                };
                data.Add(d);
            }

            using (var access = new DataAccess())
            {
                access.Data.BulkInsert(data);
            }
        }
    }
}