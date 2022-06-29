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
    table.AddColumn(new TableColumn("[dodgerblue2]Instructions[/]").LeftAligned());
    table.AddColumn(new TableColumn("[dodgerblue2]Categories[/]").Centered());
    // Add the Recipes to the table
    data.Recipes.ForEach(r => table.AddRow("[bold][red]" + r.Title + "[/][/]",
                                            ListLimitedView(ref data, r.Ingredients),
                                            ListLimitedView(ref data, r.Instructions),
                                            ListLimitedView(ref data, r.Categories)));
    AnsiConsole.Write(table);

    // User chooses the Action he would like to perform
    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("What [red]action[/] would you like to perform ?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
            "View Recipe","Add Recipe","Edit Recipe","Delete Recipe","Exit"
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
    List<string> ingredients = ListInput(ref data,"ingredients");
    List<string> instructions = ListInput(ref data, "instructions");
    List<string> categories = ListInput(ref data, "categories");
    data.addRecipe(new Recipe(title,ingredients,instructions,categories));

}

static void EditRecipe(ref Data data)
{
    var title = RecipeSelection(ref data, "Choose which recipe you would like to edit?");
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
    var title = RecipeSelection(ref data, "Choose which recipe you would like to delete?");
    data.removeRecipe(title);
}

static string RecipeSelection(ref Data data, string text)
{
    AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title(text)
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
        .AddChoices(data.Recipes.Select(r => r.Title)
        ));
    return text;
}

static List<string> ListInput(ref Data data, string text)
{
    AnsiConsole.Markup($"Please insert the [dodgerblue2]{text}[/] of your recipe \n");
    AnsiConsole.Markup("Press Enter after writing to add another\n If you are done write [red]Done[/] then press Enter \n");
    string input;
    List<string> inputList = new List<string>();
    for (int i = 0; i < 30; i++)
    {

        input = AnsiConsole.Ask<string>("- ");
        if (input == "Done")
            break;
        inputList.Add(input);

    }
    return inputList;
}

static string ListLimitedView(ref Data data, List<String> list)
{
    string result = string.Join(", ", list.ToArray());
    if (result.Length > 30)
        return result.Substring(0, 30)+"...";
    return result;
}