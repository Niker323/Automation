using System;
using UnityEngine;

namespace Automation
{
    public class Bootstrap : MonoBehaviour
    {
        const double tickTime = 0.1f;
        public static Bootstrap instance;
        public static event Action OnUpdate;
        public static event Action OnLogicUpdate;
        public static event Action OnLateUpdate;
        public GameObject field;
        public Material gridMaterial;
        public Grid grid = new Grid();
        double time;

        void Start()
        {
            instance = this;
            Lang.Init();
            GetComponent<Blocks>().Init();
            grid.Init("Grid1");
            grid.DrawGrid();
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

        void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
    }
}
