using Automation.BlockEntities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

namespace Automation
{
    public class Grid
    {
        public const int size = 20;
        public const float cellSize = 0.32f;
        const int verts = size * size * 4;
        public string name;
        BlockEntity[,] blocks = new BlockEntity[size, size];
        GameObject gridGO;
        Mesh gridMesh;
        Vector3[] uvs;
        HashSet<Vector2Int> toredraw = new HashSet<Vector2Int>();
        static Vector3[] forblockuvs = new Vector3[4];
        bool visual = false;

        public void Init(string gridName)
        {
            LoadGrid(gridName);
        }

        void LoadGrid(string gridName)
        {
            name = gridName;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    SetBlock(x, y, PlayerPrefs.GetInt($"{gridName}_{x}_{y}"));
                }
            }
        }

        public BlockEntity GetBlockEntity(Vector3 pos)
        {
            const float halfField = cellSize * (size / 2);
            if (pos.x > -halfField && pos.x < halfField)
            {
                if (pos.y > -halfField && pos.y < halfField)
                {
                    int x = Mathf.FloorToInt((pos.x + halfField) / cellSize);
                    int y = Mathf.FloorToInt((pos.y + halfField) / cellSize);
                    return blocks[x, y];
                }
            }
            return null;
        }

        public void SetBlock(int x, int y, int id)
        {
            Block block = Blocks.blocks[id];
            BlockEntity blockEntity = Activator.CreateInstance(block.blockEntityType) as BlockEntity;
            blockEntity.block = block;
            blockEntity.pos = new Vector2Int(x, y);
            blocks[x, y]?.OnUnloaded();
            blocks[x, y] = blockEntity;
            blockEntity.OnLoaded();
            RedrawBlock(x, y);
        }

        public void SetBlock(Vector2Int pos, int id) => SetBlock(pos.x, pos.y, id);

        public void RedrawBlock(int x, int y)
        {
            if (visual) toredraw.Add(new Vector2Int(x, y));
        }

        public void RedrawBlock(Vector2Int pos) => RedrawBlock(pos.x, pos.y);

        public void DrawGrid()
        {
            gridMesh = new Mesh();
            gridMesh.MarkDynamic();
            Vector3[] vertices = new Vector3[verts];
            int[] indices = new int[verts];
            for (int i = 0; i < indices.Length; i++)
                indices[i] = i;
            uvs = new Vector3[verts];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    BlockEntity be = blocks[x, y];

                    float xPos = x * cellSize;
                    float yPos = y * cellSize;
                    int ind = (x + y * size) * 4;
                    vertices[ind] = new Vector3(xPos, yPos, 0);
                    vertices[ind + 1] = new Vector3(xPos + cellSize, yPos, 0);
                    vertices[ind + 2] = new Vector3(xPos + cellSize, yPos + cellSize, 0);
                    vertices[ind + 3] = new Vector3(xPos, yPos + cellSize, 0);

                    be.GetBlockUVs(forblockuvs);
                    uvs[ind] = forblockuvs[0];
                    uvs[ind + 1] = forblockuvs[1];
                    uvs[ind + 2] = forblockuvs[2];
                    uvs[ind + 3] = forblockuvs[3];
                }
            }
            gridMesh.vertices = vertices;
            gridMesh.SetIndices(indices, MeshTopology.Quads, 0);
            gridMesh.SetUVs(0, uvs);
            gridMesh.RecalculateBounds();
            gridGO = new GameObject("Grid");
            gridGO.transform.parent = Bootstrap.instance.field.transform;
            gridGO.transform.position = new Vector3(0, 0, -1) - gridMesh.bounds.center;
            gridGO.AddComponent<MeshFilter>().mesh = gridMesh;
            gridGO.AddComponent<MeshRenderer>().material = Bootstrap.instance.gridMaterial;
            Bootstrap.OnLateUpdate += OnLateUpdate;
            visual = true;
        }

        public void OnLateUpdate()
        {
            if (toredraw.Count > 0)
            {
                foreach (Vector2Int pos in toredraw)
                {
                    BlockEntity be = blocks[pos.x, pos.y];
                    be.GetBlockUVs(forblockuvs);
                    int ind = (pos.x + pos.y * size) * 4;
                    uvs[ind] = forblockuvs[0];
                    uvs[ind + 1] = forblockuvs[1];
                    uvs[ind + 2] = forblockuvs[2];
                    uvs[ind + 3] = forblockuvs[3];
                }
                gridMesh.SetUVs(0, uvs);
            }
        }

        public void UndrawGrid()
        {
            GameObject.Destroy(gridGO);
            GameObject.Destroy(gridMesh);
            uvs = null;
            Bootstrap.OnLateUpdate -= OnLateUpdate;
            visual = false;
        }
    }
}
