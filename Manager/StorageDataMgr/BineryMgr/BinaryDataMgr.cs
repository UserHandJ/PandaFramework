using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryDataMgr : BaseSingleton<BinaryDataMgr>
{
    /// <summary>
    /// 数据存储的位置
    /// </summary>
    private static string SAVE_PATH = Application.persistentDataPath + "/Data/";
    /// <summary>
    /// 存储文件后缀
    /// </summary>
    private static string FILE_EXTENSION = ".bd";

    /// <summary>
    /// 存储类对象数据
    /// </summary>
    /// <param name="obj">数据类</param>
    /// <param name="fileName">存储文件名称</param>
    public void Save(object obj, string fileName)
    {
        //先判断路径文件夹有没有
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        /*这是用文件流的方式存数据，
        using (FileStream fs = new FileStream(SAVE_PATH + fileName + FILE_EXTENSION, FileMode.OpenOrCreate, FileAccess.Write))
        {
            //把类序列化成二进制数据的工具  命名空间：using System.Runtime.Serialization.Formatters.Binary;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }
        */

        //改用内存流的方式可以对数据再做些操作，比如进行加密
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);

            byte[] bytes = ms.GetBuffer();
            //ToDo:..在这里可以做一些加密的工作
            File.WriteAllBytes(SAVE_PATH + fileName + FILE_EXTENSION, bytes);
            ms.Close();
        }

    }
    /// <summary>
    /// 读取2进制数据，并转换成对象
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="fileName">存储文件名称</param>
    /// <returns>数据对象</returns>
    public T Load<T>(string fileName) where T : class
    {
        //如果不存在这个文件 就直接返回泛型对象的默认值
        if (!File.Exists(SAVE_PATH + fileName + FILE_EXTENSION))
        {
            return default(T);
        }
        T obj = null;

        /*这是用文件流的方式读数据，
        using (FileStream fs = File.Open(SAVE_PATH + fileName + FILE_EXTENSION, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs) as T;
            fs.Close();
        }
        */

        byte[] bytes = File.ReadAllBytes(SAVE_PATH + fileName + FILE_EXTENSION);
        //ToDo..这里可以根据规则做解密的工作
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(ms) as T;
            ms.Close();
        }
        return obj;
    }
}
