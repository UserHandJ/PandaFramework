using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreatDirectory
{
    private static string PATH_3rd = Application.dataPath + "/3rd";
    private static string PATH_ArtAssets = Application.dataPath + "/ArtAssets";
    private static string PATH_Resources = Application.dataPath + "/Resources";
    private static string PATH_Scenes = Application.dataPath + "/Scenes";
    private static string PATH_StreamingAssets = Application.dataPath + "/StreamingAssets";
    private static string PATH_Scripts = Application.dataPath + "/Scripts";
    private static string PATH_AssetBundle = Application.dataPath + "/AssetBundle";



    [MenuItem("Tools/����Ŀ¼�ļ���")]
    private static void CreatForder()
    {
        #region ��
        //if (!Directory.Exists(PATH_3rd))
        //{
        //    Directory.CreateDirectory(PATH_3rd);
        //    Debug.Log("�������������ԴĿ¼3rd�������");
        //}
        //else
        //{
        //    Debug.Log("3rd�ļ�Ŀ¼�Ѵ���");
        //}
        //if (!Directory.Exists(PATH_ArtAssets))
        //{
        //    Directory.CreateDirectory(PATH_ArtAssets);
        //    Debug.Log("������ԴĿ¼ArtAssets�������");
        //}
        //else
        //{
        //    Debug.Log("ArtAssets�ļ�Ŀ¼�Ѵ���");
        //}

        //if (!Directory.Exists(PATH_Resources))
        //{
        //    Directory.CreateDirectory(PATH_Resources);
        //    Debug.Log("ResourcesĿ¼�������");
        //}
        //else
        //{
        //    Debug.Log("Resources�ļ�Ŀ¼�Ѵ���");
        //}
        //if (!Directory.Exists(PATH_Scenes))
        //{
        //    Directory.CreateDirectory(PATH_Scenes);
        //    Debug.Log("SceneĿ¼�������");
        //}
        //else
        //{
        //    Debug.Log("Scenes�ļ�Ŀ¼�Ѵ���");
        //}
        //if (!Directory.Exists(PATH_StreamingAssets))
        //{
        //    Directory.CreateDirectory(PATH_StreamingAssets);
        //    Debug.Log("StreamingAssetsĿ¼�������");
        //}
        //else
        //{
        //    Debug.Log("StreamingAssets�ļ�Ŀ¼�Ѵ���");
        //}
        //if (!Directory.Exists(PATH_Scripts))
        //{
        //    Directory.CreateDirectory(PATH_Scripts);
        //    Debug.Log("�ű�Ŀ¼�������");
        //}
        //else
        //{
        //    Debug.Log("Scripts�ļ�Ŀ¼�Ѵ���");
        //}
        //if (!Directory.Exists(PATH_AssetBundle))
        //{
        //    Directory.CreateDirectory(PATH_AssetBundle);
        //    Debug.Log("AssetBundleĿ¼�������");
        //}
        //else
        //{
        //    Debug.Log("AssetBundle�ļ�Ŀ¼�Ѵ���");
        //}

        #endregion
        CreatForderPacking(PATH_3rd);
        CreatForderPacking(PATH_ArtAssets);
        CreatForderPacking(PATH_Resources);
        CreatForderPacking(PATH_Scenes);
        CreatForderPacking(PATH_StreamingAssets);
        CreatForderPacking(PATH_Scripts);
        CreatForderPacking(PATH_AssetBundle);
        AssetDatabase.Refresh();
    }
    private static void CreatForderPacking(string path)
    {
        string name = path.Substring(path.LastIndexOf('/') + 1);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log($"{name}Ŀ¼�������");
        }
        else
        {
            Debug.Log($"{name}Ŀ¼�Ѵ���");
        }

    }
}
