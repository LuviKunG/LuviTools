using UnityRandom = UnityEngine.Random;
using System;

namespace LuviKunG.Math
{
    [Serializable]
    public struct DeviationFloat
    {
        [UnityEngine.SerializeField]
        private float value;
        [UnityEngine.SerializeField]
        private float deviation;
        public DeviationFloat(float value, float deviation)
        {
            this.value = value;
            this.deviation = deviation;
        }
        public float Value { get { return value + UnityRandom.Range(-deviation, deviation); } }
        public static implicit operator float(DeviationFloat v) { return v.Value; }
    }
}