using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTLine : MonoBehaviour
{
    public TTNode ttpanelcomp;
    public Sprite[] sprites;
    private SpriteRenderer thisspriter;
    private Sprite defaultsprites;

    void Start()
    {
        thisspriter = GetComponent<SpriteRenderer>();
        defaultsprites = thisspriter.sprite;
    }

    void FixedUpdate()
    {
        if (ttpanelcomp.state == TechTree.NodeState.Locked)
        {
            if (thisspriter.color != new Color(0, 0, 0, 1))
            {
                thisspriter.color = new Color(0, 0, 0, 1);
            }
        }
        else if (ttpanelcomp.state == TechTree.NodeState.CanResearch)
        {
            if (thisspriter.color != new Color(1, 1, 1, 1))
            {
                thisspriter.color = new Color(1, 1, 1, 1);
            }
            int sprnum = (int)((Time.time % 1) / 0.03125f % sprites.Length);
            thisspriter.sprite = sprites[sprnum];
        }
        else if (ttpanelcomp.state == TechTree.NodeState.Researched)
        {
            if (thisspriter.color != new Color(1, 1, 1, 1))
            {
                thisspriter.color = new Color(1, 1, 1, 1);
            }
            if (thisspriter.sprite != defaultsprites)
            {
                thisspriter.sprite = defaultsprites;
            }
        }
    }
}
