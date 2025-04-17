using System;
using UnityEngine;

namespace Automation.BlockEntities
{
    public class BlockEntity
    {
        public Grid grid;
        public Vector2Int pos;
        public Block block;

        public virtual void OnLoaded()
        {

        }

        public virtual void DrawBlockEntity()
        {

        }

        public virtual void OnRemoved()
        {

        }

        public virtual void OnUse()
        {

        }

        public virtual void GetBlockUVs(Vector3[] outUVs)
        {
            outUVs[0] = new Vector3(block.downRect.x, block.downRect.y, block.downAnimData);
            outUVs[1] = new Vector3(block.downRect.x + block.downRect.width / block.downFrames, block.downRect.y, block.downAnimData);
            outUVs[2] = new Vector3(block.downRect.x + block.downRect.width / block.downFrames, block.downRect.y + block.downRect.height, block.downAnimData);
            outUVs[3] = new Vector3(block.downRect.x, block.downRect.y + block.downRect.height, block.downAnimData);

            outUVs[4] = new Vector3(block.upRect.x, block.upRect.y, block.upAnimData);
            outUVs[5] = new Vector3(block.upRect.x + block.upRect.width / block.upFrames, block.upRect.y, block.upAnimData);
            outUVs[6] = new Vector3(block.upRect.x + block.upRect.width / block.upFrames, block.upRect.y + block.upRect.height, block.upAnimData);
            outUVs[7] = new Vector3(block.upRect.x, block.upRect.y + block.upRect.height, block.upAnimData);
        }
    }
}
