using System;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    public class Items : MonoBehaviour
    {
        public static Item[] items;
        public static List<Item> marketItems = new List<Item>();
        public static GameObject itemIcon;
        public static Stack<GameObject> pool = new Stack<GameObject>();
        public GameObject _itemIcon;
        public Item[] _items;
        bool inited = false;

        public void Init()
        {
            if (inited) return;
            inited = true;
            items = _items;
            itemIcon = _itemIcon;

            marketItems.Add(null);
            for (int i = 0; i < items.Length; i++)
            {
                Item item = items[i];
                item.id = i;
                if (item.inMarket) marketItems.Add(item);
            }
        }

        public static GameObject CreateItemSprite(Grid grid, Vector3 pos, Vector3 scale)
        {
            GameObject ret;
            if (pool.TryPop(out ret))
            {
                ret.SetActive(true);
            }
            else
            {
                ret = GameObject.Instantiate(Items.itemIcon, grid.gridGO.transform);
            }
            ret.transform.localPosition = pos;
            ret.transform.localScale = scale;
            return ret;
        }

        public static void PoolSprite(GameObject sprite)
        {
            sprite.SetActive(false);
            pool.Push(sprite);
        }

        public static void ClearPool()
        {
            pool.Clear();
        }
    }
}