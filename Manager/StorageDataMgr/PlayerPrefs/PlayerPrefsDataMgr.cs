using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerPrefsDataMgr : BaseSingleton<PlayerPrefsDataMgr>
{
    /// <summary>
    /// ��ӡ����
    /// </summary>
    /// <param name="db">����</param>
    private void MyDebug(string db)
    {
#if UNITY_EDITOR
        //Debug.Log(db);
#endif
    }
    /// <summary>
    /// �洢����
    /// </summary>
    /// <param name="data">���ݶ���</param>
    /// <param name="keyName">���ݶ����Ψһkey</param>
    public void SaveData(object data, string keyName)
    {
        //����Ҫͨ�� Type �õ��������ݶ�������е� �ֶ�
        //Ȼ���� PlayerPrefs�����д洢

        #region ��һ�� ��ȡ�������ݶ���������ֶ�
        Type dataType = data.GetType();
        //�õ����е��ֶ�
        FieldInfo[] infos = dataType.GetFields();
        #endregion

        #region �ڶ��� �Լ�����һ��key�Ĺ��� �������ݴ洢
        //���Ǵ洢����ͨ��PlayerPrefs�����д洢��
        //��֤key��Ψһ�� ���Ǿ���Ҫ�Լ���һ��key�Ĺ���

        //�����Լ���һ������
        // keyName_��������_�ֶ�����_�ֶ���
        #endregion

        #region ������ ������Щ�ֶ� �������ݴ洢
        string saveKeyName = "";
        FieldInfo info;
        for (int i = 0; i < infos.Length; i++)
        {
            //��ÿһ���ֶ� �������ݴ洢
            //�õ�������ֶ���Ϣ
            info = infos[i];
            //ͨ��FieldInfo����ֱ�ӻ�ȡ�� �ֶε����� ���ֶε�����
            //�ֶε����� info.FieldType.Name
            //�ֶε����� info.Name;

            //Ҫ�������Ƕ���key��ƴ�ӹ��� ������key������
            //Player1_PlayerInfo_Int32_age
            saveKeyName = keyName + "_" + dataType.Name + "_" + info.FieldType.Name + "_" + info.Name;

            //���ڵõ���Key �������ǵĹ���
            //��������Ҫ��ͨ��PlayerPrefs�����д洢
            //��λ�ȡֵ
            //info.GetValue(data)
            //��װ��һ������ ר�����洢ֵ 
            SaveValue(info.GetValue(data), saveKeyName);
        }
        PlayerPrefs.Save();
        #endregion
    }
    /// <summary>
    /// ��װ��һ������ ר�����洢ֵ 
    /// </summary>
    /// <param name="value">���ݶ���</param>
    /// <param name="keyName">���ݵ�Key</param>
    private void SaveValue(object value, string keyName)
    {
        //ֱ��ͨ��PlayerPrefs�����д洢��
        //���Ǹ����������͵Ĳ�ͬ ������ʹ����һ��API�����д洢
        //PlayerPrefsֻ֧��3�����ʹ洢 
        //�ж� �������� ��ʲô���� Ȼ����þ���ķ������洢
        Type fieldType = value.GetType();
        //�����ж�
        if (fieldType == typeof(int))
        {
            MyDebug($"{keyName}�洢int,���ͳ�Ա{fieldType.Name}");
            int intValue = (int)value;
            //TODO:���������Ϊint���ݼ���
            //rValue += 10;
            PlayerPrefs.SetInt(keyName, intValue);
        }
        else if (fieldType == typeof(float))
        {
            MyDebug($"{keyName}�洢float,���ͳ�Ա{fieldType.Name}");
            PlayerPrefs.SetFloat(keyName, (float)value);
        }
        else if (fieldType == typeof(string))
        {
            MyDebug($"{keyName}�洢string,���ͳ�Ա{fieldType.Name}");
            PlayerPrefs.SetString(keyName, (string)value);
        }
        else if (fieldType == typeof(bool))
        {
            MyDebug($"{keyName}�洢bool,���ͳ�Ա{fieldType.Name}");
            //�Լ����Ĵ洢bool�Ĺ���1 true,0 false
            PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);
        }
        //�����List��������
        else if (typeof(IList).IsAssignableFrom(fieldType))
        {
            MyDebug($"{keyName}�洢List����,���ͳ�Ա{fieldType.Name}");
            //����װ����
            IList list = value as IList;
            //�ȴ洢�������� 
            PlayerPrefs.SetInt(keyName, list.Count);
            int index = 0;
            foreach (object item in list)
            {
                //�洢�����ֵ
                SaveValue(item, keyName + index);
                index++;
            }
        }
        //�ж��ǲ���Dictionary���� ͨ��Dictionary�ĸ������ж�
        else if (typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            MyDebug($"{keyName}�洢Dictionary,���ͳ�Ա{fieldType.Name}");
            //����װ����
            IDictionary dic = value as IDictionary;
            //�ȴ��ֵ䳤��
            PlayerPrefs.SetInt(keyName, dic.Count);
            //�����洢Dic����ľ���ֵ
            //�������� ��ʾ�� ���� key
            int index = 0;
            foreach (object key in dic.Keys)
            {
                SaveValue(key, keyName + "_key_" + index);
                SaveValue(dic[key], keyName + "_value_" + index);
                index++;
            }
        }
        else
        {
            SaveData(value, keyName);
        }
    }
    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="type">��Ҫ��ȡ���ݵ� ��������Type</param>
    /// <param name="keyName">���ݶ����Ψһkey �Լ����Ƶ�</param>
    /// <returns></returns>
    public object LoadData(Type type, string keyName)
    {
        //����object������ ��ʹ�� Type����
        //��ҪĿ���ǽ�Լһ�д��루���ⲿ��
        //����������Ҫ ��ȡһ��Player���͵����� �����object ��ͱ������ⲿnewһ��������
        //������Type�� ��ֻ�ô��� һ��Type typeof(Player) Ȼ�������ڲ���̬����һ��������㷵�س���
        //�ﵽ�� �������ⲿ ��дһ�д��������

        //�����㴫������� �� keyName
        //������洢����ʱ  key��ƴ�ӹ��� ���������ݵĻ�ȡ��ֵ ���س�ȥ

        //���ݴ����Type ����һ������ ���ڴ洢����
        object data = Activator.CreateInstance(type);
        //Ҫ�����new�����Ķ����д洢���� �������
        //�õ������ֶ� 
        FieldInfo[] infos = type.GetFields();
        //����ƴ��key���ַ���
        string loadKeyName = "";
        //���ڴ洢 �����ֶ���Ϣ�� ����
        FieldInfo info;
        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            //key��ƴ�ӹ��� һ���Ǻʹ洢ʱһģһ�� ���������ҵ���Ӧ����
            loadKeyName = keyName + "_" + type.Name + "_" + info.FieldType.Name + "_" + info.Name;
            //��key �Ϳ��Խ�� PlayerPrefs����ȡ����
            //������ݵ�data�� 
            info.SetValue(data, LoadValue(info.FieldType, loadKeyName));
        }
        return data;
    }
    /// <summary>
    /// �õ��������ݵķ���
    /// </summary>
    /// <param name="fieldType">�ֶ����� �����ж� ���ĸ�api����ȡ</param>
    /// <param name="keyName">���ڻ�ȡ��������</param>
    /// <returns></returns>
    private object LoadValue(Type fieldType, string keyName)
    {
        //���� �ֶ����� ���ж� ���ĸ�API����ȡ
        if (fieldType == typeof(int))
        {
            //TODO:����洢ʱ�м��ܲ�����Ҫ��������н���
            return PlayerPrefs.GetInt(keyName, 0);//�ڶ������������û�����ֵ��������Ĭ��ֵ
        }
        else if (fieldType == typeof(float))
        {
            return PlayerPrefs.GetFloat(keyName, 0);
        }
        else if (fieldType == typeof(string))
        {
            return PlayerPrefs.GetString(keyName, "");
        }
        else if (fieldType == typeof(bool))
        {
            //�����Զ���洢bool�Ĺ��� ������ֵ�Ļ�ȡ
            return PlayerPrefs.GetInt(keyName, 0) == 1 ? true : false;
        }
        else if (typeof(IList).IsAssignableFrom(fieldType))
        {
            //�õ�����
            int count = PlayerPrefs.GetInt(keyName, 0);
            //ʵ����һ��List���� �����и�ֵ
            //���˷�����˫A�� Activator���п���ʵ����List����
            IList list = Activator.CreateInstance(fieldType) as IList;
            for (int i = 0; i < count; i++)
            {
                //Ŀ����Ҫ�õ� List�з��͵����� 
                list.Add(LoadValue(fieldType.GetGenericArguments()[0], keyName + i));
            }
            return list;
        }
        else if (typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            //�õ��ֵ�ĳ���
            int count = PlayerPrefs.GetInt(keyName, 0);
            //ʵ����һ���ֵ���� �ø���װ����
            IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;
            Type[] kvType = fieldType.GetGenericArguments();
            for (int i = 0; i < count; i++)
            {
                dic.Add(LoadValue(kvType[0], keyName + "_key_" + i), LoadValue(kvType[1], keyName + "_value_" + i));
            }
            return dic;
        }
        else
        {
            return LoadData(fieldType, keyName);
        }
    }
}
