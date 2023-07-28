using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ABURL",menuName = "ScriptableObject/AB������")]
public class ABURL : ScriptableObject
{
    [Tooltip("AB��������ļ���λ��")]
    public string CurrentABFilePath = Application.streamingAssetsPath + "/PC/";
    [Tooltip("�ϴ�AB����ַ")]
    public string UpABURL = "LoacalHost";
    [Tooltip("����AB����ַ")]
    public string DownLoadURL = "LoacalHost";
    
}
