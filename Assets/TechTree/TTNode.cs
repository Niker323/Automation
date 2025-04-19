using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    [SelectionBase]
    public class TTNode : MonoBehaviour
    {
        public TechTree.NodeState state = TechTree.NodeState.Locked;
        public event Action OnStateChange;
        public TTNode[] needNodes;
        [NonSerialized]
        public SpriteRenderer spriter;
        private Vector3 vec = new Vector3(0, 0, 0);
        private GameObject offsprite;
        private SpriteRenderer offspriter;

        private void Awake()
        {
            if (state == TechTree.NodeState.CanResearch) state = TechTree.NodeState.Locked;
            TechTree.nodes.Add(name, this);
        }

        void Start()
        {
            spriter = GetComponent<SpriteRenderer>();
            offsprite = transform.Find("Off").gameObject;
            offspriter = offsprite.GetComponent<SpriteRenderer>();
            TechUpdate();
            UpdateNeeds();
            foreach (var node in needNodes)
                node.OnStateChange += UpdateNeeds;
        }

        void OnUpdate()
        {
            float ost = Time.time % 1;
            if (ost > 0.5f)
            {
                offspriter.color = new Color(1, 1, 1, 1.2f - Time.time % 1);
            }
            else
            {
                offspriter.color = new Color(1, 1, 1, 0.2f + Time.time % 1);
            }
        }

        public void SetState(TechTree.NodeState state)
        {
            if (this.state != state)
            {
                if (this.state == TechTree.NodeState.CanResearch) Bootstrap.OnUpdate -= OnUpdate;
                this.state = state;
                if (this.state == TechTree.NodeState.CanResearch) Bootstrap.OnUpdate += OnUpdate;
                OnStateChange?.Invoke();
                TechUpdate();
            }
        }

        private void UpdateNeeds()
        {
            if (state != TechTree.NodeState.Researched)
            {
                bool can = true;
                foreach (var node in needNodes)
                {
                    if (node.state != TechTree.NodeState.Researched)
                    {
                        can = false;
                        break;
                    }
                }
                if (can)
                {
                    SetState(TechTree.NodeState.CanResearch);
                }
            }
        }

        private void TechUpdate()
        {
            if (state == TechTree.NodeState.Locked)
            {
                offsprite.SetActive(true);
                offspriter.color = new Color(1, 1, 1, 1);
            }
            else if (state == TechTree.NodeState.CanResearch)
            {
                offsprite.SetActive(true);
            }
            else if (state == TechTree.NodeState.Researched)
            {
                offsprite.SetActive(false);
            }
        }

        void OnMouseDown()
        {
            vec.x = Input.mousePosition.x;
            vec.y = Input.mousePosition.y;
        }

        void OnMouseUp()
        {
            if (Mathf.Abs(vec.x - Input.mousePosition.x) <= Screen.height / 200f && Mathf.Abs(vec.y - Input.mousePosition.y) <= Screen.height / 200f && vec.y > Screen.height / 12f)
            {
                TechTree.SelectTech(this);
            }
        }
    }
}
