using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Զ����ɵ������ĵ�������
/// ��Ҫʱֱ����
/// �̳����ֵ���ģʽ��������һֱ�����ڳ�����
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseMonoSingletonAuto<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                //���ýű����ض���ű���
                obj.name = typeof(T).Name;
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();

            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        instance = this as T;
        DontDestroyOnLoad(instance);
    }
}
