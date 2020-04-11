using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Data.Instructions
{
    [JsonConverter(typeof(Converter))]
    public abstract class PatchInstruction
    {
        public virtual void Apply(Dictionary<string, TranslationString> allStrings, TranslationString currentString)
        {
        }

        public string InstructionType => GetType().Name;

        private sealed class Converter : JsonConverter
        {
            private PatchInstruction Create(Type _, JObject jObject)
            {
                var typename = jObject.Value<string>("instructionType") ?? jObject.Value<string>("InstructionType");
                if (typeCache.TryGetValue(typename, out var type))
                    return type();
                
                throw new InvalidOperationException($"Invalid InstructionType '{typename ?? "null"}'");
            }

            private static readonly Dictionary<string, Func<PatchInstruction>> typeCache = typeof(PatchInstruction)
                .Assembly.GetTypes()
                .Where(x => !x.IsAbstract && typeof(PatchInstruction).IsAssignableFrom(x))
                .ToDictionary(x => x.Name, MakeFactory);

            private static Func<PatchInstruction> MakeFactory(Type type)
            {
                return Expression.Lambda<Func<PatchInstruction>>(
                    Expression.Convert(
                        Expression.New(type.GetConstructor(Type.EmptyTypes)),
                        typeof(PatchInstruction))).Compile();
            }
            
            public override bool CanWrite { get { return false; } }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotSupportedException();

            public override bool CanConvert(Type objectType)
            {
                return typeof(PatchInstruction).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                    return null;

                var jObject = JObject.Load(reader);
                var target = Create(objectType, jObject);
                serializer.Populate(jObject.CreateReader(), target);

                return target;
            }
        }
    }
}
