using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTNode : MonoBehaviour
{
    public TechTree.NodeState state = TechTree.NodeState.Locked;
    public event Action OnStateChange;
    public TTNode[] needNodes;
    public SpriteRenderer spriter;
    private Vector3 vec = new Vector3(0,0,0);
    private GameObject offsprite;
    private SpriteRenderer offspriter;

    void Start()
    {
        spriter = GetComponent<SpriteRenderer>();
        offsprite = transform.Find("Off").gameObject;
        offspriter = offsprite.GetComponent<SpriteRenderer>();
        TechUpdate();
    }

    void FixedUpdate()
    {
        if (state == TechTree.NodeState.CanResearch)
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
    }

    public void TechUpdate()
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
                state = TechTree.NodeState.CanResearch;
            }
        }
        if (state == TechTree.NodeState.Locked)
        {
            offsprite.SetActive(true);
            offspriter.color = new Color(1, 1, 1, 1);
        }
        else if(state == TechTree.NodeState.CanResearch)
        {
            offsprite.SetActive(true);
            //offspriter.color = new Color(255, 255, 255, 1);
        }
        else if(state == TechTree.NodeState.Researched)
        {
            offsprite.SetActive(false);
        }
        else
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
        if (Mathf.Abs(vec.x - Input.mousePosition.x) <= Screen.height/400f && Mathf.Abs(vec.y - Input.mousePosition.y) <= Screen.height/400f)
        {
            TechTree.instance.SelectTech(this);
        }
    }
}
