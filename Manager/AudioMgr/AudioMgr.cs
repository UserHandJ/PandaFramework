using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class AudioMgr : BaseSingleton<AudioMgr>
{
    //Ψһ�ı����������
    private AudioSource bkMusic = null;

    //���ִ�С
    private float bkValue = 1;

    //��Ч��������
    private GameObject soundObj = null;
    //��Ч�б�
    private List<AudioSource> soundList = new List<AudioSource>();
    //��Ч��С
    private float soundValue = 1;
    public AudioMgr()
    {
        MonoMgr.Instance.AddUpdateListener(Update);
    }
    private void Update()
    {
        for (int i = soundList.Count - 1; i >= 0; i++)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="AcPath">����·��</param>
    public void PlayBKMusic(string AcPath)
    {
        if (bkMusic == null)
        {
            GameObject obj = new GameObject("BKMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }
        //�첽���ر������� ������ɺ� ����
        ResMgr.Instance.LoadAsync<AudioClip>(AcPath, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkValue;
            bkMusic.Play();
        });
    }
    /// <summary>
    /// ��ͣ��������
    /// </summary>
    public void PauseBKMusic()
    {
        if (bkMusic == null) { return; }
        bkMusic.Pause();
    }
    /// <summary>
    /// ֹͣ��������
    /// </summary>
    public void StopBKMusic()
    {
        if (bkMusic == null) { return; }
        bkMusic.Stop();
    }
    /// <summary>
    /// �ı䱳������ ������С
    /// </summary>
    /// <param name="v">������С</param>
    public void ChangeBKValue(float v)
    {
        bkValue = v;
        if (bkMusic == null) { return; }
        bkMusic.volume = bkValue;
    }
    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="AcPath">·��</param>
    /// <param name="isLoop">�Ƿ�ѭ��</param>
    /// <param name="callBack">���ؽ����Ļص�����������Ч���</param>
    public void PlaySound(string AcPath, bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject("Sound");
        }
        //����Ч��Դ�첽���ؽ����� �����һ����Ч
        ResMgr.Instance.LoadAsync<AudioClip>(AcPath, (clip) =>
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();

            soundList.Add(source);
            callBack?.Invoke(source);
        });
    }
    /// <summary>
    /// �ı���Ч������С
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        for (int i = 0; i < soundList.Count; ++i)
            soundList[i].volume = value;
    }
    /// <summary>
    /// ֹͣ��Ч
    /// </summary>
    /// <param name="source"></param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }
}
