using System;
using System.Collections;
using UnityEngine;

namespace LuviKunG
{
    public class WaitForTimeSpan : IEnumerator
    {
        DateTime current;
        TimeSpan additive;
        public WaitForTimeSpan(TimeSpan duration) { additive = duration; Reset(); }
        public object Current { get { return current; } }
        public bool MoveNext() { return current > DateTime.Now; }
        public void Reset() { current = DateTime.Now.Add(additive); }
    }

    public class WaitForTimeSpanYield : CustomYieldInstruction
    {
        DateTime current;
        TimeSpan additive;
        public WaitForTimeSpanYield(TimeSpan duration) { additive = duration; current = DateTime.Now.Add(additive); }
        public override bool keepWaiting { get { return current > DateTime.Now; } }
    }
}