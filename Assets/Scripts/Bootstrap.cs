using System;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public static event Action OnUpdate;
    public static event Action OnLogicUpdate;

    void Start()
    {
        
    }

    void Update()
    {
        OnUpdate?.Invoke();
    }
}
