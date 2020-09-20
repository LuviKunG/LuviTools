using UnityEngine;

namespace LuviKunG.Attributes
{
    public class AnimatorParameterAttribute : PropertyAttribute
    {
        public string animatorPropertyName;

        public AnimatorParameterAttribute(string animatorPropertyName)
        {
            this.animatorPropertyName = animatorPropertyName;
        }
    }
}