using UnityEngine;

namespace QF
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if(Instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(obj);
                    m_Instance = obj.AddComponent<T>();
                }

                return m_Instance;
            }
        }

        private static T m_Instance = null;
    }
}
