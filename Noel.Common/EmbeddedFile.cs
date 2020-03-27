using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Noel.Common
{
    public static class EmbeddedFile
    {
        private static readonly Dictionary<string, string> textCache = new Dictionary<string, string>();
        private static readonly Dictionary<string, Func<Stream>> resourceStreams = new Dictionary<string, Func<Stream>>();

        static EmbeddedFile()
        {
            HeatCache(typeof(EmbeddedFile).Assembly);
            HeatCache(Assembly.GetEntryAssembly());
        }

        private static void HeatCache(Assembly asm)
        {
            var prefix = (asm.GetName().Name).Length;
            foreach (var name in asm.GetManifestResourceNames())
            {
                var key = name.Substring(prefix + 1).ToUpperInvariant();
                resourceStreams[key] = () => asm.GetManifestResourceStream(name);
            }
        }

        public static string GetTextFile(string filePath)
        {
            if (textCache.TryGetValue(filePath, out var result))
                return result;

            using (var stream = GetStream(filePath))
            using (var reader = new StreamReader(stream))
                result = reader.ReadToEnd();

            return result;
        }

        public static Stream GetStream(string filePath)
        {
            string key = GetResourceKey(filePath);
            if (!resourceStreams.TryGetValue(key, out var streamGetter))
                throw new Exception($"Couldn't find embedded resource '{filePath}' (generated resource key '{key}'");

            return streamGetter();
        }

        public static bool Exists(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filepath");
            return resourceStreams.TryGetValue(GetResourceKey(filePath), out _);
        }

        private static string GetResourceKey(string filePath)
        {
            return filePath.Replace('\\', '.').Replace('/', '.').ToUpperInvariant();
        }

        public static string[] GetTextLines(string filePath)
        {
            return GetTextFile(filePath).Split('\n')
                .Select(x => x.Trim('\r'))
                .ToArray();
        }
    }
}
