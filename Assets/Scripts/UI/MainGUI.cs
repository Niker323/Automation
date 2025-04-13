using UnityEngine;
using UnityEngine.UIElements;

namespace Automation
{
    public class MainGUI : MonoBehaviour
    {
        public static Label money;
        public static Label fps;
        VisualElement root;
        void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            money = root.Q<Label>("money");
            fps = root.Q<Label>("fps");
        }

        void Update()
        {
            fps.text = (1.0f / Time.deltaTime).ToString();
        }
    }
}
