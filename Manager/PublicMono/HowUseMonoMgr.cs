using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ʹ�÷�ʽ
/// </summary>
public class HowUseMonoMgr
{
    /// <summary>
    /// ���ø÷����Ϳ�����MyUpdate()������ÿ֡����
    /// </summary>
    public void EnableMyUpdate()
    {
        MonoMgr.Instance.AddUpdateListener(MyUpdate);
    }
    public void DisableMyUpdate()
    {
        MonoMgr.Instance.RemoveUpdateListener(MyUpdate);
    }

    private void MyUpdate()
    {
        Debug.Log("����");
    }
    /// <summary>
    /// ����Э�̵ķ�ʽ
    /// </summary>
    public void StartMyIEnumerator()
    {
        MonoMgr.Instance.StartCoroutine(SelfIEnumerator());
    }

    private IEnumerator SelfIEnumerator()
    {
        Debug.Log("����Э��");
        yield return new WaitForSeconds(3f);
        Debug.Log("Э�̽���");
    }
}
