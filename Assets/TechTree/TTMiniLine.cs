using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Automation
{
    public class TTMiniLine : MonoBehaviour
    {
        public TTNode[] ttpanelcomp;
        public Sprite[] sprites;
        private int maxtstate = 0;
        private SpriteRenderer thisspriter;
        private Sprite defaultsprites;

        void Start()
        {
            thisspriter = GetComponent<SpriteRenderer>();
            defaultsprites = thisspriter.sprite;
            foreach (var node in ttpanelcomp)
            {
                node.OnStateChange += UpdateStates;
            }
            UpdateStates();
        }

        void UpdateStates()
        {
            int locmaxtstate = 0;
            foreach (var node in ttpanelcomp)
            {
                if (locmaxtstate < (int)node.state)
                {
                    locmaxtstate = (int)node.state;
                }
            }
            if (locmaxtstate != maxtstate)
            {
                if (maxtstate == 1)
                {
                    TechTree.OnUpdate -= OnUpdate;
                }
                maxtstate = locmaxtstate;
                if (maxtstate == 0)
                {
                    thisspriter.color = new Color(0, 0, 0, 1);
                }
                else if (maxtstate == 1)
                {
                    thisspriter.color = new Color(1, 1, 1, 1);
                    TechTree.OnUpdate += OnUpdate;
                }
                else if (maxtstate == 2)
                {
                    thisspriter.color = new Color(1, 1, 1, 1);
                    thisspriter.sprite = defaultsprites;
                }
            }
        }

        void OnUpdate()
        {
            int sprnum = (int)((Time.time % 1) / 0.03125f % sprites.Length);
            thisspriter.sprite = sprites[sprnum];
        }
    }
}
