using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
/// ��������AB��
/// </summary>
public class ABUpdataMgr : BaseMonoSingleton<ABUpdataMgr>
{
    public class ABInfo
    {
        public string name;
        public long size;
        public string md5;
        public ABInfo(string name, string size, string md5)
        {
            this.name = name;
            this.size = long.Parse(size);
            this.md5 = md5;
        }
    }
    private string DownLoadURL = "ftp://192.168.31.48/AB/PC/";
    private string Ftp_UserName = "Admin";
    private string Ftp_Password = "Admin123";

    //���ڴ洢Զ��AB����Ϣ���ֵ� ֮��ͱ��ؽ��жԱȼ������ ���� ��������߼�
    private Dictionary<string, ABInfo> remoteABInfo = new Dictionary<string, ABInfo>();

    //���ڴ洢����AB����Ϣ���ֵ� ��Ҫ���ں�Զ����Ϣ�Ա�
    private Dictionary<string, ABInfo> localABInfo = new Dictionary<string, ABInfo>();

    //����Ǵ����ص�AB���б��ļ� �洢AB��������
    private List<string> downLoadList = new List<string>();

    //���ص��ļ�����
    private int downLoadOverNum = 0;

    /// <summary>
    /// ���ڼ���ȸ��µĺ���
    /// </summary>
    /// <param name="overCallBack">���½����Ļص����������Ƿ�ɹ�</param>
    /// <param name="updataInfoCallBack">������Ϣ�Ļص����ڼ�����и�����Ϣ������ȥ</param>
    public void CheckUpdata(UnityAction<bool> overCallBack, UnityAction<string> updataInfoCallBack)
    {
        //Ϊ�˱���������һ�α��� ��������Ϣ �������
        remoteABInfo.Clear();
        localABInfo.Clear();
        downLoadList.Clear();

        //1.����Զ����Դ�Ա��ļ�
        DownLoadABComparteFile((isOver) =>
        {
            updataInfoCallBack("��ʼ������Դ");
            if (isOver)
            {
                updataInfoCallBack("�Ա��ļ����ؽ���");
                string remoteInfo = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_TMP.txt");
                updataInfoCallBack("����Զ�˶Ա��ļ�");
                AnalysisABCompareFileInfo(remoteInfo, remoteABInfo);
                updataInfoCallBack("����Զ�˶Ա��ļ����");
                //2.���ر�����Դ�Ա��ļ�
                GetLocalABCompareFileInfo((isOK) =>
                {
                    if (isOK)
                    {
                        updataInfoCallBack("�������ضԱ��ļ����");
                        //3.�Ա����� Ȼ�����AB������
                        updataInfoCallBack("��ʼ�Ա�");
                        foreach (string abName in remoteABInfo.Keys)
                        {
                            //1.�ж� ��Щ��Դ���µ� Ȼ���¼ ֮����������
                            //�����ڱ��ضԱ���Ϣ��û�н�������ֵ�AB�� ���Լ�¼������
                            if (!localABInfo.ContainsKey(abName))
                            {
                                downLoadList.Add(abName);
                            }
                            else//���ֱ�����ͬ��AB�� Ȼ���������
                            {
                                //2.�ж� ��Щ��Դ����Ҫ���µ� Ȼ���¼ ֮����������
                                if (localABInfo[abName].md5 != remoteABInfo[abName].md5)
                                {
                                    downLoadList.Add(abName);
                                }
                                //���md5����� ֤����ͬһ����Դ ����Ҫ����
                                //3.�ж� ��Щ��Դ��Ҫɾ��
                                //ÿ�μ����һ�����ֵ�AB�� ���Ƴ����ص���Ϣ ��ô����ʣ��������Ϣ ����Զ��û�е�����
                                //�Ϳ��԰�����ɾ����
                                localABInfo.Remove(abName);
                            }
                        }
                        updataInfoCallBack("�Ա����");
                        updataInfoCallBack("��ʼɾ�����õ�AB���ļ�");
                        //����Ա����� �Ǿ���ɾ��û�õ����� ������AB��
                        //ɾ�����õ�AB��
                        foreach (string abName in localABInfo.Keys)
                        {
                            //����ɶ�д�ļ����������� ���Ǿ�ɾ���� 
                            //Ĭ����Դ�е� ��Ϣ ����û�취ɾ��
                            if (File.Exists(Application.persistentDataPath + "/" + abName))
                            {
                                File.Delete(Application.persistentDataPath + "/" + abName);
                            }
                        }
                        updataInfoCallBack("���غ͸���AB���ļ�");
                        //���ش������б��е�����AB��
                        //����
                        DownLoadABFile((isOver) =>
                        {
                            if (isOver)
                            {
                                //����������AB���ļ���
                                //�ѱ��ص�AB���Ա��ļ� ����Ϊ����
                                //��֮ǰ��ȡ������ Զ�˶Ա��ļ���Ϣ �洢�� ���� 
                                updataInfoCallBack("��ʼ���±���AB���Ա��ļ�Ϊ����");
                                File.WriteAllText(Application.persistentDataPath + "/ABCompareInfo.txt", remoteInfo);
                                overCallBack(true);
                            }
                            else
                            {
                                overCallBack(false);
                            }
                        },
                        updataInfoCallBack);
                    }
                    else
                    {
                        overCallBack(false);
                    }
                });
            }
            else
            {
                overCallBack(false);
            }
        });
    }
    /// <summary>
    /// ����AB���Ա��ļ����� ������Ϣ
    /// </summary>
    /// <param name="overCallBack"></param>
    public void GetLocalABCompareFileInfo(UnityAction<bool> overCallBack)
    {
        //����ɶ���д�ļ����� ���ڶԱ��ļ� ˵��֮ǰ�Ѿ����ظ��¹���
        if (File.Exists(Application.persistentDataPath + "/ABCompareInfo.txt"))
        {
            StartCoroutine(IE_GetLocalABCompareFileInfo(Application.persistentDataPath + "/ABCompareInfo.txt", overCallBack));
        }
        //ֻ�е��ɶ���д��û�жԱ��ļ�ʱ  �Ż�������Ĭ����Դ����һ�ν���Ϸʱ�Żᷢ����
        else if (File.Exists(Application.streamingAssetsPath + "/ABCompareInfo.txt"))
        {
            StartCoroutine(IE_GetLocalABCompareFileInfo(Application.streamingAssetsPath + "/ABCompareInfo.txt", overCallBack));
        }
        //������������� ֤����һ�β���û��Ĭ����Դ 
        else
        {
            overCallBack(true);
        }
    }
    /// <summary>
    /// ���ر�����Ϣ ���ҽ��������ֵ�
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="overCallBack"></param>
    /// <returns></returns>
    private IEnumerator IE_GetLocalABCompareFileInfo(string filePath, UnityAction<bool> overCallBack)
    {
        //ͨ�� UnityWebRequest ȥ���ر����ļ�
        UnityWebRequest req = UnityWebRequest.Get(filePath);
        yield return req.SendWebRequest();
        //��ȡ�ļ��ɹ� ��������ִ��
        if (req.result == UnityWebRequest.Result.Success)
        {
            AnalysisABCompareFileInfo(req.downloadHandler.text, localABInfo);
            overCallBack(true);
        }
        else
        {
            overCallBack(false);
        }
    }

    /// <summary>
    /// ������Դ�Ա��ļ���txt��
    /// </summary>
    /// <param name="overCallBack">���ػص����������Ƿ����سɹ�</param>
    public async void DownLoadABComparteFile(UnityAction<bool> overCallBack)
    {
        //1.����Դ������������Դ�Ա��ļ�
        // www UnityWebRequest ftp���api
        //print(Application.persistentDataPath);
        //�����Ƿ�ɹ�
        bool isOver = false;
        //�����ش���
        int reDownTimes = 5;
        //���������߳��з���Unity���̵߳� Application ���� ����������
        string localPath = Application.persistentDataPath;
        while (!isOver && reDownTimes > 0)
        {
            await Task.Run(() =>
            {
                isOver = DownLoadFile("ABCompareInfo.txt", localPath + "/ABCompareInfo_TMP.txt");
            });
            --reDownTimes;
        }
        //�����ⲿ�ɹ����
        overCallBack?.Invoke(isOver);
    }
    /// <summary>
    /// �����ļ�
    /// </summary>
    /// <param name="fileName">�ļ�����</param>
    /// <param name="localPath">�ļ�·��</param>
    /// <returns></returns>
    private bool DownLoadFile(string fileName, string localPath)
    {
        try
        {
            //1.����һ��FTP���� ��������
            FtpWebRequest req = FtpWebRequest.Create(new Uri(DownLoadURL + fileName)) as FtpWebRequest;
            //2.����һ��ͨ��ƾ֤ �����������أ�����������˺� ���Բ�����ƾ֤ ����ʵ�ʿ����� ���� ���ǲ�Ҫ���������˺ţ�
            NetworkCredential n = new NetworkCredential(Ftp_UserName, Ftp_Password);
            req.Credentials = n;
            //3.��������
            //  ���ô���Ϊnull
            req.Proxy = null;
            //  ������Ϻ� �Ƿ�رտ�������
            req.KeepAlive = false;
            //  ��������-����
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            //  ָ����������� 2����
            req.UseBinary = true;
            //4.�����ļ�
            //  ftp��������
            FtpWebResponse res = req.GetResponse() as FtpWebResponse;
            Stream downLoadStream = res.GetResponseStream();
            using (FileStream file = File.Create(localPath))
            {
                //һ��һ�����������
                byte[] bytes = new byte[1024];
                //����ֵ  �����ȡ�˶��ٸ��ֽ�
                int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                //ѭ����������
                while (contentLength != 0)
                {
                    //д�뵽�����ļ�����
                    file.Write(bytes, 0, contentLength);
                    //д���ٶ�
                    contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                }
                //ѭ����Ϻ� ֤�����ؽ���
                file.Close();
                downLoadStream.Close();
                return true;
            }

        }
        catch (System.Exception ex)
        {
            print(fileName + "����ʧ��:" + ex.Message);
            return false;
        }
    }
    /// <summary>
    /// ������Դ�Ա��ļ�
    /// </summary>
    /// <param name="info">�������ַ���</param>
    /// <param name="AbInfo">��������Ϣ������ֵ�</param>
    public void AnalysisABCompareFileInfo(string info, Dictionary<string, ABInfo> AbInfo)
    {
        //���ǻ�ȡ��Դ�Ա��ļ��е� �ַ�����Ϣ ���в��
        //string info = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_TMP.txt");
        //ͨ��|����ַ��� ��һ����AB����Ϣ��ֳ���
        string[] strs = info.Split('|');
        string[] abInfos = null;
        for (int i = 0; i < strs.Length; i++)
        {
            //�ְ�һ��AB����ϸ��Ϣ��ֳ���
            abInfos = strs[i].Split(' ');
            //��¼ÿһ��Զ��AB������Ϣ ֮�� �������Ա�
            AbInfo.Add(abInfos[0], new ABInfo(abInfos[0], abInfos[1], abInfos[2]));
        }
    }

    /// <summary>
    /// ���ݽ����ĶԱ��ļ� ����AB��
    /// </summary>
    /// <param name="overCallBack">���ؽ����Ļص�������������Դ�Ƿ�ȫ�����سɹ�</param>
    /// <param name="updatePro">���ؽ��Ȼص���ÿ����һ�������سɹ��������</param>
    public async void DownLoadABFile(UnityAction<bool> overCallBack, UnityAction<string> updatePro)
    {
        //���ش洢��·�� ���ڶ��̲߳��ܷ���Unity��ص�һЩ���ݱ���Application �����������ⲿ
        string DownPath = Application.persistentDataPath + "/";
        // �Ƿ����سɹ�
        bool isOver = false;
        //���سɹ����б� ֮�������Ƴ����سɹ�������
        List<string> tempList = new List<string>();
        //�������ص�������
        int ReDownTimes = 5;
        //���سɹ�����Դ��
        downLoadOverNum = 0;
        //��һ��������Ҫ���ض��ٸ���Դ
        int currentDownCount = downLoadList.Count;
        //whileѭ����Ŀ�� �ǽ���n���������� ���������쳣ʱ ����ʧ��
        while (downLoadList.Count > 0 && ReDownTimes > 0)
        {
            for (int i = 0; i < downLoadList.Count; i++)
            {
                isOver = false;
                await Task.Run(() =>
                {
                    isOver = DownLoadFile(downLoadList[i], DownPath + downLoadList[i]);
                });
                if (isOver)
                {
                    //Ҫ֪�����������˶��� �������
                    updatePro(++downLoadOverNum + "/" + currentDownCount);
                    //���سɹ���¼����
                    tempList.Add(downLoadList[i]);
                }
            }
            //�����سɹ����ļ��� �Ӵ������б����Ƴ�
            for (int i = 0; i < tempList.Count; i++)
            {
                downLoadList.Remove(tempList[i]);
            }
            --ReDownTimes;
        }
        //�������ݶ��������� �����ⲿ�Ƿ��������
        overCallBack(downLoadList.Count == 0);
    }
}
