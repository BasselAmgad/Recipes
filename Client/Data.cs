using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

class Data
{
    public List<Recipe> Recipes { get; set; }
    private string _filePath;

    public Data()
    {

        Recipes = new();
        // This method creates a path where we have access to read and write data inside the ProgramData folder
        var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        _filePath = Path.Combine(systemPath, "Recipes.json");
        if (!File.Exists(this._filePath))
        {
            Recipes = new List<Recipe>();
            File.WriteAllText(this._filePath, JsonSerializer.Serialize(Recipes));
        }
        else
        {
            using (StreamReader r = new StreamReader(this._filePath))
            {
                var data = r.ReadToEnd();
                var json = JsonSerializer.Deserialize<List<Recipe>>(data);
                if (json != null)
                    Recipes = json;
            }
        }
    }

    public void AddRecipe(Recipe r)
    {
        Recipes.Add(r);
    }

    public void RemoveRecipe(string title)
    {
        Recipes.Remove(Recipes.Find(r => r.Title == title));
    }

    public void EditTitle(string title, string newTitle)
    {
        Recipes.Find(r => r.Title == title).Title = newTitle;
    }

    public void EditIngredients(string title, string newIngredients)
    {
        Recipes.Find(r => r.Title == title).Ingredients = newIngredients;
    }

    public void EditInstructions(string title, string newInstructions)
    {
        Recipes.Find(r => r.Title == title).Instructions = newInstructions;
    }

    public void AddCategory(string title, string newCategory)
    {
        Recipes.Find(r => r.Title == title).Categories.Add(newCategory);

    }

    public void RemoveCategory(string title, string category)
    {
        Recipes.Where(r => r.Title == title).ToList()[0].Categories.RemoveAll(c => c == category);
    }

    public void EditCategory(string title, string category, string newCategory)
    {
        RemoveCategory(title, newCategory);
        AddCategory(title, newCategory);
    }

    public void SaveRecipes()
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(Recipes));
    }

}