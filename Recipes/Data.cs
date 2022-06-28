using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

class Data
{
    string curFile = @"C:\Users\RS3\source\repos\Recipes\Recipes\recipes.json";
    public List<Recipe> Recipes { get; set; }

    public Data()
    {   // Need to handle if the file doenst exist
        
        

        using (StreamReader r = new StreamReader(curFile))
        {
            string json = r.ReadToEnd();
            Recipes = JsonSerializer.Deserialize<List<Recipe>>(json);
        }
        
    }

    public void addRecipe(Recipe r)
    {
        Recipes.Add(r);
    }

    public void editTitle(string title, string newTitle)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => r.Title = newTitle);
    }
    public void editIngredients(string title, string newIngredients)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => r.Ingredients = newIngredients);
    }
    public void editInstructions(string title, string newInstructions)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => r.Instructions = newInstructions);   
    }
    
    public void addCategory(string title, string newCategory)
    {
        //customers.Where(c => c.IsValid).Select(c => { c.CreditLimit = 1000; return c; }).ToList();
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Categories.Add(newCategory); return r; }).ToList();

    }
    public void editCategory(string title, string category, string newCategory)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Categories.Remove(category);r.Categories.Add(newCategory); return r; }).ToList();
    }
    public void saveRecipes()
    {
        File.WriteAllText(curFile, JsonSerializer.Serialize(Recipes));
    }


}