using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �¼�����
/// </summary>
public interface I_PD_EventInfo { }
public class PD_EventInfo<T> : I_PD_EventInfo
{
    public UnityAction<T> actions;
    public PD_EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}
public class PD_EventInfo : I_PD_EventInfo
{
    public UnityAction actions;
    public PD_EventInfo(UnityAction action)
    {
        this.actions += action;
    }
}
/// <summary>
/// �¼�����
/// </summary>
public class EventCenter : BaseSingleton<EventCenter>
{
    private Dictionary<EEventDefine, I_PD_EventInfo> eventDic = new Dictionary<EEventDefine, I_PD_EventInfo>();
    /// <summary>
    /// ����¼�
    /// </summary>
    /// <typeparam name="T">�¼��Ĳ�������</typeparam>
    /// <param name="eventDefine">�¼�ö��</param>
    /// <param name="action">�¼�</param>
    public void AddEventListener<T>(EEventDefine eventDefine, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(eventDefine))
        {
            (eventDic[eventDefine] as PD_EventInfo<T>).actions += action;
        }
        else
        {
            eventDic.Add(eventDefine, new PD_EventInfo<T>(action));
        }
    }
    /// <summary>
    /// ����¼�(�޲���)
    /// </summary>
    /// <param name="eventDefine">�¼�ö��</param>
    /// <param name="action">�¼�</param>
    public void AddEventListener(EEventDefine eventDefine, UnityAction action)
    {
        if (eventDic.ContainsKey(eventDefine))
        {
            (eventDic[eventDefine] as PD_EventInfo).actions += action;
        }
        else
        {
            eventDic.Add(eventDefine, new PD_EventInfo(action));
        }
    }
    /// <summary>
    /// �Ƴ���Ӧ���¼�����
    /// </summary>
    /// <typeparam name="T">�¼�����������</typeparam>
    /// <param name="eventDefine">�¼�ö��</param>
    /// <param name="action">�¼�</param>
    public void RemoveEventListener<T>(EEventDefine eventDefine, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(eventDefine))
        {
            (eventDic[eventDefine] as PD_EventInfo<T>).actions -= action;
        }
    }
    /// <summary>
    /// �Ƴ��¼����޲�����
    /// </summary>
    /// <param name="eventDefine">�¼�ö��</param>
    /// <param name="action">�¼�</param>
    public void RemoveEventListener(EEventDefine eventDefine, UnityAction action)
    {
        if (eventDic.ContainsKey(eventDefine))
        {
            (eventDic[eventDefine] as PD_EventInfo).actions -= action;
        }
    }
    /// <summary>
    /// �¼�����
    /// </summary>
    /// <typeparam name="T">�¼�����������</typeparam>
    /// <param name="eventDefine">�¼�ö��</param>
    /// <param name="info">�¼�����</param>
    public void EventTrigger<T>(EEventDefine eventDefine, T info)
    {
        if (eventDic.ContainsKey(eventDefine))
        {
            (eventDic[eventDefine] as PD_EventInfo<T>).actions?.Invoke(info);
        }
    }
    /// <summary>
    /// �¼�����������Ҫ�����ģ�
    /// </summary>
    /// <param name="eventDefine">�¼�ö��</param>
    public void EventTrigger(EEventDefine eventDefine)
    {
        if (eventDic.ContainsKey(eventDefine))
        {
            (eventDic[eventDefine] as PD_EventInfo).actions?.Invoke();
        }
    }
    /// <summary>
    /// ����¼����� 
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
    /// <summary>
    /// ���ָ�����¼��ļ���
    /// </summary>
    /// <param name="eventDefine">�¼�ö��</param>
    /// <returns></returns>
    public bool ClearOneEvent(EEventDefine eventDefine)
    {
        if (eventDic.ContainsKey(eventDefine))
        {
            eventDic.Remove(eventDefine);
            return true;
        }
        else
        {
            return false;
        }

    }
}
