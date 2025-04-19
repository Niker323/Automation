using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Automation.BlockEntities
{
    public class Furnace : BEWithItems
    {
        ItemEntity input;
        int timer;

        public override void OnLoaded()
        {
            base.OnLoaded();
            Bootstrap.OnLogicUpdate += OnLogicUpdate;
        }

        protected override void OnLogicUpdate()
        {
            if (input != null)
            {
                timer++;
                if (timer >= 10)
                {
                    input.itemStack = new ItemStack(Recipes.furnaceRecipes[input.itemStack.item.id]);
                    input.SetIcon(input.itemStack.item.icon);
                    input.to = side;
                    moveItems.Add(input);
                    input = null;
                    timer = 0;
                }
            }
            else
            {
                timer = 0;
            }
            base.OnLogicUpdate();
        }

        public override bool TryInsertItem(ItemEntity item, Side2D from)
        {
            item.randomOffset = Vector2.zero;
            return base.TryInsertItem(item, from);
        }

        public override void OnItemHalfPath(ItemEntity item)
        {
            moveItems.Remove(item);
            if (input == null && Recipes.furnaceRecipes.ContainsKey(item.itemStack.item.id))
            {
                input = item;
            }
            else
            {
                item.Drop();
            }
        }

        public override bool CanInputItem(Side2D from)
        {
            return from == side.Opposite;
        }

        public override void OnRemoved()
        {
            Bootstrap.OnLogicUpdate -= OnLogicUpdate;
            base.OnRemoved();
        }
    }
}
