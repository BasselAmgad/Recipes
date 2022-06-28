// See https://aka.ms/new-console-template for more information
using Spectre.Console;




// Instantiate our data from json file
Data data = new Data();

while (true)
{
    AnsiConsole.Write(
    new FigletText("Recipes")
        .LeftAligned()
        .Color(Color.Red));
    // Create a table
    var table = new Table().Border(TableBorder.Ascii2);
    table.Expand();
    // Create Table Columns
    table.AddColumn("[dodgerblue2]Title[/]");
    table.AddColumn(new TableColumn("[dodgerblue2]Ingredients[/]").Centered());
    table.AddColumn(new TableColumn("[dodgerblue2]Instuctions[/]").LeftAligned());
    table.AddColumn(new TableColumn("[dodgerblue2]Categories[/]").Centered());
    // Add the Recipes to the table
    data.Recipes.ForEach(r => table.AddRow("[bold][red]" + r.Title + "[/][/]", r.Ingredients, r.Instructions, string.Join(", ", r.Categories.ToArray())).AddEmptyRow());
    AnsiConsole.Write(table);

    // User chooses the Action he would like to perform
    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("What [red]action[/] would you like to perform ?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
            "Add Recipe","Edit Recipe","Delete Recipe","Exit"
            }));

    switch (choice)
    {
        case "Add Recipe":
            AddRecipe(ref data);
            break;
        case "Edit Recipe":
            EditRecipe(ref data);
            break;
        case "Delete Recipe":
            DeleteRecipe(ref data);
            break;
        default: break;
    }

    data.saveRecipes();
    AnsiConsole.Clear();
    if (choice == "Exit")
        break;
}




static void AddRecipe(ref Data data)
{
    var title = AnsiConsole.Ask<string>("What is the [dodgerblue2]title[/] of your recipe?");
    var ingredients = AnsiConsole.Ask<string>("What are the needed [dodgerblue2]ingredients[/]?");
    var instructions = AnsiConsole.Ask<string>("What are the  [dodgerblue2]instuctions[/]?");
    List<string> categories = new List<string>();
    AnsiConsole.Markup("Please insert the [dodgerblue2]categories[/] of your recipe");
    AnsiConsole.Markup("Press Enter after writing the category to add another, if you are done write [red]Done[/] then press Enter");
    for (int i=0;i<10;i++){
        
        var category = AnsiConsole.Ask<string>("Category: ");
        categories.Add(category);
        if (category == "Done")
            break;
    }
    data.addRecipe(new Recipe(title,ingredients,instructions,categories));

}

static void EditRecipe(ref Data data)
{
    var title = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Choose which recipe you would like to edit?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
        .AddChoices(data.Recipes.Select(r => r.Title)
        ));
    var toEdit = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Which attribute would you like to edit?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
        .AddChoices(new[] {
            "Title","Ingredients","Instructions","Categories"
            
        }));
    if(toEdit != "Categories")
    {
        var newText = AnsiConsole.Ask<string>($"Please enter the new [dodgerblue2]{toEdit}[/] of your recipe?");
        switch (toEdit)
        {
            case "Title":
                data.editTitle(title, newText);
                break;
            case "Ingredients":
                data.editIngredients(title, newText);
                break;
            case "Instructions":
                data.editInstructions(title, newText);
                break;
        }

    }
    else
    {
        CategoryChoiceMaker(ref data, title);
    }
    
}

static void CategoryChoiceMaker(ref Data data, string title)
{
    var choice = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("What would you like to do?")
       .PageSize(10)
       .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
       .AddChoices(new[] {
            "Add Category","Edit Category","Delete Category"

       }));
    string category;
    switch (choice)
    {
        case "Add Category":
            data.addCategory(title,AnsiConsole.Ask<string>("What is the name of your new [dodgerblue2]category[/]?"));
            break;

        case "Edit Category":
            category = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
                   .Title("Which Category would you like to edit?")
                   .PageSize(10)
                   .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                   .AddChoices(data.Recipes.Where(r => r.Title == title).ToList()[0].Categories.ToArray()));
            data.editCategory(title,category, AnsiConsole.Ask<string>("What is the new name of the [dodgerblue2]category[/]?"));
            break;

        case "Delete Category":
            category = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
                   .Title("Which Category would you like to remove?")
                   .PageSize(10)
                   .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                   .AddChoices(data.Recipes.Where(r => r.Title == title).ToList()[0].Categories.ToArray()));
            data.removeCategory(title, category);
            break;
        default:break;
    }

}


static void DeleteRecipe(ref Data data)
{
    var title = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Choose which recipe you would like to delete?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
        .AddChoices(data.Recipes.Select(r => r.Title)
        ));
    data.removeRecipe(title);
}
