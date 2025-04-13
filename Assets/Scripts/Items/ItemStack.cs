using Automation;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace Automation
{
    public struct ItemStack : IEquatable<ItemStack>
    {
        public static ItemStack EMPTY = new ItemStack(null, 0);
        public Item item;
        private Integer _count;
        public Integer count { get => _count; set => _count.Value = value; }

        public ItemStack(Item item, int count = 1)
        {
            this.item = item;
            this._count = count;
        }

        public static bool AreItemEqual(ItemStack itemStack1, ItemStack itemStack2)
        {
            return (itemStack1.IsEmpty() && itemStack2.IsEmpty()) || (itemStack1.IsEmpty() == itemStack2.IsEmpty() && itemStack1.item == itemStack2.item);
        }

        public bool IsEmpty()
        {
            return count <= 0 || item == null;
        }

        public ItemStack Copy()
        {
            return new ItemStack(item, count);
        }

        public ItemStack SplitStack(int size)
        {
            count -= size;
            return new ItemStack(item, size);
        }

        //public override string ToString()
        //{
        //    return item?.code + " " + count + " " + data?.ToString();
        //}

        public override int GetHashCode()
        {
            return HashCode.Combine(item.id.GetHashCode(), count.Value.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return obj is ItemStack && Equals((ItemStack)obj);
        }

        public bool Equals(ItemStack other)
        {
            return AreItemEqual(this, other);
        }

        public static bool operator ==(ItemStack left, ItemStack right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ItemStack left, ItemStack right)
        {
            return !left.Equals(right);
        }
    }
}
