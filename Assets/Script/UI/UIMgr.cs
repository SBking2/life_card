using QF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : Singleton<UIMgr>
{
    private Dictionary<Type, IUIBase> m_UIDic = new Dictionary<Type, IUIBase>();
    public void RegisterUI<T>(IUIBase ui)
    {
        if(!m_UIDic.ContainsKey(typeof(T)))
        {
            m_UIDic.Add(typeof(T), ui);
            return;
        }

        Debug.LogError(string.Format("UI {0} have registered!", typeof(T).Name));
    }
    public void RegisterUI(Type type, IUIBase ui)
    {
        if (!m_UIDic.ContainsKey(type))
        {
            m_UIDic.Add(type, ui);
            return;
        }

        Debug.LogError(string.Format("UI {0} have registered!", type.Name));
    }

    public void UnRegisterUI<T>()
    {
        if(m_UIDic.ContainsKey(typeof(T)))
            m_UIDic.Remove(typeof(T));
    }

    public void ShowUI<T>()
    {
        if (m_UIDic.ContainsKey(typeof(T)))
            m_UIDic[typeof(T)].ShowMe();
    }

    public void HideUI<T>()
    {
        if (m_UIDic.ContainsKey(typeof(T)))
            m_UIDic[typeof(T)].HideMe();
    }
}
