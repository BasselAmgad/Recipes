// See https://aka.ms/new-console-template for more information
using Spectre.Console;


AnsiConsole.Write(
    new FigletText("Recipes")
        .LeftAligned()
        .Color(Color.Red));

var Data = new Data();
Data.Recipes.ForEach(r=>Console.WriteLine(r.Title));
// Create a table
var table = new Table();

// Add some columns
table.AddColumn("Foo");
table.AddColumn(new TableColumn("Bar").Centered());

// Add some rows
table.AddRow("Baz", "[green]Qux[/]");
table.AddRow(new Markup("[blue]Corgi[/]"), new Panel("Waldo"));

// Render the table to the console
AnsiConsole.Write(table);

