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
    public class JsonDataReader : DataReader
    {
        public override bool LoadFile(Stream stream, Layer layer, ref IList<string> messages)
        {
            try
            {
                JSchema schema = CreateSchema(layer.Id, ref messages);
                if (schema == null)
                {
                    return false;
                }
                using (StreamReader file = new StreamReader(stream))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JArray array = JToken.ReadFrom(reader) as JArray;
                    if (!array.IsValid(schema, out messages))
                    {
                        return false;
                    }
                    StoreData(array, layer);

                    return true;
                }
            }
            catch (Exception ex)
            {
                messages.Add(ex.Message);
            }
            return false;
        }

        private static readonly Dictionary<UserDataType, string> UserTypeToJsonType = new Dictionary<UserDataType, string>
        {
            { UserDataType.STRING,  "string"},
            { UserDataType.NUMBER, "number"},
            { UserDataType.LONGITUDE, "number"},
            { UserDataType.LATITUDE, "number"},
        };

        private JSchema CreateSchema(Guid id, ref IList<string> messages)
        {
            try
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

                    JSchema schema = JSchema.Parse(schemaString);
                    return schema;
                }
            }
            catch (Exception ex)
            {
                messages.Add(ex.Message);
                return null;
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
