using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Automation
{
    public class Blocks : MonoBehaviour
    {
        public static Block[] blocks;
        public static Texture2D blockAtlas;
        public Block[] _blocks;

        public void Init()
        {
            blocks = _blocks;

            HashSet<Texture2D> arrayOfTextures = new HashSet<Texture2D>();
            for (int i = 0; i < blocks.Length; i++)
            {
                Block block = blocks[i];
                block.id = i;
                block.downAnimData = BitConverter.Int32BitsToSingle(block.downFrames | (block.downFPS << 8));
                block.upAnimData = BitConverter.Int32BitsToSingle(block.upFrames | (block.upFPS << 8));
                arrayOfTextures.Add(block.downTexture);
                arrayOfTextures.Add(block.upTexture);
                block.blockEntityType = Type.GetType("Automation.BlockEntities." + block.blockEntity);
            }

            blockAtlas = new Texture2D(1, 1);
            blockAtlas.filterMode = FilterMode.Point;
            blockAtlas.wrapMode = TextureWrapMode.Clamp;
            Texture2D[] textures = arrayOfTextures.ToArray();
            Rect[] uvRectChangesFromTextureAtlas = blockAtlas.PackTextures(textures, 0, 2048);

            foreach (var block in blocks)
            {
                block.downRect = uvRectChangesFromTextureAtlas[Array.IndexOf(textures, block.downTexture)];
                block.upRect = uvRectChangesFromTextureAtlas[Array.IndexOf(textures, block.upTexture)];
            }

            Shader.SetGlobalVector("_AtlasSize", new Vector2(blockAtlas.width / 32, blockAtlas.height / 32));
            Bootstrap.instance.gridMaterial.mainTexture = blockAtlas;
        }
    }
}
