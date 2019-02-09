using UnityRandom = UnityEngine.Random;
using System;

namespace LuviKunG.Math
{
    [Serializable]
    public struct DeviationInt
    {
        [UnityEngine.SerializeField]
        public int value;
        [UnityEngine.SerializeField]
        public int deviation;
        public DeviationInt(int value, int deviation)
        {
            this.value = value;
            this.deviation = deviation;
        }
        public int Value { get { return value + UnityRandom.Range(-deviation, deviation); } }
        public static implicit operator int(DeviationInt v) { return v.Value; }
    }
}