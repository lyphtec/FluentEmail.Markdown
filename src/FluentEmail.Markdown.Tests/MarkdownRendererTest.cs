using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentEmail.Markdown.Tests
{
    public class MarkdownRendererTest
    {
        private const string ToEmail = "bob@test.com";
        private const string FromEmail = "john@test.com";
        private const string Subject = "sup dawg";

        private MarkdownRenderer _renderer;

        public MarkdownRendererTest()
        {
            _renderer = new MarkdownRenderer();
        }

        [Fact]
        public void Anonymous_Model_Plain_Matches()
        {
            var template = "# sup @Model.Name";

            _renderer.RenderHtml = false;

            var email = Email.From(FromEmail)
                .To(ToEmail)
                .Subject(Subject)
                .UsingTemplateEngine(_renderer)
                .UsingTemplate(template, new { Name = "LUKE" }, false /* isHtml - Need to be able to pass this down the the renderer!! */ );

            Assert.Equal("# sup LUKE", email.Message.Body);
        }

        [Fact]
        public void Anonymous_Model_Html_Matches()
        {
            var template = "# sup @Model.Name";

            _renderer.RenderHtml = true;

            var email = Email.From(FromEmail)
                .To(ToEmail)
                .Subject(Subject)
                .UsingTemplateEngine(_renderer)
                .UsingTemplate(template, new { Name = "LUKE" });

            Assert.Equal("<h1>sup LUKE</h1>\n", email.Message.Body);
        }

        [Fact]
        public void Anonymous_Model_With_List_Matches()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Hello @Model.Name");
            sb.AppendLine();
            sb.AppendLine("@foreach n in Model.Numbers {");
            sb.AppendLine("- @n");
            sb.AppendLine("}");
            sb.AppendLine();

            _renderer.RenderHtml = true;

            var email = Email
                .From(FromEmail)
                .To(ToEmail)
                .Subject(Subject)
                .UsingTemplateEngine(_renderer)
                .UsingTemplate(sb.ToString(), new { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } });

            Console.Write(email.Message.Body);

            Assert.True(email.Message.Body.Length > 0);
            Assert.True(email.Message.Body.Contains("<li>3</li>"));
        }

        [Fact]
        public void Anonymous_Model_With_Dictionary_From_File_Matches()
        {
            _renderer.RenderHtml = true;
            
            var email = Email
                .From(FromEmail)
                .To(ToEmail)
                .Subject(Subject)
                .UsingTemplateEngine(_renderer)
                .UsingTemplateFromFile(@"~/test.md", new
                                                         {
                                                             Name = "LUKE", 
                                                             Capitals = new Dictionary<string,string>
                                                                             {
                                                                                 {"WA", "Perth"}, 
                                                                                 {"VIC","Melbourne"}, 
                                                                                 {"NSW", "Sydney"}
                                                                             }
                                                         });

            Console.Write(email.Message.Body);

            Assert.True(email.Message.Body.Length > 0);
            Assert.True(email.Message.Body.Contains("<h1>"));
            Assert.True(email.Message.Body.Contains("<li>VIC : Melbourne </li>"));
        }
    }
}
