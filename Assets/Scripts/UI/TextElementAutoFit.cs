using Unity.Profiling;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automation.UI
{
    public class TextElementAutoFit
    {
#if ENABLE_PROFILER
        static ProfilerMarker markerUFS = new ProfilerMarker(ProfilerCategory.Scripts, "TextElementAutoFit.UpdateFontSize");
#endif
        private TextElement textElement;

        public TextElementAutoFit(TextElement textElement)
        {
            this.textElement = textElement;
            if (textElement.style.width == StyleKeyword.Auto || textElement.style.height == StyleKeyword.Auto)
                Debug.LogError("Auto size not allowed on TextElementAutoFit");
            if (textElement.panel != null)
            {
                UpdateFontSize();
                textElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
                textElement.RegisterCallback<ChangeEvent<string>>(OnLableChanged);
            }
            else textElement.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent e)
        {
            textElement.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            textElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            textElement.RegisterCallback<ChangeEvent<string>>(OnLableChanged);
        }

        private void OnGeometryChanged(GeometryChangedEvent e)
        {
            UpdateFontSize();
        }

        private void OnLableChanged(ChangeEvent<string> e)
        {
            UpdateFontSize();
        }

        private void UpdateFontSize()
        {
#if ENABLE_PROFILER
            markerUFS.Begin();
#endif
            textElement.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);

            try
            {
                textElement.style.fontSize = new StyleLength(new Length(100));
                var currentFontSize = textElement.MeasureTextSize(textElement.text, 0, VisualElement.MeasureMode.Undefined, 0, VisualElement.MeasureMode.Undefined);
                //Debug.Log(textElement.contentRect.width + " " + textElement.contentRect.height);
                //Debug.Log(currentFontSize);

                float d1 = Mathf.Max(Mathf.Max(currentFontSize.x, 1) / textElement.contentRect.width, 1);
                float d2 = Mathf.Max(Mathf.Max(currentFontSize.y, 1) / textElement.contentRect.height, 1);
                var newFontSize = 100 / Mathf.Max(d1, d2);
                //Debug.LogWarning(textElement.name + " " + newFontSize);

                textElement.style.fontSize = new StyleLength(new Length(newFontSize));
            }
            finally
            {
                textElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            }
#if ENABLE_PROFILER
            markerUFS.End();
#endif
        }
    }
}