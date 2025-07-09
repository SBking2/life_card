using QF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源加载模块，仅支持加载Resources内的文件
/// </summary>
public class ResMgr : Singleton<ResMgr>
{
    public T Load<T>(string path) where T : Object
    {
       return Resources.Load<T>(path);
    }
}
