using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automation.UI
{
    public class LabelAutoFit : Label
    {
        [UnityEngine.Scripting.Preserve]
        public new class UxmlFactory : UxmlFactory<LabelAutoFit, UxmlTraits> { }

        [UnityEngine.Scripting.Preserve]
        public new class UxmlTraits : Label.UxmlTraits
        {
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription { get { yield break; } }

            public override void Init(VisualElement visualElement, IUxmlAttributes attributes, CreationContext creationContext)
            {
                base.Init(visualElement, attributes, creationContext);
            }
        }

        public LabelAutoFit()
        {
            new TextElementAutoFit(this);
        }
    }
}
