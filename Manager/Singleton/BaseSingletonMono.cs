using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̳���MonoBehavior�ĵ�������
/// ���������Ƴ�,�õ�ʱ������
/// �������Ҳ�Լ��ֶ�������ص�������,
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj= new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
    public void OnDestroy()
    {
        instance = null;
    }
}
