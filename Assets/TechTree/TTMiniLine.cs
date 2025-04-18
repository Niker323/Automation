using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void FixedUpdate()
    {
        for (int i = 0; i < ttpanelcomp.Length; i++)
        {
            if (maxtstate < (int)ttpanelcomp[i].state)
            {
                maxtstate = (int)ttpanelcomp[i].state;
            }
        }
        if (maxtstate == 0)
        {
            if (thisspriter.color != new Color(0, 0, 0, 1))
            {
                thisspriter.color = new Color(0, 0, 0, 1);
            }
        }
        else if (maxtstate == 1)
        {
            if (thisspriter.color != new Color(1, 1, 1, 1))
            {
                thisspriter.color = new Color(1, 1, 1, 1);
            }
            int sprnum = (int)((Time.time % 1) / 0.03125f % sprites.Length);
            thisspriter.sprite = sprites[sprnum];
        }
        else if (maxtstate == 2)
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
