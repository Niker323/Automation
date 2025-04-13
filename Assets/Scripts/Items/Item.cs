
using UnityEngine;

namespace Automation
{
    [CreateAssetMenu(fileName = "Item", menuName = "Automation/Item")]
    public class Item : ScriptableObject
    {
        public int id;
        public Texture icon;
    }
}