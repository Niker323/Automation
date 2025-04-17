using UnityEngine;

namespace Automation
{
    public class ItemEntity
    {
        public ItemStack itemStack;
        public Side2D from;
        public Side2D to;
        public int pos;
        public Transform sprite;
        public float lastLogicTime;
        public Vector2 randomOffset;
    }
}