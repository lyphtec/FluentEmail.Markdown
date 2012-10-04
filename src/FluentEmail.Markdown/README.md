This is a Markdown custom template renderer for [FluentEmail](https://github.com/lukencode/FluentEmail)   
It utilises the ITemplateRenderer interface exposed by FluentEmail to hook in [custom template renderers](http://lukencode.com/2012/06/10/fluent-email-now-supporting-custom-template-renderers/)

Markdown rendering is provided by the [Markdown Razor view engine](https://github.com/ServiceStack/ServiceStack/wiki/Markdown-Razor) that is built into [ServiceStack](https://github.com/ServiceStack/ServiceStack) and supports full model binding as available in the default Razor renderer.

#### Example Usage

```csharp
var email = Email
    .From("bob@hotmail.com")
    .To("somedude@gmail.com")
    .Subject("woo nuget")
	.UsingTemplateEngine(new MarkdownRenderer())
    .UsingTemplateFromFile(@"~/test.md", new { Name = "John Smith", Numbers = new string[] { "1", "2", "3" } });
```


test.md:

```markdown
# Heading 1

This is a [Markdown](http://mouapp.com) page

1. one
2. two
3. three

You can also bind to Model

Name: @Model.Name

Numbers:

@foreach i in Model.Numbers {
- Number: @i 
}

The current date is: @DateTime.Now
```

This will be the rendered output (Message.Body):

```html
<h1>Heading 1</h1>

<p>This is a <a href="http://mouapp.com">Markdown</a> page</p>

<ol>
<li>one</li>
<li>two</li>
<li>three</li>
</ol>

<p>You can also bind to Model</p>

<p>Name: John Smith</p>

<p>Numbers:</p>

<ul>
<li>Number: 1 </li>
<li>Number: 2 </li>
<li>Number: 3 </li>
</ul>

<p>The current date is: 04/10/2012 10:52:33 PM</p>
```

