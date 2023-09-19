using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

/// <summary>
/// ��ȡ�����Ƶ����ݣ�ͨ������õ����ݣ��������ݴ�������������������������ݹ�����ͳһ����
/// </summary>
public class BinaryDataMgr : BaseSingleton<BinaryDataMgr>
{
    /// <summary>
    /// ������洢��λ��
    /// </summary>
    private static string SAVE_PATH = Application.persistentDataPath + "/Data/";

    /// <summary>
    /// �洢�ļ���׺
    /// </summary>
    private static string FILE_EXTENSION = ".binary";

    /// <summary>
    /// Excel���õ� 2����������� �洢λ��·��
    /// ·����ExcelTool������ݴ洢·��һ��
    /// </summary>
    private static string DATA_BINARY_PATH = Application.streamingAssetsPath + "/BinaryData/";
    /// <summary>
    /// ���ڴ洢 ����Excel������������ ������
    /// </summary>
    private Dictionary<string, object> tableDic = new Dictionary<string, object>();

    public BinaryDataMgr()
    {
        InitData();
    }
    /// <summary>
    /// ÿ��һ��Excel�������������LoadTable�ķ�����������
    /// </summary>
    public void InitData()
    {

    }
    /// <summary>
    /// ����Excel���2�������ݵ��ڴ��� 
    /// </summary>
    /// <typeparam name="T">������</typeparam>
    /// <typeparam name="K">���ݽṹ��</typeparam>
    public void LoadTable<T, K>()
    {
        //��ȡ excel���Ӧ��2�����ļ� �����н���
        using (FileStream fs = File.Open(DATA_BINARY_PATH + typeof(K).Name + FILE_EXTENSION, FileMode.Open, FileAccess.Read))
        {
            //�����ֽ�����
            byte[] bytes = new byte[fs.Length];
            //·���µ��ļ����ݶ����ֽ�������
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();

            //���ڼ�¼��ǰ��ȡ�˶����ֽ���
            int index = 0;

            //��ȡһ���ж���������
            int count = BitConverter.ToInt32(bytes, index);
            index += 4;

            //��ȡ����������
            int keyNameLength = BitConverter.ToInt32(bytes, index);
            index += 4;
            string keyName = Encoding.UTF8.GetString(bytes, index, keyNameLength);
            index += keyNameLength;

            //�������������
            Type contaninerType = typeof(T);
            object contaninerObj = Activator.CreateInstance(contaninerType);
            //�õ����ݽṹ���Type
            Type classType = typeof(K);
            //ͨ������ �õ����ݽṹ�� �����ֶε���Ϣ
            FieldInfo[] infos = classType.GetFields();
            //��ȡÿһ�е���Ϣ
            for (int i = 0; i < count; i++)
            {
                //ʵ����һ�����ݽṹ�� ����
                object dataObj = Activator.CreateInstance(classType);
                foreach (FieldInfo info in infos)
                {
                    if (info.FieldType == typeof(int))
                    {
                        //�൱�ھ��ǰ�2��������תΪint Ȼ��ֵ���˶�Ӧ���ֶ�
                        info.SetValue(dataObj, BitConverter.ToInt32(bytes, index));
                        index += 4;
                    }
                    else if (info.FieldType == typeof(float))
                    {
                        info.SetValue(dataObj, BitConverter.ToSingle(bytes, index));
                        index += 4;
                    }
                    else if (info.FieldType == typeof(bool))
                    {
                        info.SetValue(dataObj, BitConverter.ToBoolean(bytes, index));
                        index += 4;
                    }
                    else if (info.FieldType == typeof(string))
                    {
                        //�ȶ�ȡ�ַ����ֽ�����ĳ���
                        int length = BitConverter.ToInt32(bytes, index);
                        index += 4;
                        info.SetValue(dataObj, Encoding.UTF8.GetString(bytes, index, length));
                        index += length;
                    }
                }
                //��ȡ��һ�е������� Ӧ�ð����������ӵ�����������
                //�õ����������е� �ֵ����
                object dicObject = contaninerType.GetField("dataDic").GetValue(contaninerObj);
                //ͨ���ֵ����õ����е� Add����
                MethodInfo mInfo = dicObject.GetType().GetMethod("Add");
                //�õ����ݽṹ������� ָ�������ֶε�ֵ
                object keyValue = classType.GetField(keyName).GetValue(dataObj);
                mInfo.Invoke(dicObject, new object[] { keyValue, dataObj });
            }
            //�Ѷ�ȡ��ı��¼����
            tableDic.Add(typeof(T).Name, contaninerObj);

            fs.Close();
        }
    }
    /// <summary>
    /// �õ�һ�ű����Ϣ
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <returns></returns>
    public T GetTable<T>() where T : class
    {
        string tableName = typeof(T).Name;
        if(tableDic.ContainsKey(tableName))
        {
            return tableDic[tableName] as T;
        }
        return null;
    }

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
