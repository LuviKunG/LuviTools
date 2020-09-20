using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG.Attributes
{
    public class AnimatorNameAttribute : PropertyAttribute
    {
        public string animatorPropertyName;

        public AnimatorNameAttribute(string animatorPropertyName)
        {
            this.animatorPropertyName = animatorPropertyName;
        }
    }
}