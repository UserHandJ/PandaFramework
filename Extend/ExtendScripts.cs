using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ű���չ
/// </summary>
public static class ExtendScripts
{
    /// <summary>
    /// ��Transform��ӵ���չ��������out�������������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trs"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Transform GetOrAddComponent<T>(this Transform trs,out T t) where T : Component
    {
        t = trs.GetComponent<T>();
        if(t == null) t = trs.gameObject.AddComponent<T>();
        return trs.transform;
    }
}
