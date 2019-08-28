using Maps.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Maps.Utils
{
    public class SchemaUtils
    {
        public static bool CheckColumns(IList<CreateColumnViewModel> columns, ref IList<string> messages)
        {
            if (columns.GroupBy(c => c.Name).Count() != columns.Count())
            {
                messages.Add("All columns must have unique name.");
            }
            if (columns.Where(c => c.DataType == UserDataType.LATITUDE).ToList().Count() != 1)
            {
                messages.Add("There must be exactly one latitude column");
            }
            if (columns.Where(c => c.DataType == UserDataType.LONGITUDE).ToList().Count() != 1)
            {
                messages.Add("There must be exactly one latitude column");
            }

            return messages.Count() == 0;
        }
    }
}
