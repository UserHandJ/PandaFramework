using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �Ա��ļ���ʽ��
/// �ļ��� �ļ���С MD5��|�ļ��� �ļ���С MD5��
/// </summary>
public class CreatABMD5
{
    [MenuItem("Tools/AB������/�����Ա��ļ�")]
    public static void CreateABCompareFile()
    {
        //��ȡ�ļ�����Ϣ
        DirectoryInfo directory = Directory.CreateDirectory(DataConfig.CurrentABFilePath);
        //��ȡ��Ŀ¼�µ������ļ���Ϣ
        FileInfo[] fileInfos = directory.GetFiles();

        //���ڴ洢��Ϣ�� �ַ���
        string abCompareInfo = "";

        foreach (FileInfo item in fileInfos)
        {
            //û�к�׺�� ����AB�� ����ֻ��ҪAB������Ϣ
            if (item.Extension == "")
            {
                //ƴ��һ��AB������Ϣ
                abCompareInfo += item.Name + " " + item.Length + " " + GetMD5(item.FullName);
                abCompareInfo += "|";
            }
        }
        //��Ϊѭ����Ϻ� ���������һ�� | ���� ���� ����ȥ��
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        //�洢ƴ�Ӻõ� AB����Դ��Ϣ
        File.WriteAllText(DataConfig.CurrentABFilePath + "/ABCompareInfo.txt", abCompareInfo);
        AssetDatabase.Refresh();
        Debug.Log("AB���Ա��ļ����ɳɹ�,·����" + DataConfig.CurrentABFilePath + "/ABCompareInfo.txt");

    }
    /// <summary>
    /// �õ��ļ���MD5��
    /// </summary>
    /// <param name="filePath">�ļ�·��</param>
    /// <returns></returns>
    public static string GetMD5(string filePath)
    {
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            //����һ��MD5���� ��������MD5��
            MD5 md5 = new MD5CryptoServiceProvider();
            //����API �õ����ݵ�MD5�� 16���ֽ� ����
            byte[] md5Info = md5.ComputeHash(file);
            //�ر��ļ���
            file.Close();
            //��16���ֽ�ת��Ϊ 16���� ƴ�ӳ��ַ��� Ϊ�˼�Сmd5��ĳ���
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
            {
                sb.Append(md5Info[i].ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
