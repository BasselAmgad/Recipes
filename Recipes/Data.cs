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


        if (!File.Exists(curFile))
        {
            Recipes = new List<Recipe>();
            File.WriteAllText(curFile, JsonSerializer.Serialize(Recipes));
        }
        else
        {
            using (StreamReader r = new StreamReader(curFile))
            {
                string json = r.ReadToEnd();
                Recipes = JsonSerializer.Deserialize<List<Recipe>>(json);
            }
        }
        
        
    }

    public void addRecipe(Recipe r)
    {
        Recipes.Add(r);
    }
    public void removeRecipe(string title)
    {
        Recipes.RemoveAll(x => x.Title == title);
    }

    public void editTitle(string title, string newTitle)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => r.Title = newTitle).ToList();
    }
    public void editIngredients(string title, string newIngredients)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Ingredients.Remove(newIngredients); r.Categories.Add(newIngredients); return r; }).ToList();
    }
    public void editInstructions(string title, string newInstructions)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Ingredients.Remove(newInstructions); r.Categories.Add(newInstructions); return r; }).ToList();
    }
    
    public void addCategory(string title, string newCategory)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Categories.Add(newCategory); return r; }).ToList();

    }
    public void editCategory(string title, string category, string newCategory)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Categories.Remove(category);r.Categories.Add(newCategory); return r; }).ToList();
    }

    public void removeCategory(string title,string category)
    {
        Recipes.Where(r => r.Title == title).ToList()[0].Categories.RemoveAll(c=>c == category);
    }
    public void saveRecipes()
    {
        File.WriteAllText(curFile, JsonSerializer.Serialize(Recipes));
    }


}