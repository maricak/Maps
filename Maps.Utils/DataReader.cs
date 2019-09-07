using Maps.Entities;
using System.Collections.Generic;
using System.IO;

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

        protected void CreateFilters()
        {

        }
        protected void CreateCharts()
        {

        }
    }
}