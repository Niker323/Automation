using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechTree : MonoBehaviour
{
    public static TechTree instance;
    public Sprite defsprite;
    public Sprite selsprite;
    public TTNode[] ttpanels;
    public TTNode selectedtech;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < ttarr.Length; i++)
        //{
        //    ttarr[i] = PlayerPrefs.GetInt("tech_tree_" + i, ttarr[i]);
        //}
        //UpdateTree();
    }

    //public void UpdateTree()
    //{
    //    foreach (TTNode panel in ttpanels)
    //    {
    //        if (ttarr[panel.ttid] == 0)
    //        {
    //            locres = true;
    //            foreach (TTNode needpanel in panel.needNodes)
    //            {
    //                if (needpanel.state != NodeState.Researched)
    //                {
    //                    locres = false;
    //                }
    //            }
    //            if (locres)
    //            {
    //                ttarr[panel.ttid] = 1;
    //            }
    //        }
    //        panel.TechUpdate();
    //    }
    //}

    public void SelectTech(TTNode node)
    {
        if (node.state != NodeState.Locked)
        {
            if (selectedtech != null) selectedtech.spriter.sprite = defsprite;
            selectedtech = node;
            selectedtech.spriter.sprite = selsprite;
        }
    }

    public void Upgrade()
    {
        if (selectedtech.state == NodeState.CanResearch)
        {
            selectedtech.state = NodeState.Researched;
        }
    }

    public enum NodeState
    {
        Locked,
        CanResearch,
        Researched
    }
}
