using System.Collections.Generic;

namespace Game
{
    public sealed class SubSystem<T> where T : ISubSystem
    {
        private List<T> m_list;
        private T m_current;

        public SubSystem()
        {
            m_list = new List<T>();
            m_current = default;
        }

        public void AddControl(T controller)
        {
            if (controller == null)
                return;
            m_list.Add(controller);
            UpdateControl();
        }

        public bool RemoveControl(T controller)
        {
            if (controller == null)
                return false;
            bool isRemoved = m_list.Remove(controller);
            UpdateControl();
            return isRemoved;
        }

        private void UpdateControl()
        {
            if (m_current != null)
                m_current.Unbind();
            m_current = m_list.Count > 0 ? m_list[m_list.Count - 1] : default;
            m_current?.Bind();
        }
    }
}