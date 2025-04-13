using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace Automation.UI
{
    [UxmlElement]
    public partial class LocalizedLabel : Label
    {
        [UxmlAttribute("localization-key")]
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
