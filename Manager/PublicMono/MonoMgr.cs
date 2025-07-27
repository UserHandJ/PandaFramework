using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono�Ĺ���
///1.����֡���»�ʱ���´����߼�
///2.����Эͬ�������߼�
///3.����ͳһִ�й���֡���»�ʱ��������߼�
/// </summary>
public class MonoMgr : BaseMonoSingletonAuto<MonoMgr>
{
    private event UnityAction updateEvent;
    private event UnityAction fixedUpdatteEvent;
    private event UnityAction lateUpdateEvent;
    

    // Update is called once per frame
    void Update()
    {
        updateEvent?.Invoke();
    }
    private void FixedUpdate()
    {
        fixedUpdatteEvent?.Invoke();
    }

    private void LateUpdate()
    {
        lateUpdateEvent?.Invoke();
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

    public void AddFixedUpdatteEventListener(UnityAction fun)
    {
        fixedUpdatteEvent += fun;
    }
    public void RemoveFixedUpdatteListener(UnityAction fun)
    {
        fixedUpdatteEvent -= fun;
    }

    public void AddLateUpdateEventListener(UnityAction fun)
    {
        lateUpdateEvent += fun;
    }
    public void RemoveLateUpdateEventListener(UnityAction fun)
    {
        lateUpdateEvent -= fun;
    }
   
}
