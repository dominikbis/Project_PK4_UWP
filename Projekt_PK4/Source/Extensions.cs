using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_PK4
{

    static class ObservableCollectionExtension
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> otherCollection)
        {
            foreach (T item in otherCollection)
            {
                collection.Add(item);
            }
        }
    }

    public static class EnumExtensions//jest ale nigdzie na razie tego nie uzywam
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attr.Length == 0 ? value.ToString() : (attr[0] as DescriptionAttribute).Description;
        }
    }

}
