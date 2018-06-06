using System.Collections;
using UnityEngine;

namespace LuviKunG
{
    public class WaitforWatcher : IEnumerator
    {
        public delegate bool Watcher();
        public Watcher watcher;
        public WaitforWatcher(Watcher watch) { watcher = watch; }
        public object Current { get { return null; } }
        public bool MoveNext() { return !watcher(); }
        public void Reset() { }
    }

    public class WaitforWatcherYield : CustomYieldInstruction
    {
        public delegate bool Watcher();
        public Watcher watcher;
        public WaitforWatcherYield(Watcher watch) { watcher = watch; }
        public override bool keepWaiting { get { return !watcher(); } }
    }
}