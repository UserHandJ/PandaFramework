using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DataConfig 
{
    //[Tooltip("AB��������ļ���λ��")]
    public static string CurrentABFilePath = Application.dataPath + "/AssetBundle/PC/";
    //[Tooltip("�ϴ�AB����ַ")]
    public static string UpABURL = "ftp://192.168.31.48/AB/PC/";
    //[Tooltip("����AB����ַ")]
    public static string DownLoadURL = "ftp://192.168.31.48/AB/PC/";
    //FTPͨ��ƾ֤
    //ftp�û���
    public static string Ftp_UserName = "Admin";
    //ftp����
    public static string Ftp_Password = "Admin123";
}
