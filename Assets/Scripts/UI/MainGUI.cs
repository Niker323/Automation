using Automation.BlockEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automation
{
    public class MainGUI : MonoBehaviour
    {
        public static Label money;
        public static Label mps;
        public static Label fps;
        public ToolMode toolMode;
        static Button buildB, breakB, rotationB, cancelB, buildSelectB;
        static VisualElement root;
        static VisualElement midPanel;
        static Vector3 downPos;
        static Camera mainCamera;
        private bool md;
        private float dist;
        private float screencoof;
        private Block buildBlock;
        private Vector3 tempCamPos;
        const float camBorderCoof = 5;

        public void Init()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            UpdateSafeArea(null);
            root.Q("safeArea").RegisterCallback<GeometryChangedEvent>(UpdateSafeArea);

            money = root.Q<Label>("money");
            mps = root.Q<Label>("mps");
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
            buildSelectB.clicked += () =>
            {
                OpenMidPanel("selectBuild");
            };
            ListView blocksL = root.Q<ListView>("blockList");
            blocksL.makeItem = () =>
            {
                VisualElement itemElem = new VisualElement();

                Label name = new Label();
                name.style.marginTop = 0;
                itemElem.Add(name);

                //Label wpath = new Label();
                //wpath.style.color = Color.gray;
                //wpath.style.marginTop = 0;
                //itemElem.Add(wpath);

                itemElem.userData = name;
                return itemElem;
            };
            blocksL.bindItem = (e, i) =>
            {
                var items = (Label)e.userData;
                items.text = Blocks.buildBlockList[i].code;
            };
            blocksL.itemsSource = Blocks.buildBlockList;
            blocksL.itemsChosen += (obj) =>
            {
                IEnumerator<object> enumerator = obj.GetEnumerator();
                enumerator.MoveNext();
                SetBuildBlock(enumerator.Current as Block);
                CloseMidPanel();
            };

            ListView itemsL = root.Q<ListView>("itemList");
            itemsL.makeItem = () =>
            {
                VisualElement itemElem = new VisualElement();

                Label name = new Label();
                name.style.marginTop = 0;
                itemElem.Add(name);

                //Label wpath = new Label();
                //wpath.style.color = Color.gray;
                //wpath.style.marginTop = 0;
                //itemElem.Add(wpath);

                itemElem.userData = name;
                return itemElem;
            };
            itemsL.bindItem = (e, i) =>
            {
                var items = (Label)e.userData;
                Item mitem = Items.marketItems[i];
                items.text = mitem == null ? "Empty" : mitem.code;
            };
            itemsL.itemsSource = Items.marketItems;
            itemsL.itemsChosen += (obj) =>
            {
                IEnumerator<object> enumerator = obj.GetEnumerator();
                enumerator.MoveNext();
                Market.selected?.SetItem(enumerator.Current as Item);
                CloseMidPanel();
            };

            root.Query<Button>("closeMid").ForEach((btn) => { btn.clicked += CloseMidPanel; });
            mainCamera = Camera.main;
            tempCamPos = mainCamera.transform.position;
            screencoof = (Screen.height / 1000f) * 10;
            SetBuildBlock(Blocks.blocks[1]);
            root.Q<Button>("techTree").clicked += OpenTechTree;
            root.Q<Button>("closeTechTree").clicked += CloseTechTree;
            //For PC
            Bootstrap.OnUpdate += OnUpdate;
            root.Q<Button>("hide").clicked += () => { Bootstrap.instance.grid.UndrawGrid(); };
            root.Q<Button>("show").clicked += () => { Bootstrap.instance.grid.DrawGrid(); };
        }

        void OpenTechTree()
        {
            Vector3 temp = mainCamera.transform.position;
            mainCamera.transform.position = tempCamPos;
            tempCamPos = temp;
            Bootstrap.instance.field = Bootstrap.instance.techField;
            Bootstrap.instance.techField.SetActive(true);
            Bootstrap.instance.grid.UndrawGrid();
        }

        void CloseTechTree()
        {
            Vector3 temp = mainCamera.transform.position;
            mainCamera.transform.position = tempCamPos;
            tempCamPos = temp;
            Bootstrap.instance.field = Bootstrap.instance.gridField;
            Bootstrap.instance.techField.SetActive(false);
            Bootstrap.instance.grid.DrawGrid();
        }

        private void UpdateSafeArea(GeometryChangedEvent evt)
        {
            try
            {
                VisualElement safeAreaVE = root.Q("safeArea");
                Rect safeArea = Screen.safeArea;
                Vector2 leftTop = RuntimePanelUtils.ScreenToPanel(safeAreaVE.panel, new Vector2(safeArea.xMin, Screen.height - safeArea.yMax));
                Vector2 rightBottom = RuntimePanelUtils.ScreenToPanel(safeAreaVE.panel, new Vector2(Screen.width - safeArea.xMax, safeArea.yMin));
                safeAreaVE.style.paddingLeft = leftTop.x;
                safeAreaVE.style.paddingTop = leftTop.y;
                safeAreaVE.style.paddingRight = rightBottom.x;
                safeAreaVE.style.paddingBottom = rightBottom.y;
            }
            catch (Exception e)
            {
                VisualElement safeAreaVE = root.Q("safeArea");
                safeAreaVE.style.paddingLeft = 0;
                safeAreaVE.style.paddingTop = 0;
                safeAreaVE.style.paddingRight = 0;
                safeAreaVE.style.paddingBottom = 0;
            }
        }

        private void SetBuildBlock(Block block)
        {
            buildBlock = block;
            buildSelectB.iconImage = block.icon;
        }

        public static void CloseMidPanel()
        {
            var oldMid = midPanel;
            midPanel.style.scale = new Scale(new Vector2(0.75f, 0.75f));
            midPanel.style.opacity = 0;
            midPanel.schedule.Execute(() => { oldMid.style.display = DisplayStyle.None; })
                .ExecuteLater((long)(oldMid.resolvedStyle.transitionDuration.First().value * 1000));
            midPanel = null;
        }

        public static void OpenMidPanel(string name)
        {
            VisualElement panel = root.Q(name);
            if (midPanel != null)
            {
                midPanel.style.display = DisplayStyle.None;
                midPanel.style.scale = new Scale(new Vector2(0.75f, 0.75f));
                midPanel.style.opacity = 0;
            }
            midPanel = panel;
            midPanel.style.display = DisplayStyle.Flex;
            midPanel.style.scale = new Scale(new Vector2(1, 1));
            midPanel.style.opacity = 1;
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
                        float sizemult = camBorderCoof * (Bootstrap.instance.field.transform.localScale.x - 0.5f);
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
            fps.text = (1.0f / Time.deltaTime).ToString();
        }

        void ChangeSize(float change)
        {
            float oldscale = Bootstrap.instance.field.transform.localScale.x;
            float newscale = oldscale + change / (Screen.height / 1500f);
            if (newscale > 8) newscale = 8;
            else if (newscale < 0.6f) newscale = 0.6f;
            float mult = newscale / oldscale;
            Bootstrap.instance.field.transform.localScale = new Vector3(newscale, newscale, 1);
            Vector3 nowpos = mainCamera.transform.position;
            float newx = nowpos.x * mult;
            float newy = nowpos.y * mult;
            float sizemult = camBorderCoof * (newscale - 0.5f);
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
                                Bootstrap.instance.grid.TryBuildBlock(be.pos, buildBlock);
                            }
                            break;
                        case ToolMode.Break:
                            if (be.block.id != 0)
                            {
                                Bootstrap.instance.grid.SellBlock(be.pos);
                            }
                            break;
                        case ToolMode.Rotate:
                            if (be is SidedBlockEntity sbe)
                            {
                                sbe.SetRotation(sbe.side.GetCW());
                            }
                            break;
                        case ToolMode.None:
                            be.OnUse();
                            break;
                    }
                }
            }
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
