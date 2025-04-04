using ConfigEditor.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConfigEditor.Utils
{
    public static class ECSDisplayAnalyzer
    {
        #region Attributes & Properties
        private static readonly IDictionary<Type, IDictionary<string, ECSDisplayAttribute>> _cache = new Dictionary<Type, IDictionary<string, ECSDisplayAttribute>>();
        #endregion


        #region Methods
        public static Type GetDisplayType(Type type)
        {
            // Safe design
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            ECSDisplaySchemaAttribute schemaAttr = type.GetCustomAttribute<ECSDisplaySchemaAttribute>();
            if (schemaAttr != null)
            {
                return schemaAttr.DisplayType;
            }
            else
            {
                return type;
            }
        }

        public static IDictionary<string, ECSDisplayAttribute> Analyze(Type type)
        {
            // Safe design
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            if (_cache.ContainsKey(type))
            {
                return _cache[type];
            }
            else
            {
                ECSDisplaySchemaAttribute schemaAttr = type.GetCustomAttribute<ECSDisplaySchemaAttribute>();
                Type displayType = type;
                if (schemaAttr != null)
                {
                    displayType = schemaAttr.DisplayType;
                }
                IDictionary<string, ECSDisplayAttribute> result = ExtractECSDisplayAttributes(displayType);

                _cache.Add(type, result);

                return result;
            }
        }

        private static IDictionary<string, ECSDisplayAttribute> ExtractECSDisplayAttributes(Type type)
        {
            var result = new Dictionary<string, ECSDisplayAttribute>();

            foreach (PropertyInfo property in ExtractProperties(type))
            {
                if (!result.ContainsKey(property.Name))
                {
                    ECSDisplayAttribute dispAttr = property.GetCustomAttributes<ECSDisplayAttribute>(true).FirstOrDefault();

                    result.Add(property.Name, dispAttr);
                }
            }

            return result;
        }

        private static IEnumerable<PropertyInfo> ExtractProperties(Type type)
        {
            if (!type.IsInterface)
            {
                return type.GetProperties().Where(prop => prop.IsDefined(typeof(ECSDisplayAttribute), true));
            }
            else
            {
                var propertyInfos = new List<PropertyInfo>();

                var listType = new List<Type>();
                var aTraiter = new List<Type>();

                listType.Add(type);
                aTraiter.Add(type);
                while (aTraiter.Count > 0)
                {
                    Type subType = aTraiter[0];

                    foreach (Type subInterface in subType.GetInterfaces())
                    {
                        if (!listType.Contains(subInterface))
                        {
                            listType.Add(subInterface);
                            aTraiter.Add(subInterface);
                        }
                    }

                    PropertyInfo[] typeProperties = subType.GetProperties();

                    IEnumerable<PropertyInfo> newPropertyInfos = typeProperties.Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);

                    aTraiter.RemoveAt(0);
                }

                return propertyInfos.ToArray().Where(prop => prop.IsDefined(typeof(ECSDisplayAttribute), true)); ;
            }
        }
        #endregion
    }
}