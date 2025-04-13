using UnityEngine;
using UnityEngine.UIElements;

namespace Automation
{
    public class MainGUI : MonoBehaviour
    {
        VisualElement root;
        void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
        }
    }
}
