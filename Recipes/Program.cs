// See https://aka.ms/new-console-template for more information
using Spectre.Console;
// Instantiate our data from json file
Data data = new Data();
while (true)
{

    RecipeTableView(ref data);
    // User chooses the Action he would like to perform
    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("What [red]action[/] would you like to perform ?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
            "View Recipe","Add Recipe","Edit Recipe","Delete Recipe","Exit"
            }));

    if (choice == "View Recipe")
    {
        // View recipes
        var recipeTitle = RecipeSelection(ref data, "Choose which recipe you would like to view: ");
        AnsiConsole.Clear();
        AnsiConsole.Write(
           new FigletText("Recipes")
       .LeftAligned()
       .Color(Color.Red));
        // detailed view of recipe
        var table = new Table().Border(TableBorder.Ascii2);
        table.Expand();
        table.AddColumn("[dodgerblue2]Title[/]");
        table.AddColumn(new TableColumn("[dodgerblue2]Ingredients[/]").LeftAligned());
        table.AddColumn(new TableColumn("[dodgerblue2]Instructions[/]").LeftAligned());
        table.AddColumn(new TableColumn("[dodgerblue2]Categories[/]").LeftAligned());
        // Add the details of the recipe to the table
        Recipe selectedRecipe = data.Recipes.FirstOrDefault(r => r.Title == recipeTitle);
        table.AddRow(selectedRecipe.Title,
                     String.Join("\n", selectedRecipe.Ingredients.Split(",").Select((x) => $"- {x}")),
                     String.Join("\n", selectedRecipe.Instructions.Split(",").Select((x, n) => $"{n + 1}. {x}")),
                     String.Join("\n", selectedRecipe.Categories.Select((x) => $"- {x}")));
        AnsiConsole.Write(table);
        choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("What [red]action[/] would you like to perform on this recipe?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
            "Edit Recipe","Delete Recipe","Back"
            }));
    }

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
    data.SaveRecipes();
    AnsiConsole.Clear();
    if (choice == "Exit")
        break;
}

static void AddRecipe(ref Data data)
{
    var title = AnsiConsole.Ask<string>("What is the [dodgerblue2]title[/] of your recipe?");
    string ingredients = MultiLineInput(ref data, "ingredients");
    string instructions = MultiLineInput(ref data, "instructions");
    List<string> categories = ListInput(ref data, "categories");
    data.AddRecipe(new Recipe(title, ingredients, instructions, categories));

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
    if (toEdit != "Categories")
    {
        var newText = AnsiConsole.Ask<string>($"Please enter the new [dodgerblue2]{toEdit}[/] of your recipe?");
        switch (toEdit)
        {
            case "Title":
                data.EditTitle(title, newText);
                break;
            case "Ingredients":
                data.EditIngredients(title, newText);
                break;
            case "Instructions":
                data.EditInstructions(title, newText);
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
            data.AddCategory(title, AnsiConsole.Ask<string>("What is the name of your new [dodgerblue2]category[/]?"));
            break;

        case "Edit Category":
            category = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
                   .Title("Which Category would you like to edit?")
                   .PageSize(10)
                   .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                   .AddChoices(data.Recipes.Where(r => r.Title == title).ToList()[0].Categories.ToArray()));
            data.EditCategory(title, category, AnsiConsole.Ask<string>("What is the new name of the [dodgerblue2]category[/]?"));
            break;

        case "Delete Category":
            category = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
                   .Title("Which Category would you like to remove?")
                   .PageSize(10)
                   .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                   .AddChoices(data.Recipes.Where(r => r.Title == title).ToList()[0].Categories.ToArray()));
            data.RemoveCategory(title, category);
            break;
        default: break;
    }

}

static void DeleteRecipe(ref Data data)
{
    var title = RecipeSelection(ref data, "Choose which recipe you would like to delete?");
    data.RemoveRecipe(title);
}

static string RecipeSelection(ref Data data, string text)
{
    return AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title(text)
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
        .AddChoices(data.Recipes.Select(r => r.Title)
        ));
}

static string MultiLineInput(ref Data data, string text)
{
    AnsiConsole.Markup($"Please insert the [dodgerblue2]{text}[/] of your recipe \n");
    AnsiConsole.Markup("Press Enter after writing to add another\n If you are done write [red]Done[/] then press Enter \n");
    string input;
    string inputString = "";
    for (int i = 0; i < 30; i++)
    {
        input = AnsiConsole.Ask<string>("- ");
        if (input == "Done")
            break;
        inputString+=$"{input}, ";
    }
    return inputString.Substring(0,inputString.Length-2);
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

static string stringLimiter(string input)
{
    Console.WriteLine(input);
    if (input.Length > 30)
        return input.Substring(0, 30) + "...";
    
    return input;
}

static string ListLimitedView(List<String> list)
{
    string result = string.Join(", ", list.ToArray());
    if (result.Length > 30)
        return result.Substring(0, 30) + "...";
    return result;
}

static void RecipeTableView(ref Data data)
{
    AnsiConsole.Write(
    new FigletText("Recipes")
        .LeftAligned()
        .Color(Color.Red));
    // View recipes
    // Create a table
    var table = new Table().Border(TableBorder.Ascii2);
    table.Expand();
    // Create Table Columns
    table.AddColumn("[dodgerblue2]Title[/]");
    table.AddColumn(new TableColumn("[dodgerblue2]Ingredients[/]").LeftAligned());
    table.AddColumn(new TableColumn("[dodgerblue2]Instructions[/]").LeftAligned());
    table.AddColumn(new TableColumn("[dodgerblue2]Categories[/]").LeftAligned());
    // Add the Recipes to the table
    data.Recipes.ForEach(r => table.AddRow("[bold][red]" + r.Title + "[/][/]",
                                            stringLimiter(r.Ingredients),
                                            stringLimiter(r.Instructions),
                                            ListLimitedView(r.Categories)));
    AnsiConsole.Write(table);
}