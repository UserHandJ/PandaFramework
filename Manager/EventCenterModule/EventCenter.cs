using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{

}
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        this.actions += action;
    }
}

public class EventCenter : BaseSingleton<EventCenter>
{
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
    /// <summary>
    /// ����¼�
    /// </summary>
    /// <typeparam name="T">�¼��Ĳ�������</typeparam>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼�</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }
    /// <summary>
    /// ����¼�(�޲���)
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼�</param>
    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }
    /// <summary>
    /// �Ƴ���Ӧ���¼�����
    /// </summary>
    /// <typeparam name="T">�¼�����������</typeparam>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼�</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }
    /// <summary>
    /// �Ƴ��¼����޲�����
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼�</param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
    }
    /// <summary>
    /// �¼�����
    /// </summary>
    /// <typeparam name="T">�¼�����������</typeparam>
    /// <param name="name">�¼�����</param>
    /// <param name="info">�¼�����</param>
    public void EventTrigger<T>(string name, T info)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions?.Invoke(info);
        }
    }
    /// <summary>
    /// �¼�����������Ҫ�����ģ�
    /// </summary>
    /// <param name="name">�¼���</param>
    public void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions?.Invoke();
        }
    }
    /// <summary>
    /// ����¼����� ��Ҫ���� �����л�ʱ
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
