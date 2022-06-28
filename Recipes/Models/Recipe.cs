using System;

class Recipe
{
    public string Title { get; set; }
    public string Ingredients { get; set; }
    public string Instructions { get; set; }
    public List<String> Categories { get; set; }

    public Recipe(string title, string ingredients, string instructions, List<String> categories)
    {
        Title = title;
        Ingredients = ingredients;
        Instructions = instructions;
        Categories = categories;
    }

}


