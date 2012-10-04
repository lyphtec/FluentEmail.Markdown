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
        /// <summary>
        /// Instantiates a new instance of the Markdown renderer
        /// </summary>
        public MarkdownRenderer()
        {
            RenderHtml = true;
        }

        /// <summary>
        /// Indicate whether or not to render the output has Html. Default is true.
        /// </summary>
        public bool RenderHtml { get; set; }
        
        private static readonly object SyncLock = new object();

        public string Parse<TModel>(string markdownTemplate, TModel model)
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

            var result = mdFormat.RenderDynamicPage(mdPage, scopeArgs, this.RenderHtml, false);

            // clean up temp file
            fi.Delete();

            return result;
        }
    }
}
