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
        public Vector2 randomOffsetInterp;

        public void SetIcon(Sprite icon)
        {
            if (sprite != null) sprite.GetComponent<SpriteRenderer>().sprite = icon;
        }

        public void Drop()
        {
            if (sprite != null)
            {
                Items.PoolSprite(sprite.gameObject);
            }
        }
    }
}