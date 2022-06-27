using System;

class Recipe
{
    public string Title { get; set; }
    public string Ingredients { get; set; }
    public string Instructions { get; set; }
    public Categories[] Categories { get; set; }

    public Recipe(string title, string ingredients, string instructions, Categories[] categories)
    {
        Title = title;
        Ingredients = ingredients;
        Instructions = instructions;
        Categories = categories;
    }

}

enum Categories
{
    Breakfast,
    Lunch,
    Dinner,
    Snack
}
