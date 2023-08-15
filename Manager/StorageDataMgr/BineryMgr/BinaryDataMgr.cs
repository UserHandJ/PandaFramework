using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryDataMgr : BaseSingleton<BinaryDataMgr>
{
    /// <summary>
    /// ���ݴ洢��λ��
    /// </summary>
    private static string SAVE_PATH = Application.persistentDataPath + "/Data/";
    /// <summary>
    /// �洢�ļ���׺
    /// </summary>
    private static string FILE_EXTENSION = ".bd";

    /// <summary>
    /// �洢���������
    /// </summary>
    /// <param name="obj">������</param>
    /// <param name="fileName">�洢�ļ�����</param>
    public void Save(object obj, string fileName)
    {
        //���ж�·���ļ�����û��
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        /*�������ļ����ķ�ʽ�����ݣ�
        using (FileStream fs = new FileStream(SAVE_PATH + fileName + FILE_EXTENSION, FileMode.OpenOrCreate, FileAccess.Write))
        {
            //�������л��ɶ��������ݵĹ���  �����ռ䣺using System.Runtime.Serialization.Formatters.Binary;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }
        */

        //�����ڴ����ķ�ʽ���Զ���������Щ������������м���
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);

            byte[] bytes = ms.GetBuffer();
            //ToDo:..�����������һЩ���ܵĹ���
            File.WriteAllBytes(SAVE_PATH + fileName + FILE_EXTENSION, bytes);
            ms.Close();
        }

    }
    /// <summary>
    /// ��ȡ2�������ݣ���ת���ɶ���
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="fileName">�洢�ļ�����</param>
    /// <returns>���ݶ���</returns>
    public T Load<T>(string fileName) where T : class
    {
        //�������������ļ� ��ֱ�ӷ��ط��Ͷ����Ĭ��ֵ
        if (!File.Exists(SAVE_PATH + fileName + FILE_EXTENSION))
        {
            return default(T);
        }
        T obj = null;

        /*�������ļ����ķ�ʽ�����ݣ�
        using (FileStream fs = File.Open(SAVE_PATH + fileName + FILE_EXTENSION, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs) as T;
            fs.Close();
        }
        */

        byte[] bytes = File.ReadAllBytes(SAVE_PATH + fileName + FILE_EXTENSION);
        //ToDo..������Ը��ݹ��������ܵĹ���
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(ms) as T;
            ms.Close();
        }
        return obj;
    }
}
