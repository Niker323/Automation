using System;
using UnityEngine;

namespace Automation
{
    public class Bootstrap : MonoBehaviour
    {
        public static event Action OnUpdate;
        public static event Action OnLogicUpdate;

        void Start()
        {
            Lang.Init();
            GetComponent<Blocks>().Init();
            new Grid().Init();
        }

        void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}
