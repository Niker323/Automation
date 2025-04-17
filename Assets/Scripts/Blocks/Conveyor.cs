using System;
using UnityEngine;

namespace Automation.BlockEntities
{
    public class Conveyor : BEWithItems
    {
        public override void OnItemHalfPath(ItemEntity item)
        {
            item.to = side;
        }

        public override bool CanInputItem(Side2D from)
        {
            return true;
        }
    }
}
