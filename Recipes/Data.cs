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

    public void AddRecipe(Recipe r)
    {
        Recipes.Add(r);
    }
    public void RemoveRecipe(string title)
    {
        Recipes.RemoveAll(x => x.Title == title);
    }

    public void EditTitle(string title, string newTitle)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => r.Title = newTitle).ToList();
    }
    public void EditIngredients(string title, string newIngredients)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Ingredients.Remove(newIngredients); r.Categories.Add(newIngredients); return r; }).ToList();
    }
    public void EditInstructions(string title, string newInstructions)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Ingredients.Remove(newInstructions); r.Categories.Add(newInstructions); return r; }).ToList();
    }
    
    public void AddCategory(string title, string newCategory)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Categories.Add(newCategory); return r; }).ToList();

    }
    public void EditCategory(string title, string category, string newCategory)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Categories.Remove(category);r.Categories.Add(newCategory); return r; }).ToList();
    }

    public void RemoveCategory(string title,string category)
    {
        Recipes.Where(r => r.Title == title).ToList()[0].Categories.RemoveAll(c=>c == category);
    }
    public void SaveRecipes()
    {
        File.WriteAllText(curFile, JsonSerializer.Serialize(Recipes));
    }


}