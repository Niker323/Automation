using UnityEngine.UIElements;

namespace Automation.UI
{
    public class TextElementLocalizer
    {
        private TextElement textElement;
        private string key;

        public TextElementLocalizer(TextElement textElement)
        {
            this.textElement = textElement;
            key = textElement.text;
            LocalizeLabel();
            if (textElement.parent != null) Lang.onLangChanged += LocalizeLabel;
            textElement.RegisterCallback<AttachToPanelEvent>(OnAttach);
            textElement.RegisterCallback<DetachFromPanelEvent>(OnDetach);
        }

        private void OnAttach(AttachToPanelEvent evt)
        {
            LocalizeLabel();
            Lang.onLangChanged += LocalizeLabel;
        }

        private void OnDetach(DetachFromPanelEvent evt)
        {
            Lang.onLangChanged -= LocalizeLabel;
        }

        private void LocalizeLabel()
        {
            textElement.text = Lang.Get(key);
        }
    }
}