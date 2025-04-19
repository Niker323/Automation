using UnityEngine;

namespace Automation.BlockEntities
{
    public class Conveyor : BEWithItems
    {
        public override bool TryInsertItem(ItemEntity item, Side2D from)
        {
            if (from != side.Opposite && item.randomOffset == Vector2.zero) item.randomOffset = new Vector2(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
            return base.TryInsertItem(item, from);
        }

        public override void OnItemHalfPath(ItemEntity item)
        {
            item.to = side;
        }

        public override bool CanInputItem(Side2D from)
        {
            return from != side;
        }
    }
}
