using Automation.BlockEntities;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automation
{
    public class MainGUI : MonoBehaviour
    {
        public static Label money;
        public static Label fps;
        public ToolMode toolMode;
        Button buildB, breakB, rotationB, cancelB, buildSelectB;
        VisualElement root;
        Vector3 downPos;
        Camera mainCamera;
        private bool md;
        private float dist;
        private float screencoof;

        void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            money = root.Q<Label>("money");
            fps = root.Q<Label>("fps");
            var back = root.Q("back");
            back.RegisterCallback<PointerDownEvent>(OnFieldDown);
            back.RegisterCallback<PointerUpEvent>(OnFieldUp);
            buildB = root.Q<Button>("build");
            buildB.clicked += () => { SetToolMode(ToolMode.Build); };
            breakB = root.Q<Button>("break");
            breakB.clicked += () => { SetToolMode(ToolMode.Break); };
            rotationB = root.Q<Button>("rotation");
            rotationB.clicked += () => { SetToolMode(ToolMode.Rotate); };
            cancelB = root.Q<Button>("cancel");
            cancelB.clicked += () => { SetToolMode(ToolMode.None); };
            cancelB.enabledSelf = false;
            buildSelectB = root.Q<Button>("buildSelect");
            mainCamera = Camera.main;
            screencoof = (Screen.height / 1000f) * 12;
            //For PC
            Bootstrap.OnUpdate += OnUpdate;
        }

        public void SetToolMode(ToolMode toolMode)
        {
            if (toolMode != this.toolMode)
            {
                switch (this.toolMode)
                {
                    case ToolMode.Build:
                        buildB.enabledSelf = true;
                        buildSelectB.style.display = DisplayStyle.None;
                        break;
                    case ToolMode.Break:
                        breakB.enabledSelf = true;
                        break;
                    case ToolMode.Rotate:
                        rotationB.enabledSelf = true;
                        break;
                    case ToolMode.None:
                        cancelB.enabledSelf = true;
                        break;
                }
                this.toolMode = toolMode;
                switch (this.toolMode)
                {
                    case ToolMode.Build:
                        buildB.enabledSelf = false;
                        buildSelectB.style.display = DisplayStyle.Flex;
                        break;
                    case ToolMode.Break:
                        breakB.enabledSelf = false;
                        break;
                    case ToolMode.Rotate:
                        rotationB.enabledSelf = false;
                        break;
                    case ToolMode.None:
                        cancelB.enabledSelf = false;
                        break;
                }
            }
        }

        private void OnFieldDown(PointerDownEvent evt)
        {
            downPos = evt.position;
            Bootstrap.OnUpdate += OnTouchUpdate;
            OnTouchUpdate();
        }

        void OnTouchUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.touchCount >= 2)
                {
                    Vector2 touch1 = Input.GetTouch(0).position;
                    Vector2 touch2 = Input.GetTouch(1).position;

                    if (dist == 0) dist = Vector2.Distance(touch1, touch2);

                    float delta = Vector2.Distance(touch1, touch2) - dist;

                    ChangeSize(delta / 180);

                    dist = Vector2.Distance(touch1, touch2);
                    if (md) md = false;
                }
                else
                {
                    if (md)
                    {
                        Vector3 nowpos = mainCamera.transform.position;
                        float newx = nowpos.x - Input.GetAxis("Mouse X") / screencoof;
                        float newy = nowpos.y - Input.GetAxis("Mouse Y") / screencoof;
                        float sizemult = 3.8f * (Bootstrap.instance.field.transform.localScale.x - 0.5f);
                        if (newx > sizemult) newx = sizemult;
                        else if (newx < -sizemult) newx = -sizemult;
                        if (newy > sizemult) newy = sizemult;
                        else if (newy < -sizemult) newy = -sizemult;
                        mainCamera.transform.position = new Vector3(newx, newy, nowpos.z);
                    }
                    else md = true;
                    if (dist != 0) dist = 0;
                }
            }
            else
            {
                if (md) md = false;
                if (dist != 0) dist = 0;
                Bootstrap.OnUpdate -= OnTouchUpdate;
            }
        }

        void OnUpdate()
        {
            float scroll = Input.mouseScrollDelta.y;
            if (scroll != 0)
            {
                ChangeSize(scroll / 18);
            }
        }

        void ChangeSize(float change)
        {
            float oldscale = Bootstrap.instance.field.transform.localScale.x;
            float newscale = oldscale + change / (Screen.height / 1500f);
            if (newscale > 8) newscale = 8;
            else if (newscale < 1) newscale = 1;
            float mult = newscale / oldscale;
            Bootstrap.instance.field.transform.localScale = new Vector3(newscale, newscale, 1);
            Vector3 nowpos = mainCamera.transform.position;
            float newx = nowpos.x * mult;
            float newy = nowpos.y * mult;
            float sizemult = 3.8f * (newscale - 0.5f);
            if (newx > sizemult) newx = sizemult;
            else if (newx < -sizemult) newx = -sizemult;
            if (newy > sizemult) newy = sizemult;
            else if (newy < -sizemult) newy = -sizemult;
            mainCamera.transform.position = new Vector3(newx, newy, nowpos.z);
        }

        private void OnFieldUp(PointerUpEvent evt)
        {
            Vector3 mpos = evt.position;
            if (Vector3.Distance(mpos, downPos) < 10)
            {
                mpos.y = Screen.height - mpos.y;
                Vector3 wpos = Camera.main.ScreenToWorldPoint(mpos);
                //Debug.Log(wpos);
                BlockEntity be = Bootstrap.instance.grid.GetBlockEntity(wpos / Bootstrap.instance.field.transform.localScale.x);
                if (be != null)
                {
                    switch (toolMode)
                    {
                        case ToolMode.Build:
                            if (be.block.id == 0)
                            {
                                Bootstrap.instance.grid.SetBlock(be.pos, 1);
                            }
                            break;
                        case ToolMode.Break:
                            if (be.block.id != 0)
                            {
                                Bootstrap.instance.grid.SetBlock(be.pos, 0);
                            }
                            break;
                        case ToolMode.Rotate:
                            if (be is SidedBlockEntity sbe)
                            {
                                sbe.side = sbe.side.GetCW();
                                Bootstrap.instance.grid.RedrawBlock(sbe.pos);
                            }
                            break;
                        case ToolMode.None:
                            break;
                    }
                }
            }
        }

        void Update()
        {
            fps.text = (1.0f / Time.deltaTime).ToString();
        }

        public enum ToolMode
        {
            None,
            Build,
            Break,
            Rotate
        }
    }
}
