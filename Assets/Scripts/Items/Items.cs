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
        public GameObject _itemIcon;
        public Item[] _items;
        bool inited = false;

        public void Init()
        {
            if (inited) return;
            inited = true;
            items = _items;
            itemIcon = _itemIcon;

            for (int i = 0; i < items.Length; i++)
            {
                Item item = items[i];
                item.id = i;
                if (item.inMarket) marketItems.Add(item);
            }
        }

        public static GameObject CreateItemSprite(Grid grid, Vector3 pos, Vector3 scale)
        {
            GameObject ret = GameObject.Instantiate(Items.itemIcon, grid.gridGO.transform);
            ret.transform.localPosition = pos;
            ret.transform.localScale = scale;
            return ret;
        }

        public static void PoolSprite(GameObject sprite)
        {
            GameObject.Destroy(sprite);
        }
    }
}