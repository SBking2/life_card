using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace QF
{
    public class Singleton<T> where T : class, new()
    {
        public static T Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new T();
                }

                return m_Instance;
            }
        }

        private static T m_Instance;
    }
}
