using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �ϴ�����AB��
/// </summary>
public class UpLoadAB
{
    [MenuItem("Tools/AB������/�ϴ�AB���ͶԱ��ļ�")]
    private static void UpLoadAllABFile()
    {
        DirectoryInfo directory = Directory.CreateDirectory(DataConfig.CurrentABFilePath);
        FileInfo[] fileInfos = directory.GetFiles();
        foreach (FileInfo fileInfo in fileInfos)
        {
            if (fileInfo.Extension == "" || fileInfo.Extension == ".txt")
            {
                FtpUploadFile(fileInfo.FullName, fileInfo.Name);
            }
        }
    }
    private async static void FtpUploadFile(string filePath, string fileName)
    {
        await Task.Run(() =>
        {
            try
            {
                //1.����һ��FTP���� �����ϴ�
                FtpWebRequest req = FtpWebRequest.Create(new Uri(DataConfig.UpABURL + fileName)) as FtpWebRequest;
                //2.����һ��ͨ��ƾ֤ ���������ϴ�
                NetworkCredential n = new NetworkCredential(DataConfig.Ftp_UserName, DataConfig.Ftp_Password);
                req.Credentials = n;
                //3.��������
                //  ���ô���Ϊnull
                req.Proxy = null;
                //  ������Ϻ� �Ƿ�رտ�������
                req.KeepAlive = false;
                //  ��������-�ϴ�
                req.Method = WebRequestMethods.Ftp.UploadFile;
                //  ָ����������� 2����
                req.UseBinary = true;
                //4.�ϴ��ļ�
                //  ftp��������
                Stream upLoadStream = req.GetRequestStream();
                //  ��ȡ�ļ���Ϣ д���������
                using (FileStream file = File.OpenRead(filePath))
                {
                    //һ��һ����ϴ�����
                    byte[] bytes = new byte[1024];
                    //����ֵ �����ȡ�˶��ٸ��ֽ�
                    int contentLength = file.Read(bytes, 0, bytes.Length);
                    while (contentLength != 0)
                    {
                        //д�뵽�ϴ�����
                        upLoadStream.Write(bytes, 0, contentLength);
                        //д���ٶ�
                        contentLength = file.Read(bytes, 0, bytes.Length);
                    }
                    //ѭ����Ϻ� ֤���ϴ�����
                    file.Close();
                    upLoadStream.Close();

                }
                Debug.Log(fileName + "�ϴ��ɹ�");
            }
            catch (Exception ex)
            {
                Debug.Log(fileName + "�ϴ�ʧ�ܣ�������Ϣ��" + ex.Message);
            }
        });
    }

    [MenuItem("Tools/AB������/��ӡpersistentDataPath·��")]
    private static void DebugPath()
    {
        Debug.Log(Application.persistentDataPath);
    }
}
