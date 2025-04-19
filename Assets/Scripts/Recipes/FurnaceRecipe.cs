using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    [CreateAssetMenu(fileName = "FurnaceRecipe", menuName = "Automation/Recipe/Furnace")]
    public class FurnaceRecipe : ScriptableObject
    {
        public Item input;
        public Item output;
        public string tech;
    }
}