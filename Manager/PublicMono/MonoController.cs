using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono�Ĺ�����
/// 1.�������ں���
/// 2.�¼� 
/// 3.Э��
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        updateEvent?.Invoke();
    }
    /// <summary>
    /// �ṩ���ⲿ���֡�����¼��ķ���
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }
    /// <summary>
    /// �ṩ���ⲿ �����Ƴ�֡�����¼�����
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
