// See https://aka.ms/new-console-template for more information
using Spectre.Console;
using System.Text.Json;
using System.IO;
string curFile = @"C:\Users\RS3\source\repos\Recipes\Recipes\recipes.json";

AnsiConsole.Markup("[underline red]Hello[/] World!");

Categories[] c = { Categories.Breakfast };

Recipe recipe1 = new Recipe("Scrambeled Eggs 1",
    "salt, pepper, eggs, olive oil",
    "Add Olive oil to the pan on low heat then put the crach the egg and put it",
     c);
Recipe recipe2 = new Recipe("Scrambeled Eggs 2",
    "salt, pepper, eggs, olive oil",
    "Add Olive oil to the pan on low heat then put the crach the egg and put it",
     c);
Recipe recipe3 = new Recipe("Scrambeled Eggs 3",
    "salt, pepper, eggs, olive oil",
    "Add Olive oil to the pan on low heat then put the crach the egg and put it",
     c);
Recipe recipe4 = new Recipe("Scrambeled Eggs 4",
    "salt, pepper, eggs, olive oil",
    "Add Olive oil to the pan on low heat then put the crach the egg and put it",
     c);
Recipe[] recipes = { recipe1, recipe2, recipe3, recipe4 };
var json = JsonSerializer.Serialize(recipes);


Console.WriteLine(File.Exists(curFile) ? "File exists." : "File does not exist.");
if (!File.Exists(curFile))
{
    File.WriteAllText(curFile, json);
}
