using Noel.Common;
using Noel.Common.Data;
using Noel.Common.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Edit
{
    class EditApi
    {
        [HttpGet]
        public object Filetree()
        {
            return NoelEnvironment.Instance.Seasons
                .Select(x => new {
                    x.Number,
                    Files = x.TranslationFilenames
                }).ToArray();
        }

        [HttpGet]
        public TranslationFile TranslationFile(int seasonNum, string filename)
        {
            return NoelEnvironment.Instance.TranslationFileCache.Get(seasonNum, filename);
        }

        [HttpGet]
        public List<object> FullTextSearch(string term)
        {
            term = term.ToUpperInvariant();

            var result = new List<object>();
            if (string.IsNullOrWhiteSpace(term))
                return result;

            foreach (var season in NoelEnvironment.Instance.Seasons)
            {
                foreach (var filename in season.TranslationFilenames)
                {
                    var cache = NoelEnvironment.Instance.TranslationFileCache;
                    var workingFile = cache.Get(season.Number, filename);
                    result.AddRange(workingFile.Strings.SelectMany(x => new[] { x.SourceValue, x.PatchValue })
                        .Where(x => x != null && x.ToUpperInvariant().Contains(term))
                        .Select(x => new { SeasonNum = season.Number, Filename = filename, Match = x }));
                }
            }

            return result;
        }

        [HttpPost]
        public void TranslationFile(TranslationFile file)
        {
            NoelEnvironment.Instance.TranslationFileCache.Update(file);
        }
    }
}
