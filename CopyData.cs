using System;
using System.Reflection;

namespace SpaghettiCodeStudios.Utility
{
    public class DataUtil
    {
        /// <summary>
        /// Copies all public field values from the source object to the target object.
        /// </summary>
        /// <param name="source">The source object from which field values are copied.</param>
        /// <param name="target">The target object to which field values are copied.</param>
        /// <typeparam name="T">The type of the objects involved in the copy operation.</typeparam>
        public static void CopyValues<T>(T source, T target)
        {
            Type type = source.GetType();
            
            foreach (FieldInfo field in type.GetFields())
            {
                field.SetValue(target, field.GetValue(source));
            }
        }
    }
}
