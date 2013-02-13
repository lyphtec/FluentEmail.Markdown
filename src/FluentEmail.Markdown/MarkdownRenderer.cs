using System.Collections.Generic;
using System.IO;
using ServiceStack.WebHost.Endpoints.Formats;
using ServiceStack.WebHost.Endpoints.Support.Markdown;

namespace FluentEmail.Markdown
{
    /// <summary>
    /// Markdown renderer
    /// </summary>
    public class MarkdownRenderer : ITemplateRenderer
    {
        private static readonly object SyncLock = new object();

        public string Parse<TModel>(string markdownTemplate, TModel model, bool isHtml = true)
        {
            if (string.IsNullOrWhiteSpace(markdownTemplate))
                return string.Empty;

            var file = Path.GetTempFileName();

            lock (SyncLock)
            {
                File.WriteAllText(file, markdownTemplate);
            }

            var fi = new FileInfo(file);
            if (fi.Length == 0)
            {
                fi.Delete();
                return string.Empty;
            }

            var mdFormat = new MarkdownFormat();
            var mdPage = new MarkdownPage(mdFormat, file, "_markdown_", markdownTemplate);

            mdFormat.AddPage(mdPage);

            // attach view model
            var scopeArgs = new Dictionary<string, object> { { MarkdownPage.ModelName, model } };

            var result = mdFormat.RenderDynamicPage(mdPage, scopeArgs, isHtml, false);

            // clean up temp file
            fi.Delete();

            return result;
        }
    }
}
