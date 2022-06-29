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
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Categories.Add(newCategory); return r; }).ToList();

    }
    public void EditCategory(string title, string category, string newCategory)
    {
        Recipes.Where(r => r.Title.Equals(title)).Select(r => { r.Categories.Remove(category); r.Categories.Add(newCategory); return r; }).ToList();
    }

    public void RemoveCategory(string title, string category)
    {
        Recipes.Where(r => r.Title == title).ToList()[0].Categories.RemoveAll(c => c == category);
    }
    public void SaveRecipes()
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(Recipes));
    }


}