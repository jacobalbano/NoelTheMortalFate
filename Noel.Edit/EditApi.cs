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

        [HttpPost]
        public void TranslationFile(TranslationFile file)
        {
            NoelEnvironment.Instance.TranslationFileCache.Update(file);
        }
    }
}
