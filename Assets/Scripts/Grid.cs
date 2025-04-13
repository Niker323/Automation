using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

namespace Automation
{
    public class Grid
    {
        const int size = 20;
        BlockEntity[,] blocks = new BlockEntity[size, size];
        const float cellSize = 0.32f;
        static VertexAttributeDescriptor[] layout = new[]
            {
            new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 4)//,
            //new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.SNorm8, 4),
            //new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 2),
            //new VertexAttributeDescriptor(VertexAttribute.TexCoord3, VertexAttributeFormat.UInt8, 4)
        };

        public void Init()
        {
            Mesh grid = new Mesh();
            grid.MarkDynamic();
            int verts = size * size * 4;
            BlockVertex[] vertices = new BlockVertex[verts];
            int[] indices = new int[verts];
            for (int i = 0; i < indices.Length; i++)
                indices[i] = i;
            Vector2[] uvs = new Vector2[verts];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Block block = Blocks.blocks[0];

                    float xPos = x * cellSize;
                    float yPos = y * cellSize;
                    vertices[(x + y * size) * 4] = new BlockVertex(xPos, yPos, 0, block.downAnimData);
                    vertices[(x + y * size) * 4 + 1] = new BlockVertex(xPos + cellSize, yPos, 0, block.downAnimData);
                    vertices[(x + y * size) * 4 + 2] = new BlockVertex(xPos + cellSize, yPos + cellSize, 0, block.downAnimData);
                    vertices[(x + y * size) * 4 + 3] = new BlockVertex(xPos, yPos + cellSize, 0, block.downAnimData);

                    uvs[(x + y * size) * 4] = new Vector2(block.downRect.x, block.downRect.y);
                    uvs[(x + y * size) * 4 + 1] = new Vector2(block.downRect.x + block.downRect.width / block.downFrames, block.downRect.y);
                    uvs[(x + y * size) * 4 + 2] = new Vector2(block.downRect.x + block.downRect.width / block.downFrames, block.downRect.y + block.downRect.height);
                    uvs[(x + y * size) * 4 + 3] = new Vector2(block.downRect.x, block.downRect.y + block.downRect.height);

                    blocks[x, y] = new BlockEntity(block, new Vector2Int(x, y));
                }
            }
            grid.SetVertexBufferParams(verts, layout);
            grid.SetVertexBufferData(vertices, 0, 0, verts, 0, MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontValidateIndices);
            grid.SetIndices(indices, MeshTopology.Quads, 0);
            grid.uv = uvs;
            grid.RecalculateBounds();
            GameObject gridgo = new GameObject("Grid");
            gridgo.transform.parent = Bootstrap.instance.field.transform;
            gridgo.transform.position = new Vector3(0, 0, -1);
            gridgo.AddComponent<MeshFilter>().mesh = grid;
            gridgo.AddComponent<MeshRenderer>().material = Bootstrap.instance.gridMaterial;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct BlockVertex
        {
            public float posX;
            public float posY;
            public float posZ;
            public float posW;

            public BlockVertex(float posX, float posY, float posZ, float posW)
            {
                this.posX = posX;
                this.posY = posY;
                this.posZ = posZ;
                this.posW = posW;
            }
        }
    }
}
