using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Automation
{
    public class TechTree : MonoBehaviour
    {
        public static TechTree instance;
        public static event Action OnUpdate;
        public static Dictionary<string, TTNode> nodes = new Dictionary<string, TTNode>();
        public static TTNode selectedtech;
        public static event Action<string> OnResearchTech;
        public Sprite defsprite;
        public Sprite selsprite;

        private void Awake()
        {
            instance = this;
        }

        void Update()
        {
            OnUpdate?.Invoke();
        }

        public static void Init()
        {

        }

        public static void SelectTech(TTNode node)
        {
            if (node.state != NodeState.Locked)
            {
                if (selectedtech != null)
                {
                    if (selectedtech == node) Upgrade();
                    else selectedtech.spriter.sprite = instance.defsprite;
                }
                selectedtech = node;
                selectedtech.spriter.sprite = instance.selsprite;
            }
        }

        public static void Upgrade()
        {
            if (selectedtech.state == NodeState.CanResearch)
            {
                selectedtech.SetState(NodeState.Researched);
                OnResearchTech?.Invoke(selectedtech.name);
            }
        }

        public enum NodeState
        {
            Locked,
            CanResearch,
            Researched
        }
    }
}
