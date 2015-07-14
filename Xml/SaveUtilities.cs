using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Rzr.Core.Xml
{
    public class SavePropertyAttribute : Attribute
    {
        
    }

    public class SaveUtilities
    {
        public static void SaveTo(object source, Dictionary<string, string> target)
        {
            Type type = source.GetType();

            foreach (PropertyInfo info in type.GetProperties().Where(x =>
                x.GetCustomAttributes(typeof(SavePropertyAttribute), true).Count() > 0))
            {
                target[info.Name] = Convert.ToString(info.GetValue(source, null));
            }
        }

        public static void LoadFrom(object target, Dictionary<string, string> source)
        {
            Type type = target.GetType();

            foreach (PropertyInfo info in type.GetProperties().Where(x =>
                x.GetCustomAttributes(typeof(SavePropertyAttribute), true).Count() > 0))
            {
                if (source.ContainsKey(info.Name))
                    info.SetValue(target, SaveUtilities.GetValue(info, source[info.Name]), null);
            }
        }

        public static object GetValue(PropertyInfo info, object value)
        {
            if (info.PropertyType == typeof(float))
            {
                return Convert.ToSingle(value);
            }
            else
            {
                return Convert.ToString(value);
            }
        }
    }
}
