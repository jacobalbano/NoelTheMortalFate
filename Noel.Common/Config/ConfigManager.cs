using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Noel.Common.Config
{
    public class ConfigManager
    {
        internal static void Establish()
        {
            foreach (var type in GetAllDomainConfigMasterTypes())
            {
                var inst = Activator.CreateInstance(type);
                File.WriteAllText(Path.Combine("cfg", type.Name + ".json"), JsonConvert.SerializeObject(inst, Formatting.Indented));
                configCache[type] = inst;
            }
        }

        internal static void Load()
        {
            foreach (var type in GetAllDomainConfigMasterTypes())
            {
                var src = File.ReadAllText(Path.Combine("cfg", type.Name + ".json"));
                configCache[type] = JsonConvert.DeserializeObject(src, type);
            }
        }

        public static T GetDomainConfig<T>()
        {
            if (!configCache.TryGetValue(typeof(T), out var result))
                throw new Exception($"Invalid domain config '{typeof(T).Name}'");

            return (T)result;
        }

        private static IEnumerable<Type> GetAllDomainConfigMasterTypes()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetExportedTypes())
                    if (type.IsDefined(typeof(DomainConfigAttribute), false))
                        yield return type;
            }
        }


        private static readonly Dictionary<Type, object> configCache = new Dictionary<Type, object>();
    }
}
