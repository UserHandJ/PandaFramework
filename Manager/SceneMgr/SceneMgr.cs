using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// �����л�ģ��,
/// </summary>
public class SceneMgr : BaseSingleton<SceneMgr>
{
    /// <summary>
    /// ͬ��
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="Callback"></param>
    public void LoadScene(string sceneName, UnityAction Callback = null)
    {
        SceneManager.LoadScene(sceneName);
        Callback?.Invoke();
    }

    /// <summary>
    /// �첽
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="Callback"></param>
    public void LoadSceneAsyn(string sceneName, UnityAction Callback = null)
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadSceneAsyn(sceneName, Callback));
    }
    private IEnumerator ReallyLoadSceneAsyn(string sceneName, UnityAction Callback = null)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        while(!ao.isDone)
        {
            // �¼����� ����ַ� �������  �������þ���
            EventCenter.Instance.EventTrigger("SceneAsynLoadProgress", ao.progress);
            yield return ao.progress;
        }
        Callback?.Invoke();
    }
}
