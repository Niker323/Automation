using System;
using UnityEngine;

namespace Automation.BlockEntities
{
    public class Seller : BEWithItems
    {
        public override void OnItemHalfPath(ItemEntity item)
        {
            moveItems.Remove(item);
            Bootstrap.ChangeMoney(item.itemStack.item.price * item.itemStack.count);
            if (item.sprite != null) Items.PoolSprite(item.sprite.gameObject);
        }

        public override bool CanInputItem(Side2D from)
        {
            return from == side.Opposite;
        }
    }
}
