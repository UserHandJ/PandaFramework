using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

                //����������

                //����2��������
            }
        }
    }

}
