using System;
using UnityEngine;

namespace Automation
{
    [CreateAssetMenu(fileName = "Block", menuName = "Automation/Block")]
    public class Block : ScriptableObject
    {
        [NonSerialized]
        public int id;
        public Texture2D icon;
        public Texture2D downTexture;
        public int downFrames = 1;
        public int downFPS = 1;
        [NonSerialized]
        public Rect downRect;
        [NonSerialized]
        public float downAnimData;
        public Texture2D upTexture;
        public int upFrames = 1;
        public int upFPS = 1;
        [NonSerialized]
        public Rect upRect;
        [NonSerialized]
        public float upAnimData;
        public string blockEntity = "BlockEntity";
        public Type blockEntityType;
    }
}
