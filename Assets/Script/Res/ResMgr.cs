using QF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Դ����ģ�飬��֧�ּ���Resources�ڵ��ļ�
/// </summary>
public class ResMgr : Singleton<ResMgr>
{
    public T Load<T>(string path) where T : Object
    {
       return Resources.Load<T>(path);
    }
}
