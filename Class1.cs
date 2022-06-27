using System;

class Recipe
{
    public string Title { get; set; }
    public string Ingredients { get; set; }
    public string Instructions { get; set; }
    public List<Categories> Categories { get; set; }

    public Recipe(string title, string ingredients, string instructions, List<Categories> categories)
    {
        Title = title;
        Ingredients = ingredients;
        Instructions = instructions;
        Categories = categories;
    }

}

enum Categories
{

}
