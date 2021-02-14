using System;
using System.Collections.Generic;
using System.Dynamic;

namespace SdcaFramework.Utilities
{
    public static class DynamicMethods
    {
        public static dynamic DictionaryToObject(Dictionary<string, object> dictionary)
        {
            IDictionary<string, object> dynamicObject = new ExpandoObject();
            foreach (KeyValuePair<string, object> kvp in dictionary)
            {
                dynamicObject.Add(kvp);
            }
            return dynamicObject;
        }
    }
}
