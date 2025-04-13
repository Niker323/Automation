using System;
using System.Drawing;
using UnityEngine;

namespace Automation
{
    public class Blocks : MonoBehaviour
    {
        public static Block[] blocks;
        public Block[] _blocks;

        public void Init()
        {
            blocks = _blocks;
        }
    }
}
