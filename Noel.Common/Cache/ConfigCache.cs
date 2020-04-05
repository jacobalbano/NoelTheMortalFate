using Newtonsoft.Json;
using Noel.Common.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Noel.Common.Cache
{
    public sealed class ConfigCache : IDisposable
    {
        internal ConfigCache(bool firstRun)
        {
            var configDir = EnvironmentDir.ConfigDirectory;
            foreach (var type in GetAllConfigTypes())
            {
                if (firstRun)
                {
                    var inst = Activator.CreateInstance(type);
                    File.WriteAllText(Path.Combine(configDir, type.Name + ".json"), JsonConvert.SerializeObject(inst, Formatting.Indented));
                    configCache[type] = inst;
                }
                else
                {
                    var src = File.ReadAllText(Path.Combine(configDir, type.Name + ".json"));
                    configCache[type] = JsonConvert.DeserializeObject(src, type);
                }
            }
        }

        public T Get<T>()
        {
            if (!configCache.TryGetValue(typeof(T), out var result))
                throw new Exception($"Invalid domain config '{typeof(T).Name}'");

            return (T)result;
        }

        private IEnumerable<Type> GetAllConfigTypes()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetExportedTypes())
                    if (type.IsDefined(typeof(ConfigAttribute), true))
                        yield return type;
            }
        }

        void IDisposable.Dispose()
        {
            var configDir = EnvironmentDir.ConfigDirectory;
            foreach (var item in configCache)
                if (item.Key.IsDefined(typeof(LiveConfigAttribute), false))
                    File.WriteAllText(Path.Combine(configDir, item.Key.Name + ".json"), JsonConvert.SerializeObject(item.Value, Formatting.Indented));
        }

        private readonly Dictionary<Type, object> configCache = new Dictionary<Type, object>();
    }
}
