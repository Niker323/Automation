using System;
using System.Drawing;
using UnityEngine;

namespace Automation
{
    public class Grid : MonoBehaviour
    {
        const int size = 20;
        BlockEntity[,] blocks = new BlockEntity[size, size];

        public void Init()
        {
            Mesh grid = new Mesh();
            grid.MarkDynamic();
            Vector3[] vertices = new Vector3[size * size * 4];
            int[] indices = new int[size * size * 4];
            for (int i = 0; i < indices.Length; i++)
                indices[i] = i;
            Vector2[] uvs = new Vector2[size * size * 4];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    //GameObject locobj = Instantiate(block, locpos, Quaternion.identity);
                    //locobj.transform.SetParent(transform);
                    //BlockInfo locblock = locobj.GetComponent<BlockInfo>();
                    //locblock.x = arr_x;
                    //locblock.y = arr_y;

                    vertices[x * y * 4] = new Vector3(x, y, 0);
                    vertices[x * y * 4 + 1] = new Vector3(x + 1, y, 0);
                    vertices[x * y * 4 + 2] = new Vector3(x + 1, y + 1, 0);
                    vertices[x * y * 4 + 3] = new Vector3(x, y + 1, 0);

                    //uvs[]

                    blocks[x, y] = new BlockEntity(Blocks.blocks[0], new Vector2Int(x, y));
                }
            }
            grid.vertices = vertices;
            grid.SetIndices(indices, MeshTopology.Quads, 0);
            grid.uv = uvs;
            GetComponent<MeshFilter>().mesh = grid;
        }
    }
}
