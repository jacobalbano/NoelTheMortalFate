using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Noel.Common
{
    public static class Embeds
    {
        private static readonly Dictionary<string, string> cache = new Dictionary<string, string>();
        private static readonly Dictionary<string, Func<Stream>> resourceStreams = new Dictionary<string, Func<Stream>>();

        static Embeds()
        {
            HeatCache(typeof(Embeds).Assembly);
            HeatCache(Assembly.GetExecutingAssembly());
        }

        private static void HeatCache(Assembly asm)
        {
            var prefix = (asm.GetName().Name).Length;
            foreach (var name in asm.GetManifestResourceNames())
            {
                var key = name.Substring(prefix).ToUpperInvariant() + ".";
                resourceStreams[key] = () => asm.GetManifestResourceStream(name);
            }
        }

        public static string GetTextFile(string filePath)
        {
            if (cache.TryGetValue(filePath, out var result))
                return result;

            var key = filePath.Replace('\\', '.').ToUpperInvariant();
            if (!resourceStreams.TryGetValue(key, out var resourceStream))
                throw new Exception($"Couldn't find embedded resource '{filePath}' (generated resource key '{key}'");

            using (var stream = resourceStream())
            using (var reader = new StreamReader(stream))
                result = reader.ReadToEnd();

            return result;
        }

        public static string[] GetTextLines(string filePath)
        {
            return GetTextFile(filePath).Split('\n')
                .Select(x => x.Trim('\r'))
                .ToArray();
        }
    }
}
