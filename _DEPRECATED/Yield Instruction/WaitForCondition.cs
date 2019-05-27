using UnityEngine;

public class WaitForCondition : CustomYieldInstruction
{
    public delegate bool ConditionDelegate();
    public ConditionDelegate watcher;
    public WaitForCondition(ConditionDelegate watch) { watcher = watch; }
    public override bool keepWaiting { get { return !watcher(); } }
}
