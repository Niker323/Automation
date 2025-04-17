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
        public static float time;
        public GameObject field;
        public Material gridMaterial;
        public long money { get; private set; }
        public Grid grid = new Grid();
        public Items items;
        public Blocks blocks;
        public MainGUI mainGUI;
        double dt;

        void Start()
        {
            instance = this;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;
            Lang.Init();
            items.Init();
            blocks.Init();
            money = long.Parse(PlayerPrefs.GetString("moneyValue", "0"));
            TechTree.Init();
            grid.Init("Grid1");
            grid.DrawGrid();
            mainGUI.Init();
        }

        public void ChangeMoney(int change)
        {
            money += change;
        }

        void Update()
        {
            dt += Time.deltaTime;
            time = Time.time;
            if (dt > tickTime)
            {
                dt = dt % tickTime;
                OnLogicUpdate?.Invoke();
                PlayerPrefs.SetString("moneyValue", money.ToString());
                MainGUI.money.text = money.ToString();
            }
            OnUpdate?.Invoke();
        }

        void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
    }
}
