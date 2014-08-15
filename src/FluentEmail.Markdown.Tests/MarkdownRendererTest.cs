using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FluentEmail.Markdown.Tests
{
    public class MarkdownRendererTest : IUseFixture<MarkdownRendererFixture>
    {
        private Email _email;

        [Fact]
        public void Anonymous_Model_Plain_Matches()
        {
            var template = "# sup @Model.Name";
            
            var email = _email.UsingTemplate(template, new { Name = "LUKE" }, false);

            Assert.Equal("# sup LUKE", email.Message.Body);
        }

        [Fact]
        public void Anonymous_Model_Html_Matches()
        {
            var template = "# sup @Model.Name";
            
            var email = _email.UsingTemplate(template, new { Name = "LUKE" });

            Assert.Equal("<h1>sup LUKE</h1>\n", email.Message.Body);
        }

        [Fact]
        public void Anonymous_Model_With_List_Matches()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Hello @Model.Name");
            sb.AppendLine();
            sb.AppendLine("@foreach (var n in Model.Numbers) {");
            sb.AppendLine("@: - @n");
            sb.AppendLine("}");
            sb.AppendLine();
            
            var email = _email.UsingTemplate(sb.ToString(), new { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } });

            Console.Write(email.Message.Body);

            Assert.True(email.Message.Body.Length > 0);
            Assert.True(email.Message.Body.Contains("<li>3</li>"));
        }

        [Fact]
        public void Anonymous_Model_With_Dictionary_From_File_Matches()
        {
            var email = _email.UsingTemplateFromFile(@"~/test.md", new
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

        #region IUseFixture<MarkdownRendererFixture> Members

        public void SetFixture(MarkdownRendererFixture data)
        {
            _email = data.GetEmail();
        }

        #endregion
    }
}
