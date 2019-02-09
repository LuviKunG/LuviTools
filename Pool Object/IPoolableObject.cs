using UnityEngine;

namespace LuviKunG.Pool
{
    public interface IPoolable
    {
        void PoolReset();
    }

    public interface IPoolableActivatable : IPoolable
    {
        void PoolActive();
        void PoolDeactive();
    }

    public interface IPoolableComponentFixedList : IPoolable
    {
        void PoolRegister(PoolComponentFixedList parent);
        PoolComponentFixedList GetPool { get; }
    }

    public interface IPoolableObjectFlexibleClass<TObject> : IPoolableActivatable where TObject : Object, IPoolableObjectFlexibleClass<TObject>
    {
        void PoolRegister(PoolObjectFlexibleClass<TObject> parent);
        PoolObjectFlexibleClass<TObject> GetPool { get; }
    }
}