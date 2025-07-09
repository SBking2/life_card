using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QF
{
    public class MonoSingleton<T> : MonoBehaviour where T : class
    {
        private static T m_Instance = null;
        protected virtual void Awake()
        {
            if(m_Instance == null)
            {
                m_Instance = this as T;
            }
        }
        public static T Instance
        {
            get
            {
                return m_Instance;
            }
        }
    }
}
