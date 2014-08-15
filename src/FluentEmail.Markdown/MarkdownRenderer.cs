using System.Collections.Generic;
using System.Globalization;
using System.IO;
using RazorEngine;
using MD = MarkdownDeep.Markdown;

namespace FluentEmail.Markdown
{
    /// <summary>
    /// Markdown renderer
    /// </summary>
    public class MarkdownRenderer : ITemplateRenderer
    {
        public string Parse<TModel>(string markdownTemplate, TModel model, bool isHtml = true)
        {
            if (string.IsNullOrWhiteSpace(markdownTemplate))
                return string.Empty;

            var rzOut = Razor.Parse(markdownTemplate, model, markdownTemplate.GetHashCode().ToString(CultureInfo.InvariantCulture));

            if (!isHtml) return rzOut;

            var md = new MD
            {
                ExtraMode = true,
                SafeMode = false
            };

            return md.Transform(rzOut);
        }
    }
}
