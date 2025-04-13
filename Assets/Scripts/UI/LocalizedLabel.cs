using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace Automation.UI
{
    public class LocalizedLabel : Label
    {
        [UnityEngine.Scripting.Preserve]
        public new class UxmlFactory : UxmlFactory<LocalizedLabel, UxmlTraits> { }

        [UnityEngine.Scripting.Preserve]
        public new class UxmlTraits : Label.UxmlTraits
        {
            private UxmlStringAttributeDescription m_Key = new UxmlStringAttributeDescription
            {
                name = "localization-key"
            };
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription { get { yield break; } }

            public override void Init(VisualElement visualElement, IUxmlAttributes attributes, CreationContext creationContext)
            {
                base.Init(visualElement, attributes, creationContext);
                LocalizedLabel textElement = (LocalizedLabel)visualElement;
                textElement.localizationKey = m_Key.GetValueFromBag(attributes, creationContext);
            }
        }

        public string localizationKey { get; set; }

        public LocalizedLabel()
        {
            localizationKey = text;
            RegisterCallback<ChangeEvent<string>>(OnLabelChanged);
            RegisterCallback<AttachToPanelEvent>(OnAttach);
            RegisterCallback<DetachFromPanelEvent>(OnDetach);
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

        private void OnLabelChanged(ChangeEvent<string> e)
        {
            localizationKey = e.newValue;
            LocalizeLabel();
        }

        private void LocalizeLabel()
        {
            if (Application.isPlaying)
            {
                UnregisterCallback<ChangeEvent<string>>(OnLabelChanged);
                text = Lang.Get(localizationKey);
                RegisterCallback<ChangeEvent<string>>(OnLabelChanged);
            }
        }
    }
}
