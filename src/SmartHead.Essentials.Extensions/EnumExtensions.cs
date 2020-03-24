using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SmartHead.Essentials.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName<T>(this T e) where T : IConvertible
        {
            if (!(e is Enum)) return null;

            return e.GetType()
                       .GetMember(e.ToString(CultureInfo.InvariantCulture))
                       .FirstOrDefault()?
                       .GetCustomAttribute<DisplayAttribute>()?
                       .Name
                   ?? e.ToString();
        }
    }
}