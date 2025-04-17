using System;
using UnityEngine;

namespace Automation.BlockEntities
{
    public class Market : BEWithItems
    {
        public static Market selected;
        static Vector2[] iconPoses = new Vector2[]
        {
            new Vector2(0.16f, 0.09f),
            new Vector2(0.09f, 0.16f),
            new Vector2(0.16f, 0.23f),
            new Vector2(0.23f, 0.16f)
        };
        string itemSaveKey;
        Item buyItem;
        SpriteRenderer itemIcon;
        int timer;

        public override void OnLoaded()
        {
            base.OnLoaded();
            itemSaveKey = $"_{grid.name}_{pos.x}_{pos.y}_item";
            int itemid = PlayerPrefs.GetInt(itemSaveKey, -1);
            if (itemid != -1) buyItem = Items.items[itemid];
            Bootstrap.OnLogicUpdate += OnLogicUpdate;
        }

        public override void DrawBlockEntity()
        {
            base.DrawBlockEntity();
            if (buyItem != null)
            {
                DrawSelectedItem();
            }
        }

        protected void DrawSelectedItem()
        {
            var locpos = iconPoses[side.Index];
            itemIcon = Items.CreateItemSprite(grid, new Vector3(locpos.x + pos.x * Grid.cellSize, locpos.y + pos.y * Grid.cellSize, -6), new Vector3(0.3125f, 0.3125f, 1)).GetComponent<SpriteRenderer>();
            itemIcon.sprite = buyItem.icon;
        }

        public override void SetRotation(Side2D rot)
        {
            base.SetRotation(rot);
            var locpos = iconPoses[side.Index];
            itemIcon.gameObject.transform.localPosition = new Vector3(locpos.x + pos.x * Grid.cellSize, locpos.y + pos.y * Grid.cellSize, -6);
        }

        private void OnLogicUpdate()
        {
            if (buyItem != null)
            {
                timer++;
                if (timer >= 10)
                {
                    ItemEntity itemEntity = new ItemEntity();
                    itemEntity.itemStack = new ItemStack(buyItem);
                    itemEntity.pos = 3;
                    itemEntity.from = side.Opposite;
                    itemEntity.to = side;
                    itemEntity.lastLogicTime = Bootstrap.time;
                    if (grid.visual)
                    {
                        itemEntity.sprite = Items.CreateItemSprite(grid, new Vector3(0, 0, -3), new Vector3(0.5f, 0.5f, 1)).transform;
                        itemEntity.sprite.GetComponent<SpriteRenderer>().sprite = buyItem.icon;
                    }
                    moveItems.Add(itemEntity);
                    timer -= 10;
                }
            }
            else
            {
                timer = 0;
            }
        }

        public override void OnUse()
        {
            base.OnUse();
            selected = this;
            MainGUI.OpenMidPanel("selectItem");
        }

        public void SetItem(Item item)
        {
            buyItem = item;
            if (buyItem == null) PlayerPrefs.DeleteKey(itemSaveKey);
            else PlayerPrefs.SetInt(itemSaveKey, buyItem.id);
            if (grid.visual)
            {
                if (buyItem != null)
                {
                    if (itemIcon != null)
                    {
                        itemIcon.sprite = buyItem.icon;
                    }
                    else
                    {
                        DrawSelectedItem();
                    }
                }
                else
                {
                    if (itemIcon != null)
                    {
                        Items.PoolSprite(itemIcon.gameObject);
                    }
                }
            }
        }

        public override void OnItemHalfPath(ItemEntity item)
        {

        }

        public override bool CanInputItem(Side2D from)
        {
            return false;
        }

        public override void OnRemoved()
        {
            PlayerPrefs.DeleteKey(itemSaveKey);
            if (itemIcon != null) Items.PoolSprite(itemIcon.gameObject);
            base.OnRemoved();
        }
    }
}
