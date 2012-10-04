# Heading 1

This is a [Markdown](http://mouapp.com) page

1. one
2. two
3. three

You can also bind to Model

Name: @Model.Name

Numbers:

@foreach n in Model.Capitals {
- @n.Key : @n.Value 
}

Do cool stuff like get the current date: @DateTime.Now