using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    public class Recipes : MonoBehaviour
    {
        public static Dictionary<int, Item> furnaceRecipes = new Dictionary<int, Item>();
        private Dictionary<string, FurnaceRecipe> techFurnaceRecipes = new Dictionary<string, FurnaceRecipe>();

        public void Init()
        {
            FurnaceRecipe[] _furnaceRecipes = Resources.LoadAll<FurnaceRecipe>("Recipes/Furnace");
            foreach (FurnaceRecipe recipe in _furnaceRecipes)
            {
                if (TechTree.nodes[recipe.tech].state == TechTree.NodeState.Researched)
                {
                    furnaceRecipes.Add(recipe.input.id, recipe.output);
                }
                else
                {
                    techFurnaceRecipes.Add(recipe.tech, recipe);
                }
            }
            TechTree.OnResearchTech += OnResearchTech;
        }

        private void OnResearchTech(string tech)
        {
            if (techFurnaceRecipes.TryGetValue(tech, out FurnaceRecipe recipe))
                furnaceRecipes.Add(recipe.input.id, recipe.output);
        }
    }
}
