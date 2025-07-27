
using System;
using UnityEngine;

/// <summary>
/// ��־ϵͳ����
/// </summary>
[CreateAssetMenu(fileName = "��־ϵͳ����", menuName = "UPanda/Create DebugConfig", order = 0)]
public class LogConfig : ScriptableObject
{
    [Header("�Ƿ����־ϵͳ")]
    public bool openLog = true;
    [Header("��־ǰ׺")]
    public string logHeadFix = "###";
    [Header("�Ƿ���ʾʱ��")]
    public bool openTime = true;
    [Header("��ʾ�߳�id")]
    public bool showThreadID = true;
    [Header("��־�ļ����濪��")]
    public bool logSave = false;
    [Header("�Ƿ���ʾFPS")]
    public bool showFPS = true;

    [Header("�ļ�����·��")]
    public string logFileSavePath { get { return Application.persistentDataPath + "/Output Log/"; } }
    [Header("��־�ļ�����")]
    public string logFileName { get { return Application.productName + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".log"; } }

}
