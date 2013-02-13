using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentEmail.Markdown.Tests
{
    public class MarkdownRendererFixture
    {
        private const string ToEmail = "bob@test.com";
        private const string FromEmail = "john@test.com";
        private const string Subject = "sup dawg";

        public Email GetEmail()
        {
            Email.DefaultRenderer = new MarkdownRenderer();

            var email = Email.From(FromEmail)
                             .To(ToEmail)
                             .Subject(Subject);

            return email;
        }
    }
}
