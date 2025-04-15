using System;
using UnityEngine;

namespace Automation.BlockEntities
{
    public class SidedBlockEntity : BlockEntity
    {
        public Side2D side = Side2D.UP;

        public override void GetBlockUVs(Vector3[] outUVs)
        {
            outUVs[(4 - side.Index) % 4] = new Vector3(block.downRect.x, block.downRect.y, block.downAnimData);
            outUVs[(5 - side.Index) % 4] = new Vector3(block.downRect.x + block.downRect.width / block.downFrames, block.downRect.y, block.downAnimData);
            outUVs[(6 - side.Index) % 4] = new Vector3(block.downRect.x + block.downRect.width / block.downFrames, block.downRect.y + block.downRect.height, block.downAnimData);
            outUVs[(7 - side.Index) % 4] = new Vector3(block.downRect.x, block.downRect.y + block.downRect.height, block.downAnimData);

            outUVs[4 + ((4 - side.Index) % 4)] = new Vector3(block.upRect.x, block.upRect.y, block.upAnimData);
            outUVs[4 + ((5 - side.Index) % 4)] = new Vector3(block.upRect.x + block.upRect.width / block.upFrames, block.upRect.y, block.upAnimData);
            outUVs[4 + ((6 - side.Index) % 4)] = new Vector3(block.upRect.x + block.upRect.width / block.upFrames, block.upRect.y + block.upRect.height, block.upAnimData);
            outUVs[4 + ((7 - side.Index) % 4)] = new Vector3(block.upRect.x, block.upRect.y + block.upRect.height, block.upAnimData);
        }
    }
}
