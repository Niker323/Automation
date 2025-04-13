using System;
using UnityEngine;

namespace Automation
{
    [CreateAssetMenu(fileName = "Block", menuName = "Automation/Block")]
    public class Block : ScriptableObject
    {
        public int id;
        public GameObject gameObject;
    }
}
