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
        public static long money { get; private set; }
        public static long mps { get; private set; }
        public GameObject field;
        public GameObject gridField;
        public GameObject techField;
        public Material gridMaterial;
        public Grid grid = new Grid();
        public Items items;
        public Blocks blocks;
        public MainGUI mainGUI;
        double dt;
        int tick;
        static long tempAdd;

        void Start()
        {
            instance = this;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;
            Lang.Init();
            items.Init();
            blocks.Init();
            money = long.Parse(PlayerPrefs.GetString("moneyValue", "0"));
            mps = long.Parse(PlayerPrefs.GetString("moneyPerSecond", "0"));
            //TechTree.Init();
            grid.Init("Grid1", 20);
            grid.DrawGrid();
            mainGUI.Init();
        }

        public static void ChangeMoney(long change)
        {
            money += change;
            if (change > 0)
            {
                tempAdd += change;
            }
        }

        public static void ChangeMoney(int change) => ChangeMoney(change);

        void Update()
        {
            dt += Time.deltaTime;
            time = Time.time;
            if (dt > tickTime)
            {
                dt = dt % tickTime;
                OnLogicUpdate?.Invoke();
                tick++;
                PlayerPrefs.SetString("moneyValue", money.ToString());
                MainGUI.money.text = StringUtil.NumberFormat(money);
                if (tick % 10 == 0)
                {
                    mps = (mps + tempAdd) / 2;
                    tempAdd = 0;
                    MainGUI.mps.text = StringUtil.NumberFormat(mps);
                    PlayerPrefs.SetString("moneyValue", mps.ToString());
                }
            }
            OnUpdate?.Invoke();
        }

        void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
    }
}
