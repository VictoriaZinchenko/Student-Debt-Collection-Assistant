using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SdcaFramework.Utilities
{
    public static class PropertiesDescriber
    {
        public static string GetActualAndExpectedObjectsProperties(object expectedObject, object actualObject)
        {
            string result = "\nExpected object:" + GetObjectProperties(expectedObject);
            result += "\nActual object:" + GetObjectProperties(actualObject);
            return result;
        }

        public static string GetActualObjectsListAndExpectedObjectProperties(object expectedObject, List<object> actualObjects)
        {
            string result = "\nExpected object" + GetObjectProperties(expectedObject);
            result += $"\nActual list of objects:" + GetPropertiesOfObjectsList(actualObjects);
            return result;
        }

        public static string GetObjectProperties(object targetObject)
        {
            string result = "\nProperties: ";
            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                result += $"\n{property.Name}: ";
                var neededProperty = property.GetValue(targetObject);
                if (property.PropertyType.IsGenericType)
                {
                    (neededProperty as IEnumerable).Cast<object>().ToList()
                        .ForEach(listItem => result += $"\n{listItem}");
                }
                else
                {
                    result += $" {neededProperty}. ";
                }
            }
            return result;
        }

        private static string GetPropertiesOfObjectsList(List<object> listOfObjects)
        {
            string result = null;
            for (int i = 0; i < listOfObjects.Count; i++)
            {
                result += $"\nObject №{i}" + GetObjectProperties(listOfObjects[i]);
            }
            return result;
        }
    }
}