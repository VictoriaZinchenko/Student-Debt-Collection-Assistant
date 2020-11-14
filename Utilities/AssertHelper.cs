using System.Collections.Generic;
using System.Reflection;

namespace SdcaFramework.Utilities
{
    public class AssertHelper
    {
        public string GetActualAndExpectedObjectsProperties(object expectedObject, object actualObject)
        {
            string result = null;
            foreach (PropertyInfo property in expectedObject.GetType().GetProperties())
            {
                var expectedProperty = property.GetValue(expectedObject);
                var actualProperty = actualObject.GetType().GetProperty(property.Name).GetValue(actualObject);
                result = result + $"\nExpected property: {property.Name} - {expectedProperty}. " +
                    $"Actual property: {property.Name} - {actualProperty}.";
            }
            return result;
        }

        public string GetActualObjectsListAndExpectedObjectProperties(object expectedObject, List<object> actualObjects)
        {
            string result = null;
            result = result + "\nExpected object";
            foreach (PropertyInfo property in expectedObject.GetType().GetProperties())
            {
                var expectedProperty = property.GetValue(expectedObject);
                result = result + $"\n{property.Name} - {expectedProperty}.";
            }
            result = result + $"\nActual list:";

                for (int i = 0; i < actualObjects.Count; i++)
                {
                    result = result + $"\nElement №{i}";
                    foreach (PropertyInfo property in actualObjects[i].GetType().GetProperties())
                    {
                        var actualProperty = actualObjects[i].GetType().GetProperty(property.Name).GetValue(actualObjects[i]);
                        result = result +
                            $"\n{property.Name} - {actualProperty}.";
                    }
                }
            return result;
        }
    }
}
