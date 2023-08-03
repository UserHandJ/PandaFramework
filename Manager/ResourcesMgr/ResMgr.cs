using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Resources���ط���
/// </summary>
public class ResMgr : BaseSingletonAutoMono<ResMgr>
{
    //ͬ��������Դ
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        T res = Resources.Load<T>(path);
        //���������һ��GameObject���͵� �Ұ���ʵ������ �ٷ��س�ȥ �ⲿ ֱ��ʹ�ü���
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;
    }
    /// <summary>
    /// Typeͬ��������Դ
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public UnityEngine.Object Load(string path, System.Type type)
    {
        UnityEngine.Object res = Resources.Load(path, type);
        if(res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;
    }
    //�첽������Դ
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : UnityEngine.Object
    {
        StartCoroutine(ReallyLoadAsync(name, callback));
    }

    private IEnumerator ReallyLoadAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(path);
        yield return r;
        if (r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }
    /// <summary>
    /// Type�첽������Դ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public void LoadAsync(string name,UnityAction<UnityEngine.Object> callback)
    {
        StartCoroutine(ReallyTypeLoadAsync(name, callback));
    }
    private IEnumerator ReallyTypeLoadAsync(string path, UnityAction<UnityEngine.Object> callback)
    {
        ResourceRequest r = Resources.LoadAsync(path);
        yield return r;
        if (r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset));
        else
            callback(r.asset);
    }
}
