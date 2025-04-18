using System;
using UnityEngine;

namespace Automation.BlockEntities
{
    public class Furnace : BEWithItems
    {
        ItemStack input;
        int timer;

        public override void OnLoaded()
        {
            base.OnLoaded();
            Bootstrap.OnLogicUpdate += OnLogicUpdate;
        }

        private void OnLogicUpdate()
        {

        }

        public override void OnItemHalfPath(ItemEntity item)
        {

        }

        public override bool CanInputItem(Side2D from)
        {
            return from == side.Opposite;
        }
    }
}
