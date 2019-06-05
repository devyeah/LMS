using System.Threading.Tasks;
using RazorLight;

namespace DevYeah.LMS.Business.Helpers
{
    public class RenderedEmailHelper
    {
        private readonly static RazorLightEngine _engine = new RazorLightEngineBuilder().UseMemoryCachingProvider().Build();

        private async static Task<string> ParseAsync(string templateKey, string template, TemplateModel model)
        {
            string result = null;
            var cacheResult = _engine.TemplateCache.RetrieveTemplate(templateKey);
            if (cacheResult.Success)
                result = await _engine.RenderTemplateAsync(cacheResult.Template.TemplatePageFactory(), model);
            else
                result = await _engine.CompileRenderAsync(templateKey, template, model);
            return result;
        }

        public static string Parse(string templateKey, string template, TemplateModel model)
        {
            return ParseAsync(templateKey, template, model).GetAwaiter().GetResult();
        }
    }
}
