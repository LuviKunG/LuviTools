using UnityEngine;

namespace LuviKunG.Attributes
{
    public class AnimatorHashAttribute : PropertyAttribute
    {
        public string animatorPropertyName;

        public AnimatorHashAttribute(string animatorPropertyName)
        {
            this.animatorPropertyName = animatorPropertyName;
        }
    }
}