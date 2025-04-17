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
        public const float cellSize = 0.32f;
        public int size { get; private set; }
        public string name { get; private set; }
        public GameObject gridGO { get; private set; }
        public bool visual { get; private set; }
        int verts;
        BlockEntity[,] blocks;
        Mesh downGridMesh, upGridMesh;
        Vector3[] downUVs, upUVs;
        HashSet<Vector2Int> toredraw = new HashSet<Vector2Int>();
        static Vector3[] forblockuvs = new Vector3[8];

        public void Init(string gridName, int size)
        {
            this.size = size;
            verts = size * size * 4;
            blocks = new BlockEntity[size, size];
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


        public BlockEntity GetBlockEntity(Vector2Int pos)
        {
            if (pos.x >= 0 && pos.x < size && pos.y >= 0 && pos.y < size)
                return blocks[pos.x, pos.y];
            else 
                return null;
        }

        public BlockEntity GetBlockEntity(Vector3 pos)
        {
            float halfField = cellSize * (size / 2);
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

        public void TryBuildBlock(int x, int y, Block block)
        {
            SetBlock(x, y, block.id);
        }

        public void TryBuildBlock(Vector2Int pos, Block block) => TryBuildBlock(pos.x, pos.y, block);

        public void SellBlock(int x, int y)
        {
            SetBlock(x, y, 0);
        }

        public void SellBlock(Vector2Int pos) => SellBlock(pos.x, pos.y);

        public void SetBlock(int x, int y, int id)
        {
            Block block = Blocks.blocks[id];
            BlockEntity blockEntity = Activator.CreateInstance(block.blockEntityType) as BlockEntity;
            blockEntity.grid = this;
            blockEntity.block = block;
            blockEntity.pos = new Vector2Int(x, y);
            blocks[x, y]?.OnRemoved();
            blocks[x, y] = blockEntity;
            PlayerPrefs.SetInt($"{name}_{x}_{y}", id);
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
            visual = true;
            gridGO = new GameObject("Grid");
            gridGO.transform.parent = Bootstrap.instance.field.transform;
            Vector3[] vertices = new Vector3[verts];
            int[] indices = new int[verts];
            for (int i = 0; i < indices.Length; i++)
                indices[i] = i;
            downUVs = new Vector3[verts];
            upUVs = new Vector3[verts];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    BlockEntity be = blocks[x, y];

                    be.DrawBlockEntity();

                    float xPos = x * cellSize;
                    float yPos = y * cellSize;
                    int ind = (x + y * size) * 4;
                    vertices[ind] = new Vector3(xPos, yPos, 0);
                    vertices[ind + 1] = new Vector3(xPos + cellSize, yPos, 0);
                    vertices[ind + 2] = new Vector3(xPos + cellSize, yPos + cellSize, 0);
                    vertices[ind + 3] = new Vector3(xPos, yPos + cellSize, 0);

                    be.GetBlockUVs(forblockuvs);
                    downUVs[ind] = forblockuvs[0];
                    downUVs[ind + 1] = forblockuvs[1];
                    downUVs[ind + 2] = forblockuvs[2];
                    downUVs[ind + 3] = forblockuvs[3];

                    upUVs[ind] = forblockuvs[4];
                    upUVs[ind + 1] = forblockuvs[5];
                    upUVs[ind + 2] = forblockuvs[6];
                    upUVs[ind + 3] = forblockuvs[7];
                }
            }
            downGridMesh = new Mesh();
            downGridMesh.MarkDynamic();
            downGridMesh.vertices = vertices;
            downGridMesh.SetIndices(indices, MeshTopology.Quads, 0);
            downGridMesh.SetUVs(0, downUVs);
            downGridMesh.RecalculateBounds();
            gridGO.transform.localPosition = new Vector3(0, 0, -1) - downGridMesh.bounds.center;
            gridGO.transform.localScale = Vector3.one;
            gridGO.AddComponent<MeshFilter>().mesh = downGridMesh;
            gridGO.AddComponent<MeshRenderer>().material = Bootstrap.instance.gridMaterial;

            GameObject upgridGO = new GameObject("UpGrid");
            upgridGO.transform.parent = gridGO.transform;
            upGridMesh = new Mesh();
            upGridMesh.MarkDynamic();
            upGridMesh.vertices = vertices;
            upGridMesh.SetIndices(indices, MeshTopology.Quads, 0);
            upGridMesh.SetUVs(0, upUVs);
            upGridMesh.RecalculateBounds();
            upgridGO.transform.localPosition = new Vector3(0, 0, -5);
            upgridGO.transform.localScale = Vector3.one;
            upgridGO.AddComponent<MeshFilter>().mesh = upGridMesh;
            upgridGO.AddComponent<MeshRenderer>().material = Bootstrap.instance.gridMaterial;
            Bootstrap.OnLateUpdate += OnLateUpdate;
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
                    downUVs[ind] = forblockuvs[0];
                    downUVs[ind + 1] = forblockuvs[1];
                    downUVs[ind + 2] = forblockuvs[2];
                    downUVs[ind + 3] = forblockuvs[3];

                    upUVs[ind] = forblockuvs[4];
                    upUVs[ind + 1] = forblockuvs[5];
                    upUVs[ind + 2] = forblockuvs[6];
                    upUVs[ind + 3] = forblockuvs[7];
                }
                downGridMesh.SetUVs(0, downUVs);
                upGridMesh.SetUVs(0, upUVs);
            }
        }

        public void UndrawGrid()
        {
            GameObject.Destroy(gridGO);
            GameObject.Destroy(upGridMesh);
            GameObject.Destroy(downGridMesh);
            Items.ClearPool();
            downUVs = null;
            upUVs = null;
            Bootstrap.OnLateUpdate -= OnLateUpdate;
            visual = false;
        }
    }
}
