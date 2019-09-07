using Maps.Data;
using Maps.Entities;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Maps.Utils
{
    /// <summary>
    /// Abstract class that represents reader of the data. The idea is that various implementations 
    /// read different types of files.
    /// </summary>
    public abstract class DataReader
    {
        /// <summary>
        /// Loads data from the given file to the database and returns whether the 
        /// action was a success. If it wasn't list of messages will be populated with 
        /// errors.
        /// </summary>
        /// <param name="stream">Stream of the file from which the data will be read.</param>
        /// <param name="layer">The layer the data belong to.</param>
        /// <param name="messages">List of error messages which will be populated in the case of failure.</param>
        /// <returns></returns>
        public abstract bool LoadFile(Stream stream, Layer layer, ref IList<string> messages);

        /// <summary>
        /// Creates default filters for all columns of the given layer.
        /// </summary>
        /// <param name="layer">Creates filters for columns of this layer.</param>
        protected void CreateFilters(Layer layer)
        {
            using (var access = new DataAccess())
            {
                IList<Entities.Data> data = access.Data.Get(d => d.Layer.Id == layer.Id).ToList();

                foreach (var column in access.Columns.Get(c => c.Layer.Id == layer.Id).ToList())
                {
                    if (column.HasChart)
                    {
                        var uniqueValues = data.GroupBy(d => d.Values[column.Name]).Select(grp => new { Name = grp.Key });
                        foreach (var unique in uniqueValues)
                        {
                            column.Filter[unique.Name.ToString()] = false;
                        }
                    }
                    else if (column.DataType == UserDataType.NUMBER)
                    {
                        column.Filter["min"] = 0;
                        column.Filter["max"] = 0;
                    }

                    access.Columns.Update(column);
                }

                access.Save();
            }
        }

        /// <summary>
        /// Calculates chart data for the columns of the given layer that have HasChart set.
        /// </summary>
        /// <param name="layer">Calulates chart data for the columns of this layer.</param>
        protected void CreateCharts(Layer layer)
        {
            using (var access = new DataAccess())
            {
                IList<Entities.Data> data = access.Data.Get(d => d.Layer.Id == layer.Id).ToList();

                foreach (var column in access.Columns.Get(c => c.Layer.Id == layer.Id).ToList())
                {
                    if (column.HasChart)
                    {
                        var res = data.GroupBy(d => d.Values[column.Name])
                                .Select(grp => new
                                {
                                    label = grp.Key,
                                    count = grp.Select(d => d.Values[column.Name]).Count()
                                });
                        column.Chart["labels"] = new JArray(res.Select(r => r.label).ToArray());
                        column.Chart["counts"] = new JArray(res.Select(r => r.count).ToArray());

                        access.Columns.Update(column);
                    }
                }

                access.Save();
            }
        }
    }
}