using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automation.UI
{
    [UxmlElement]
    public partial class LabelAutoFit : Label
    {
        public LabelAutoFit()
        {
            new TextElementAutoFit(this);
        }
    }
}
