
using UnityEngine;

namespace Automation
{
    [CreateAssetMenu(fileName = "Item", menuName = "Automation/Item")]
    public class Item : ScriptableObject
    {
        public int id;
        public string code;
        public Sprite icon;
        public bool inMarket = false;
        public long price;
    }
}