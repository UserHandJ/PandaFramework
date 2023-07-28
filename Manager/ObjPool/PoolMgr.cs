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
    public List<GameObject> poolList;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj">��Ҫ����Ķ���</param>
    /// <param name="poolObj">�����</param>
    public PoolData(GameObject obj, GameObject poolObj)
    {
        fatherObj = new GameObject(obj.name + "_F");
        fatherObj.transform.SetParent(poolObj.transform);
        poolList = new List<GameObject>();
        PushObj(obj);
    }

    /// <summary>
    /// ��������Ŷ���
    /// </summary>
    /// <param name="obj"></param>
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        obj.transform.SetParent(fatherObj.transform);
    }

    /// <summary>
    /// ��������ȡ������
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }
}

/// <summary>
/// �����ģ��
/// </summary>
public class PoolMgr : BaseSingleton<PoolMgr>
{
    //���������
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    //�����Obj
    private GameObject poolObj;
    /// <summary>
    /// ȡ����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public void GetObj(string name, UnityAction<GameObject> callback)
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
    /// <summary>
    /// ������طŶ���
    /// </summary>
    /// <param name="name">�����������������</param>
    /// <param name="obj"></param>
    public void PushObj(string name,GameObject obj)
    {
        if(poolObj == null) poolObj = new GameObject("Pool");
        //��������и�����
        if(poolDic.ContainsKey(name))
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
