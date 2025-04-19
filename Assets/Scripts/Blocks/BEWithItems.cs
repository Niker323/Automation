
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Automation.BlockEntities
{
    public abstract class BEWithItems : SidedBlockEntity
    {
        public List<ItemEntity> moveItems = new List<ItemEntity>();

        public override void OnLoaded()
        {
            base.OnLoaded();
            Bootstrap.OnLogicUpdate += OnLogicUpdate;
            if (grid.visual) Bootstrap.OnUpdate += OnUpdate;
        }

        protected virtual void OnLogicUpdate()
        {
            for (int i = moveItems.Count - 1; i >= 0; i--)
            {
                ItemEntity ie = moveItems[i];
                if (ie.lastLogicTime == Bootstrap.time) continue;
                ie.lastLogicTime = Bootstrap.time;
                ie.pos++;
                if (ie.pos == 5)
                {
                    OnItemHalfPath(ie);
                }
                else if (ie.pos >= 10)
                {
                    TryMoveItemToNextPos(ie);
                }
            }
        }

        protected void TryMoveItemToNextPos(ItemEntity item)
        {
            Vector2Int nextPos = pos + item.to.Normali;
            BlockEntity nextBE = grid.GetBlockEntity(nextPos);
            moveItems.Remove(item);
            if (nextBE is BEWithItems bewi && bewi.CanInputItem(item.to.Opposite) && bewi.TryInsertItem(item, item.to.Opposite))
            {

            }
            else
            {
                item.Drop();
            }
        }

        public virtual bool TryInsertItem(ItemEntity item, Side2D from)
        {
            item.pos = 0;
            item.from = from;
            item.to = null;
            moveItems.Add(item);
            return true;
        }

        public abstract void OnItemHalfPath(ItemEntity item);

        public override void DrawBlockEntity()
        {
            base.DrawBlockEntity();
            Bootstrap.OnUpdate += OnUpdate;
        }

        private void OnUpdate()
        {
            if (grid.visual)
            {
                for (int i = 0; i < moveItems.Count; i++)
                {
                    ItemEntity ie = moveItems[i];
                    float dt = Bootstrap.time - ie.lastLogicTime;
                    Vector2 locpos;
                    float lpos = ie.pos / 10f + dt;
                    if (ie.pos > 4)
                    {
                        locpos = (lpos - 0.5f) * Grid.cellSize * ie.to.Normal;
                    }
                    else
                    {
                        locpos = (0.5f - lpos) * Grid.cellSize * ie.from.Normal;
                    }
                    if (ie.randomOffsetInterp != ie.randomOffset)
                    {
                        ie.randomOffsetInterp = Vector2.Lerp(ie.randomOffsetInterp, ie.randomOffset, lpos * 2);
                    }
                    locpos += ie.randomOffsetInterp;
                    if (ie.sprite == null)
                    {
                        ie.sprite = Items.CreateItemSprite(grid, new Vector3(0, 0, -3), new Vector3(0.5f, 0.5f, 1)).transform;
                        ie.sprite.GetComponent<SpriteRenderer>().sprite = ie.itemStack.item.icon;
                    }
                    ie.sprite.transform.localPosition = new Vector3(pos.x * Grid.cellSize + locpos.x + Grid.cellSize / 2, pos.y * Grid.cellSize + locpos.y + Grid.cellSize / 2, -3);
                }
            }
            else
            {
                Bootstrap.OnUpdate -= OnUpdate;
            }
        }

        public abstract bool CanInputItem(Side2D from);

        public override void OnRemoved()
        {
            foreach (ItemEntity itemEntity in moveItems)
            {
                if (itemEntity.sprite != null)
                {
                    Items.PoolSprite(itemEntity.sprite.gameObject);
                }
            }
            Bootstrap.OnUpdate -= OnUpdate;
            Bootstrap.OnLogicUpdate -= OnLogicUpdate;
            base.OnRemoved();
        }
    }
}