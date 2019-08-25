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
                using (StreamReader file = new StreamReader(stream))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JArray array = JToken.ReadFrom(reader) as JArray;
                    JSchema schema = JSchema.Parse(array[0].ToString());

                    if (!GetColumns(schema, layer, out IList<Column> columns, ref messages))
                    {
                        return false;
                    }
                    array.RemoveAt(0);
                    if (!array.IsValid(schema, out messages))
                    {
                        return false;
                    }
                    using (var access = new DataAccess())
                    {
                        access.Columns.BulkInsert(columns);
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

        private static readonly Dictionary<JSchemaType?, UserDataType> NameToType = new Dictionary<JSchemaType?, UserDataType>
        {
            { JSchemaType.String, UserDataType.STRING},
            { JSchemaType.Number, UserDataType.NUMBER},
        };

        private bool GetColumns(JSchema schema, Layer layer, out IList<Column> columns, ref IList<string> messages)
        {
            columns = new List<Column>();
            if (schema.Type != JSchemaType.Array)
            {
                messages.Add("File must be a JSON array with first element as JSON schema");
                return false;
            }
            var itemSchema = schema.Items;
            if (itemSchema.Count != 1 || itemSchema[0].Type != JSchemaType.Object)
            {
                messages.Add("Only one type of user objects is allowed");
                return false;
            }
            foreach (var property in itemSchema[0].Properties)
            {
                var typeName = property.Value.Type;
                if (!NameToType.TryGetValue(typeName, out UserDataType type))
                {
                    messages.Add("Type '" + typeName + "' is not allowed");
                    return false;
                }
                else
                {
                    Column column = new Column
                    {
                        Layer = layer,
                        Id = Guid.NewGuid(),
                        Name = property.Key,
                        DataType = type
                    };
                    columns.Add(column);
                }
            }
            return CheckColumns(columns, ref messages);
        }

        private bool CheckColumns(IList<Column> columns, ref IList<string> messages)
        {
            if (columns.GroupBy(c => c.Name).Select(c => c.First()).Count() != columns.Count())
            {
                messages.Add("All columns must have unique name");
                return false;
            }
            var longitude = columns.FirstOrDefault(c => c.Name == "longitude");
            if (longitude == null)
            {
                messages.Add("There is no longitude column");
                return false;
            }
            else
            {
                longitude.DataType = UserDataType.LONGITUDE;
            }
            var latitude = columns.FirstOrDefault(c => c.Name == "latitude");
            if (latitude == null)
            {
                messages.Add("There is no latitude column");
                return false;
            }
            else
            {
                latitude.DataType = UserDataType.LATITUDE;
            }
            return true;
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
