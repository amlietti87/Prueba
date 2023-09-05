using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TECSO.FWK.Domain
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object obj) where T : class
        {
            return (T)obj;
        }

        public static T To<T>(this object obj) where T : struct
        {
            if (typeof(T) == typeof(Guid))
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
            return (T)Convert.ChangeType(obj, typeof(T), (IFormatProvider)CultureInfo.InvariantCulture);
        }

        public static bool IsIn<T>(this T item, params T[] list)
        {
            return ((IEnumerable<T>)list).Contains<T>(item);
        }
    }


}
