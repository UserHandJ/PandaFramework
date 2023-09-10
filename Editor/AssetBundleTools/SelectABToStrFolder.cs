using Codice.Client.BaseCommands.Fileinfo;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SelectABToStrFolder
{
    [MenuItem("Tools/AB������/�ƶ�ѡ�е�AB����Դ��StreamingAssets")]
    private static void MoveABToStreamingAssets()
    {
        //ͨ���༭��Selection���еķ��� ��ȡ��Project������ѡ�е���Դ 
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //���һ����Դ��û��ѡ�� ��û�б�Ҫ���������߼���
        if (selectedAsset.Length == 0)
        {
            Debug.Log("û��ѡ���ļ�");
            return;
        }

        //����ƴ�ӱ���Ĭ��AB����Դ��Ϣ���ַ���
        string abCompareInfo = "";
        //����ѡ�е���Դ����
        foreach (Object asset in selectedAsset)
        {
            //ͨ��Assetdatabase�� ��ȡ ��Դ��·��
            string assetPath = AssetDatabase.GetAssetPath(asset);
            //�ж�ѡȡ����Դ�ǲ���AB���ļ����µģ����ǵĻ�����
            string judge_fileName = assetPath.Substring(0, assetPath.LastIndexOf('/'));
            if (judge_fileName != "Assets/AssetBundle/PC")
            {
                Debug.LogError($"�벻ҪѡȡAssets/AssetBundle/PC·��������ļ�");
                return;
            }
            //��ȡ·�����е��ļ��� ������Ϊ StreamingAssets�е��ļ���
            string fileName = assetPath.Substring(assetPath.LastIndexOf('/'));

            // �ж��Ƿ���.���� ����� ֤���к�׺ ������
            if (fileName.IndexOf('.') != -1)
                continue;//Ҳ�����ڿ���֮ǰȥ��ȡȫ·����Ȼ��ͨ��FileInfoȥ��ȡ��׺���ж� ������׼ȷ

            //����AssetDatabase�е�API ��ѡ���ļ� ���Ƶ�Ŀ��·��
            AssetDatabase.CopyAsset(assetPath, "Assets/StreamingAssets" + fileName);

            //��ȡ������StreamingAssets�ļ����е��ļ���ȫ����Ϣ
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(Application.streamingAssetsPath + fileName);
            //ƴ��AB����Ϣ���ַ�����
            abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + CreatABMD5.GetMD5(fileInfo.FullName);
            //��һ�����Ÿ������AB����Ϣ
            abCompareInfo += "|";
        }
        //ȥ�����һ��|���� Ϊ��֮�����ַ�������
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        //������Ĭ����Դ�ĶԱ���Ϣ �����ļ�
        File.WriteAllText(Application.streamingAssetsPath + "/ABCompareInfo.txt", abCompareInfo);
        AssetDatabase.Refresh();
    }

}
