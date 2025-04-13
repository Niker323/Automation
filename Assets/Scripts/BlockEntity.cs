using System;
using UnityEngine;

namespace Automation
{
    public class BlockEntity
    {
        public Vector2Int pos;
        public Block block;

        public BlockEntity(Block block, Vector2Int pos)
        {
            this.block = block;
            this.pos = pos;
        }
    }
}
