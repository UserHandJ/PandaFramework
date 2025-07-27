using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��Դ��Ϣ����
/// </summary>
public abstract class ResInfoBase
{
    /// <summary>
    /// ���ü���
    /// </summary>
    public int refCount;
}
/// <summary>
/// ��Դ��Ϣ
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResInfo<T> : ResInfoBase
{
    /// <summary>
    /// ��Դ
    /// </summary>
    public T asset;
    /// <summary>
    /// �첽������ɵĻص�
    /// </summary>
    public UnityAction<T> callback;
    /// <summary>
    /// �洢�첽ʱ��Эͬ����
    /// </summary>
    public Coroutine coroutine;
    /// <summary>
    /// ���ü���Ϊ0������Դ�Ƿ��Ƴ�
    /// </summary>
    public bool isDel;
    public void AddRefCount()
    {
        ++refCount;
    }

    public void SubRefCount()
    {
        --refCount;
        if (refCount < 0) Debug.Log($"{asset}:����Դ���ü���С��0�������м����غ�ж�ص�ִ�д���");
    }


}


/// <summary>
/// Resources������Դ
/// </summary>
public class ResMgr : BaseMonoSingletonAuto<ResMgr>
{
    /// <summary>
    /// �洢 ���ع� �� ������ ����Դ
    /// key����Դ�� ��·��+��Դ���ͣ�ƴ��
    /// </summary>
    private Dictionary<string, ResInfoBase> resDic = new Dictionary<string, ResInfoBase>();
    /// <summary>
    /// ͬ��������Դ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        string resName = path + "_" + typeof(T).Name;
        ResInfo<T> info;
        if (!resDic.ContainsKey(resName))
        {
            T res = Resources.Load<T>(path);
            info = new ResInfo<T>();
            info.asset = res;
            info.AddRefCount();
            resDic.Add(resName, info);
            return res;
        }
        else//����ֵ����������Դ����Ҫ�ж��Ƿ��ڼ����е�״̬
        {
            info = resDic[resName] as ResInfo<T>;
            info.AddRefCount();
            if (info.asset == null)
            {
                //�ص������첽������Դ״̬��Э�̣�ֱ����ͬ���ķ�ʽ����
                StopCoroutine(info.coroutine);
                T res = Resources.Load<T>(path);
                info.asset = res;
                info.callback?.Invoke(res);
                info.callback = null;
                info.coroutine = null;
                return res;//�����ԴʱGameObject������Ҫ�Լ���ʵ����
            }
            else
            {
                return info.asset;
            }
        }
        
    }
    /// <summary>
    /// Typeͬ��������Դ
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [Obsolete("\n����!\n��ֹ��Load<T>��ʽ���ã�������Դ���ر�����ͬһ�ּ��ط�ʽ")]
    public UnityEngine.Object Load(string path, System.Type type)
    {
        string resName = path + "_" + type.Name;
        ResInfo<UnityEngine.Object> info;
        if (!resDic.ContainsKey(resName))
        {
            UnityEngine.Object res = Resources.Load(path, type);
            info = new ResInfo<UnityEngine.Object>();
            info.asset = res;
            info.AddRefCount();
            resDic.Add(resName, info);
            return res;
        }
        else//����ֵ����������Դ����Ҫ�ж��Ƿ��ڼ����е�״̬
        {
            info = resDic[resName] as ResInfo<UnityEngine.Object>;
            info.AddRefCount();
            if (info.asset == null)
            {
                //�ص������첽������Դ״̬��Э�̣�ֱ����ͬ���ķ�ʽ����
                StopCoroutine(info.coroutine);
                UnityEngine.Object res = Resources.Load(path, type);
                info.asset = res;
                info.callback?.Invoke(res);
                info.callback = null;
                info.coroutine = null;
                return res;
            }
            else
            {
                return info.asset;
            }
        }
    }
    /// <summary>
    /// �첽������Դ
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">��Դ·��</param>
    /// <param name="callback">�ص�</param>
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
    {
        string resName = path + "_" + typeof(T).Name;
        ResInfo<T> info;
        if (!resDic.ContainsKey(resName))//����ֵ��ﲻ��������Դ
        {
            info = new ResInfo<T>();
            info.AddRefCount();
            resDic.Add(resName, info);

            info.callback += callback;
            info.coroutine = StartCoroutine(ReallyLoadAsync<T>(path));
        }
        else
        {
            info = resDic[resName] as ResInfo<T>;
            info.AddRefCount();
            //����null ˵����Դ��û�м����� �����첽������
            if (info.asset == null)
            {
                info.callback += callback;
            }
            else//������ɵ���Դ
            {
                callback?.Invoke(info.asset);
            }
        }
    }

    private IEnumerator ReallyLoadAsync<T>(string path) where T : UnityEngine.Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(path);
        yield return r;
        string resName = path + "_" + typeof(T).Name;
        if (resDic.ContainsKey(resName))
        {
            ResInfo<T> resInfo = resDic[resName] as ResInfo<T>;
            //��¼������ɵ���Դ
            resInfo.asset = r.asset as T;

            if (resInfo.refCount == 0)//�������Ϊ0��ɾ��
            {
                UnLoadAsset_NoSubAssetsCount<T>(path, resInfo.isDel);
            }
            else
            {
                resInfo.callback?.Invoke(resInfo.asset);//ͨ��ί�аѼ��غõ���Դ����ȥ

                //��Դ����ȥ��ѻص���Э���ÿ� ��������ռ��
                resInfo.callback = null;
                resInfo.coroutine = null;
            }
        }
        //if (r.asset is GameObject)
        //    callback(GameObject.Instantiate(r.asset) as T);
        //else
        //    callback(r.asset as T);
    }
    /// <summary>
    /// ��ֹ��LoadAsync<T>��ʽ���á�
    /// ������Դ���ر���ͬ����ط�ʽ
    /// </summary>
    /// <param name="path">��Դ·��</param>
    /// <param name="type">���ͣ���typeof(class)�õ�</param>
    /// <param name="callback">�ص� </param>
    [Obsolete("\n����!\n��ֹ��LoadAsync<T>��ʽ���ã�������Դ���ر�����ͬһ�ּ��ط�ʽ")]
    public void LoadAsync(string path, Type type, UnityAction<UnityEngine.Object> callback)
    {
        string resName = path + "_" + type.Name;
        ResInfo<UnityEngine.Object> info;
        if (!resDic.ContainsKey(resName))//����ֵ��ﲻ��������Դ
        {
            info = new ResInfo<UnityEngine.Object>();
            info.AddRefCount();
            resDic.Add(resName, info);

            info.callback += callback;
            info.coroutine = StartCoroutine(ReallyTypeLoadAsync(path, type));
        }
        else
        {
            info = resDic[resName] as ResInfo<UnityEngine.Object>;
            info.AddRefCount();
            //����null ˵����Դ��û�м����� �����첽������
            if (info.asset == null)
            {
                info.callback += callback;
            }
            else//������ɵ���Դ
            {
                callback?.Invoke(info.asset);
            }
        }
        //
    }
    private IEnumerator ReallyTypeLoadAsync(string path, Type type)
    {
        ResourceRequest r = Resources.LoadAsync(path, type);
        yield return r;
        string resName = path + "_" + type.Name;
        if (resDic.ContainsKey(resName))
        {
            ResInfo<UnityEngine.Object> resInfo = resDic[resName] as ResInfo<UnityEngine.Object>;
            //��¼������ɵ���Դ
            resInfo.asset = r.asset;

            if (resInfo.refCount == 0)//�������Ϊ0��ɾ��
            {
                UnLoadAsset_NoSubAssetsCount(path, type, resInfo.isDel);
            }
            else
            {
                resInfo.callback?.Invoke(resInfo.asset);//ͨ��ί�аѼ��غõ���Դ����ȥ
                //��Դ����ȥ��ѻص���Э���ÿ� ��������ռ��
                resInfo.callback = null;
                resInfo.coroutine = null;
            }

        }
        //if (r.asset is GameObject)
        //    callback(GameObject.Instantiate(r.asset));
        //else
        //    callback(r.asset);
    }
    /// <summary>
    /// ж��ָ����Դ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="isDel">����Ϊ0ʱ���Ƿ��Ƴ�����Դ</param>
    /// <param name="callBack"></param>
    public void UnLoadAsset<T>(string path, bool isDel = false, UnityAction<T> callBack = null)
    {
        string resName = path + "_" + typeof(T).Name;
        if (resDic.ContainsKey(resName))
        {
            ResInfo<T> resInfo = resDic[resName] as ResInfo<T>;
            resInfo.SubRefCount();
            resInfo.isDel = isDel;
            if (resInfo.asset != null && resInfo.refCount == 0 && resInfo.isDel)//��Դ�Ѽ��ؽ�����״̬
            {
                resDic.Remove(resName);
                Resources.UnloadAsset(resInfo.asset as UnityEngine.Object);
            }
            else if (resInfo.asset == null)//��Դ�첽�����е�״̬
            {
                //�Ѵ��Ƴ�״̬��Ϊtrue,���첽��������ٿ�ʼж��
                //resInfo.isDel = true;

                if (callBack != null)
                    resInfo.callback -= callBack;
            }
        }
    }
    /// <summary>
    /// ResMgr˽�з������÷������ò�������Դ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="isDel">����Ϊ0ʱ���Ƿ��Ƴ�����Դ</param>
    /// <param name="callBack"></param>
    private void UnLoadAsset_NoSubAssetsCount<T>(string path, bool isDel = false, UnityAction<T> callBack = null)
    {
        string resName = path + "_" + typeof(T).Name;
        if (resDic.ContainsKey(resName))
        {
            ResInfo<T> resInfo = resDic[resName] as ResInfo<T>;
            //resInfo.SubRefCount();
            resInfo.isDel = isDel;
            if (resInfo.asset != null && resInfo.refCount == 0 && resInfo.isDel)//��Դ�Ѽ��ؽ�����״̬
            {
                resDic.Remove(resName);
                Resources.UnloadAsset(resInfo.asset as UnityEngine.Object);
            }
            else if (resInfo.asset == null)//��Դ�첽�����е�״̬
            {
                //�Ѵ��Ƴ�״̬��Ϊtrue,���첽��������ٿ�ʼж��
                //resInfo.isDel = true;

                if (callBack != null)
                    resInfo.callback -= callBack;
            }
        }
    }

    /// <summary>
    /// ж��ָ����Դ (type)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <param name="isDel">����Ϊ0ʱ���Ƿ��Ƴ�����Դ</param>
    /// <param name="callBack"></param>
    public void UnLoadAsset(string path, Type type, bool isDel = false, UnityAction<UnityEngine.Object> callBack = null)
    {
        string resName = path + "_" + type.Name;
        if (resDic.ContainsKey(resName))
        {
            ResInfo<UnityEngine.Object> resInfo = resDic[resName] as ResInfo<UnityEngine.Object>;
            resInfo.SubRefCount();
            resInfo.isDel = isDel;
            if (resInfo.asset != null && resInfo.refCount == 0)//��Դ�Ѽ��ؽ�����״̬
            {
                resDic.Remove(resName);
                Resources.UnloadAsset(resInfo.asset);
            }
            else if (resInfo.asset == null)//��Դ�첽�����е�״̬
            {
                //resInfo.isDel = true;
                if (callBack != null)
                    resInfo.callback -= callBack;
            }
        }
    }
    /// <summary>
    ///  ResMgr˽�з������÷������ò�������Դ����
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <param name="isDel">����Ϊ0ʱ���Ƿ��Ƴ�����Դ</param>
    /// <param name="callBack"></param>
    private void UnLoadAsset_NoSubAssetsCount(string path, Type type, bool isDel = false, UnityAction<UnityEngine.Object> callBack = null)
    {
        string resName = path + "_" + type.Name;
        if (resDic.ContainsKey(resName))
        {
            ResInfo<UnityEngine.Object> resInfo = resDic[resName] as ResInfo<UnityEngine.Object>;
            //resInfo.SubRefCount();
            resInfo.isDel = isDel;
            if (resInfo.asset != null && resInfo.refCount == 0)//��Դ�Ѽ��ؽ�����״̬
            {
                resDic.Remove(resName);
                Resources.UnloadAsset(resInfo.asset);
            }
            else if (resInfo.asset == null)//��Դ�첽�����е�״̬
            {
                //resInfo.isDel = true;
                if (callBack != null)
                    resInfo.callback -= callBack;
            }
        }
    }

    /// <summary>
    /// ж��û��ʹ�õ���Դ
    /// </summary>
    /// <param name="callback"></param>
    public void UnLoadUnUseAssets(UnityAction callback = null)
    {
        StartCoroutine(IE_UnLoadUnUseAssets(callback));
    }
    private IEnumerator IE_UnLoadUnUseAssets(UnityAction callback = null)
    {
        List<string> tempList = new List<string>();
        foreach (string path in resDic.Keys)
        {
            if (resDic[path].refCount == 0)
            {
                tempList.Add(path);
            }
        }
        foreach (string path in tempList)
        {
            resDic.Remove(path);
        }

        AsyncOperation ao = Resources.UnloadUnusedAssets();
        yield return ao;
        callback?.Invoke();
    }
    /// <summary>
    /// ��ȡ��Դ�����ü���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public int GetAssetsRefCount<T>(string path)
    {
        string resName = path + "_" + typeof(T).Name;
        if (resDic.ContainsKey(resName))
        {
            return (resDic[resName] as ResInfo<T>).refCount;
        }
        return 0;
    }
    /// <summary>
    /// ���������Դ��¼����ж��û��ʹ�õ���Դ
    /// </summary>
    /// <param name="callBack"></param>
    public void ClearDic(UnityAction callBack = null)
    {
        resDic.Clear();
        StartCoroutine(ReallyClearDic(callBack));
    }

    private IEnumerator ReallyClearDic(UnityAction callBack)
    {
        AsyncOperation ao = Resources.UnloadUnusedAssets();
        yield return ao;
        callBack?.Invoke();
    }
}
