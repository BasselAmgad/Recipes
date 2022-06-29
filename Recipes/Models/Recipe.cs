﻿using System;

class Recipe
{
    public string Title { get; set; }
    public List<String> Ingredients { get; set; }
    public List<String> Instructions { get; set; }
    public List<String> Categories { get; set; }

    public Recipe(string title, List<String> ingredients, List<String> instructions, List<String> categories)
    {
        Title = title;
        Ingredients = ingredients;
        Instructions = instructions;
        Categories = categories;
    }

}


