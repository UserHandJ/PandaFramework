using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ������е������࣬�Ѷ��󶼷ֺ��࣬���㿴
/// </summary>
public class PoolData
{
    public GameObject fatherObj;//�����ĸ�����
    public Stack<GameObject> poolList;//����

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="obj">��Ҫ����Ķ���</param>
    /// <param name="poolObj">�����</param>
    public PoolData(GameObject obj, GameObject poolObj)
    {
        if (PoolMgr.isOpenLayout && poolObj != null)
        {
            fatherObj = new GameObject(obj.name + "_F");
            fatherObj.transform.SetParent(poolObj.transform);
        }
        poolList = new Stack<GameObject>();
        PushObj(obj);
    }

    /// <summary>
    /// ��������Ŷ���
    /// </summary>
    /// <param name="obj"></param>
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Push(obj);
        if (PoolMgr.isOpenLayout)
            obj.transform.SetParent(fatherObj.transform);
    }

    /// <summary>
    /// ��������ȡ������
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList.Pop();
        obj.SetActive(true);
        if (PoolMgr.isOpenLayout)
            obj.transform.parent = null;
        return obj;
    }
}

/// <summary>
/// �����ģ��
/// </summary>
public class PoolMgr : BaseSingleton<PoolMgr>
{
    /// <summary>
    /// ���������
    /// </summary>
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    /// <summary>
    /// �����Obj
    /// </summary>
    private GameObject poolObj;
    /// <summary>
    /// ������Hierarchy�зŻض���غ��Ƿ񰴲㼶�ṹ���
    /// ��������ʱ����Ϊfalse�����Խ�Լһ������
    /// </summary>
    public static bool isOpenLayout = true;

    /// <summary>
    /// �첽ȡ����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public void GetObjAsync(string name, UnityAction<GameObject> callback)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            callback(poolDic[name].GetObj());
        }
        else
        {
            //ͨ���첽������Դ ����������ⲿ��
            //����Ҳ���Ըĳ���AB������
            ResMgr.Instance.LoadAsync<GameObject>(name, (obj) =>
            {
                obj.name = name;
                callback(obj);
            });
        }
    }

    public GameObject GetObj(string name)
    {
        GameObject obj = null;
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            obj = poolDic[name].GetObj();
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
        }
        if (obj == null)
        {
            Debug.LogError($"Resources��û��{name}");
        }
        return obj;
    }

    /// <summary>
    /// ������طŶ���
    /// </summary>
    /// <param name="name">�����������������</param>
    /// <param name="obj"></param>
    public void PushObj(string name, GameObject obj)
    {
        if (poolObj == null && isOpenLayout) poolObj = new GameObject("Pool");
        //��������и�����
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        //�������û���и�����
        else
        {
            poolDic.Add(name, new PoolData(obj, poolObj));
        }
    }

    /// <summary>
    /// ��ջ���صķ��� 
    /// ��Ҫ���� �����л�ʱ
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
