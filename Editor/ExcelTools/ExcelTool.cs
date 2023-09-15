using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ExcelTool
{
    /// <summary>
    /// excel�ļ���ŵ�·��
    /// </summary>
    public static string EXCEL_PATH = Application.dataPath + "/ArtAssets/Excel";
    /// <summary>
    /// ���ݽṹ��ű��洢λ��·��
    /// </summary>
    public static string DATA_CLASS_PATH = Application.dataPath + "/Scripts/ExcelData/DataClass/";
    /// <summary>
    /// ������ű��洢λ��·��
    /// </summary>
    public static string DATA_CONTAINER_PATH = Application.dataPath + "/Scripts/ExcelData/Container/";
    /// <summary>
    /// 2�������ݴ洢λ��·��
    /// </summary>
    public static string DATA_BINARY_PATH = Application.streamingAssetsPath + "/Binary/";
    /// <summary>
    /// Excel�����������ݿ�ʼ���к�
    /// </summary>
    public static int BEGIN_INDEX = 4;
    /// <summary>
    /// �����������ļ���׺
    /// </summary>
    public static string SUFFIX = ".binary";

    [MenuItem("Tools/ExcelTool/���������������������")]
    private static void GenerateExcelInfo()
    {
        //������ָ��·���е�����Excel�ļ� �������ɶ�Ӧ��3���ļ�
        DirectoryInfo dInfo = Directory.CreateDirectory(EXCEL_PATH);
        //�õ�ָ��·���е������ļ���Ϣ �൱�ھ��ǵõ����е�Excel��
        FileInfo[] files = dInfo.GetFiles();
        //���ݱ�����
        DataTableCollection tableCollection;
        for (int i = 0; i < files.Length; i++)
        {
            //�������Excel�ļ��Ͳ�������
            if (files[i].Extension != ".xlsx" && files[i].Extension != ".xls") continue;
            using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read))
            {
                //IExcelDataReader�࣬�����ж�ȡExcel����
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                tableCollection = excelReader.AsDataSet().Tables;
                fs.Close();
            }
            foreach (DataTable table in tableCollection)
            {
                Debug.Log(table.TableName);
                //�������ݽṹ��
                GenerateExcelDataClass(table);
                //����������
                GenerateExcelContainerContainer(table);
                //����2��������
                GenerateExcelBinary(table);
            }
        }
    }

    /// <summary>
    /// ����Excel���Ӧ�����ݽṹ��
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelDataClass(DataTable table)
    {
        //�ֶ�����
        DataRow rowName = GetVariableNameRow(table);
        //�ֶ�����
        DataRow rowType = GetVariableTypeRow(table);

        //�ж�·���Ƿ���� û�еĻ� �ʹ����ļ���
        if (!Directory.Exists(DATA_CLASS_PATH))
        {
            Directory.CreateDirectory(DATA_CLASS_PATH);
        }
        //���Ҫ���ɶ�Ӧ�����ݽṹ��ű� ��ʵ����ͨ����������ַ���ƴ�� Ȼ�����ļ�������
        string str = $"public class {table.TableName}\n{{\n";
        for (int i = 0; i < table.Columns.Count; i++)
        {
            str += $"   public {rowType[i].ToString()} {rowName[i].ToString()};\n";
        }
        str += "}";

        //��ƴ�Ӻõ��ַ����浽ָ���ļ���ȥ
        File.WriteAllText(DATA_CLASS_PATH + table.TableName + ".cs", str);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// ����Excel���Ӧ������������
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelContainerContainer(DataTable table)
    {
        //�õ�����������
        int keyIndex = GetKeyIndex(table);
        //�õ��ֶ�������
        DataRow rowType = GetVariableTypeRow(table);
        //û��·������·��
        if (!Directory.Exists(DATA_CONTAINER_PATH))
            Directory.CreateDirectory(DATA_CONTAINER_PATH);
        string str = "using System.Collections.Generic;\n";
        str += $"public class {table.TableName}Container\n{{\n";
        str += $"    public Dictionary<{rowType[keyIndex].ToString()},{table.TableName}> dataDic = new Dictionary<{rowType[keyIndex].ToString()},{table.TableName}>();";
        str += "\n}";

        File.WriteAllText(DATA_CONTAINER_PATH + table.TableName + "Container.cs", str);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// ����excel��2��������
    /// ������ôд�Ĵ洢���� ȡ��ʱ�����ô��
    /// ��������ֱ��ת�ɶ����ƺ�ʹ��ȥ���ַ����;��ȰѶ��������鳤�ȴ��ȥ���ٴ�ת�ɶ����Ƶ��ַ���
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelBinary(DataTable table)
    {
        //û��·������·��
        if (!Directory.Exists(DATA_BINARY_PATH)) Directory.CreateDirectory(DATA_BINARY_PATH);
        //����һ��2�����ļ�����д��
        using (FileStream fs = new FileStream(DATA_BINARY_PATH + table.TableName + SUFFIX, FileMode.OpenOrCreate, FileAccess.Write))
        {
            //�洢�����excel��Ӧ��2������Ϣ
            //1.��Ҫ�洢 ��Ҫд�����е����� �����ȡ
            //  -4��ԭ������Ϊ ǰ��4�������ù��� ��������Ҫ��¼����������
            fs.Write(BitConverter.GetBytes(table.Rows.Count - 4), 0, 4);
            //2.�洢�����ı�����
            string keyName = GetVariableNameRow(table)[GetKeyIndex(table)].ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(keyName);
            //�洢�ַ����ֽ�����ĳ���
            fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            //�洢�ַ����ֽ�����
            fs.Write(bytes, 0, bytes.Length);

            //�����������ݵ��� ����2���Ƶ�д��
            DataRow row;
            //�õ������� ��������������Ӧ�����д������
            DataRow rowType = GetVariableTypeRow(table);
            for (int i = BEGIN_INDEX; i < table.Rows.Count; i++)
            {
                //�õ�һ�е�����
                row = table.Rows[i];
                //������
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    switch (rowType[j].ToString())
                    {
                        case "int":
                            fs.Write(BitConverter.GetBytes(int.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "float":
                            fs.Write(BitConverter.GetBytes(float.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "bool":
                            fs.Write(BitConverter.GetBytes(bool.Parse(row[j].ToString())), 0, 1);
                            break;
                        case "string":
                            //bytes�������ǰ������������������Ҳд���ڴ��ˣ�������������װ��������
                            bytes = Encoding.UTF8.GetBytes(row[j].ToString());
                            //д���ַ����ֽ�����ĳ���
                            fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                            //д���ַ����ֽ�����
                            fs.Write(bytes, 0, bytes.Length);
                            break;
                        default:
                            Debug.LogError($"д������������ͳ���Ŀǰֻ֧��int float bool string\n����{rowType[j]}���У�{j}");
                            break;
                    }
                }
            }
            fs.Close();
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static int GetKeyIndex(DataTable table)
    {
        DataRow row = table.Rows[2];
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (row[i].ToString() == "key") return i;
        }
        //Ĭ�ϵ�һ��������
        return 0;
    }



    /// <summary>
    /// ��ȡ������������
    /// ��װ��Ŀ���ǣ������������п����������
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableNameRow(DataTable table)
    {
        return table.Rows[0];
    }
    /// <summary>
    /// ��ȡ��������������
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableTypeRow(DataTable table)
    {
        return table.Rows[1];
    }
}
