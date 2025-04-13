using System;
using UnityEngine;

namespace Automation
{
    public class Bootstrap : MonoBehaviour
    {
        public static Bootstrap instance;
        public static event Action OnUpdate;
        public static event Action OnLogicUpdate;
        public GameObject field;
        public Material gridMaterial;
        double time;
        const double tickTime = 0.1f;
        Grid grid = new Grid();

        void Start()
        {
            instance = this;
            Lang.Init();
            GetComponent<Blocks>().Init();
            grid.Init();
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time > tickTime)
            {
                time = time % tickTime;
                OnLogicUpdate?.Invoke();
            }
            OnUpdate?.Invoke();
        }
    }
}
