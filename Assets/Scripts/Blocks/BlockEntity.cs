using System;
using UnityEngine;

namespace Automation.BlockEntities
{
    public class BlockEntity
    {
        public Vector2Int pos;
        public Block block;

        public virtual void OnLoaded()
        {

        }

        public virtual void OnUnloaded()
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
        }
    }
}
