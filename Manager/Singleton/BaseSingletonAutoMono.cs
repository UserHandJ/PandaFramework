using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Զ����ɵ������ĵ�������
/// ��Ҫʱֱ����
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseSingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
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
                //���������ģʽ���� ������ ���Ƴ�
                //��Ϊ ����ģʽ���� ���� �Ǵ��������������������е�
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();

            }
            return instance;
        }
    }
}
