using Maps.Entities;
using System.Collections.Generic;
using System.IO;

namespace Maps.Utils
{
    public abstract class DataReader
    {
        public abstract bool LoadFile(Stream stream, Layer layer, ref IList<string> messages);
    }
}
