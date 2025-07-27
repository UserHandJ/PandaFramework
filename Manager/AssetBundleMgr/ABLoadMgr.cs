using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// AB�����ع�����
/// </summary>
public class ABLoadMgr : BaseMonoSingletonAuto<ABLoadMgr>
{
    //����
    private AssetBundle mainAB = null;
    //����������ȡ�����ļ�
    private AssetBundleManifest manifest = null;

    //ѡ��洢 AB��������
    //AB�����ܹ��ظ����� ����ᱨ��
    //�ֵ������洢 AB������
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// ��ȡAB������·��
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }
    /// <summary>
    /// ������ ����ƽ̨��ͬ ������ͬ
    /// </summary>
    private string MainName
    {
        get
        {
#if UNITY_ANDROID
            return "Android";
#endif
            return "PC";
        }
    }

    /// <summary>
    /// �������� �� �����ļ�
    /// ��Ϊ�������а��� ͨ�������ܵõ�������Ϣ
    /// </summary>
    private void LoadMainAB()
    {
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }

    /// <summary>
    /// ����ָ������������
    /// </summary>
    /// <param name="abName"></param>
    private void LoadDependencies(string abName)
    {
        //��������
        LoadMainAB();
        //��ȡ������
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
    }

    #region ͬ������ (����֧��ͬ������)
    ///// <summary>
    ///// ������Դͬ������
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="abName">ab����</param>
    ///// <param name="resName">��Դ��</param>
    ///// <returns></returns>
    //public T LoadRes<T>(string abName, string resName) where T : Object
    //{
    //    //����������
    //    LoadDependencies(abName);
    //    //����Ŀ���
    //    if (!abDic.ContainsKey(abName))
    //    {
    //        AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
    //        abDic.Add(abName, ab);
    //    }
    //    //�õ����س�������Դ
    //    T obj = abDic[abName].LoadAsset<T>(resName);
    //    //�����GameObject ��ΪGameObject 100%������Ҫʵ������
    //    //����ֱ��ʵ����
    //    if (obj is GameObject)
    //        return Instantiate(obj);
    //    else
    //        return obj;
    //}
    ///// <summary>
    /////  Typeͬ������ָ����Դ
    /////  �����Ҫ�Ǹ�Lua�ű�����
    ///// </summary>
    ///// <param name="abName"></param>
    ///// <param name="resName"></param>
    ///// <param name="type"></param>
    ///// <returns></returns>
    //public Object LoadRes(string abName, string resName, System.Type type)
    //{
    //    //����������
    //    LoadDependencies(abName);
    //    //����Ŀ���
    //    if (!abDic.ContainsKey(abName))
    //    {
    //        AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
    //        abDic.Add(abName, ab);
    //    }
    //    //�õ����س�������Դ
    //    Object obj = abDic[abName].LoadAsset(resName, type);
    //    if (obj is GameObject)
    //        return Instantiate(obj);
    //    else
    //        return obj;

    //}
    ///// <summary>
    ///// ���� ͬ������ָ����Դ
    ///// </summary>
    ///// <param name="abName"></param>
    ///// <param name="resName"></param>
    ///// <returns></returns>
    //public Object LoadRes(string abName, string resName)
    //{
    //    //����������
    //    LoadDependencies(abName);
    //    //����Ŀ���
    //    if (!abDic.ContainsKey(abName))
    //    {
    //        AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
    //        abDic.Add(abName, ab);
    //    }

    //    //�õ����س�������Դ
    //    Object obj = abDic[abName].LoadAsset(resName);
    //    //�����GameObject ��ΪGameObject 100%������Ҫʵ������
    //    //��������ֱ��ʵ����
    //    if (obj is GameObject)
    //        return Instantiate(obj);
    //    else
    //        return obj;
    //}
    #endregion


    #region �첽����
    /// <summary>
    /// �����첽������Դ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
    }
    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        //����������
        //��������
        LoadMainAB();
        //��ȡ������
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                abDic.Add(strs[i], null);
                //AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                //abDic.Add(strs[i], ab);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                yield return req;

                abDic[strs[i]] = req.assetBundle;
            }
            else
            {
                while (abDic[strs[i]] == null)//���Ϊnull˵����Դ�����첽������
                {
                    yield return 0;
                }
            }
        }


        //����Ŀ���
        if (!abDic.ContainsKey(abName))
        {
            abDic.Add(abName, null);
            //AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + abName);
            yield return req;

            abDic[abName] = req.assetBundle;
        }
        else
        {
            while (abDic[abName] == null)
            {
                yield return 0;
            }
        }
        AssetBundleRequest abq = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abq;
        callBack(abq.asset as T);
    }
    /// <summary>
    /// Type�첽������Դ
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack));
    }
    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        //����������
        //��������
        LoadMainAB();
        //��ȡ������
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                abDic.Add(strs[i], null);
                //AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                //abDic.Add(strs[i], ab);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                yield return req;

                abDic[strs[i]] = req.assetBundle;
            }
            else
            {
                while (abDic[strs[i]] == null)//���Ϊnull˵����Դ�����첽������
                {
                    yield return 0;
                }
            }
        }
        //����Ŀ���
        if (!abDic.ContainsKey(abName))
        {
            abDic.Add(abName, null);
            //AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + abName);
            yield return req;

            abDic[abName] = req.assetBundle;
        }
        else
        {
            while (abDic[abName] == null)
            {
                yield return 0;
            }
        }
        //�첽���ذ�����Դ
        AssetBundleRequest abq = abDic[abName].LoadAssetAsync(resName, type);
        yield return abq;
        callBack(abq.asset);
    }
    /// <summary>
    /// ���� �첽���� ָ����Դ
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        //����������
        //��������
        LoadMainAB();
        //��ȡ������
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                abDic.Add(strs[i], null);
                //AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                //abDic.Add(strs[i], ab);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                yield return req;

                abDic[strs[i]] = req.assetBundle;
            }
            else
            {
                while (abDic[strs[i]] == null)//���Ϊnull˵����Դ�����첽������
                {
                    yield return 0;
                }
            }
        }
        //����Ŀ���
        if (!abDic.ContainsKey(abName))
        {
            abDic.Add(abName, null);
            //AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + abName);
            yield return req;

            abDic[abName] = req.assetBundle;
        }
        else
        {
            while (abDic[abName] == null)
            {
                yield return 0;
            }
        }
        //�첽���ذ�����Դ
        AssetBundleRequest abq = abDic[abName].LoadAssetAsync(resName);
        yield return abq;
        callBack(abq.asset);
    }
    #endregion

    /// <summary>
    /// ж��AB���ķ���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callbackResult"></param>
    public void UnLoadAB(string name, UnityAction<bool> callbackResult = null)
    {
        if (abDic.ContainsKey(name))
        {
            if (abDic[name] == null)
            {
                Debug.Log("����Դ�������첽�����У��޷�ж�أ�");
                callbackResult?.Invoke(false);
                return;
            }
            abDic[name].Unload(false);
            abDic.Remove(name);
            callbackResult?.Invoke(true);
        }
    }
    /// <summary>
    /// ���AB���ķ���
    /// </summary>
    public void ClearAB()
    {
        StopAllCoroutines();
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        //ж������
        mainAB = null;
    }
}
