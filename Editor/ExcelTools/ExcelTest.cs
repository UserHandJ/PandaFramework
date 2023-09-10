using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Excel;
using System.Data;

public class ExcelTest
{
    private static string EXCEL_PATH = Application.dataPath + "/ArtAssets/Excel/Exceltest.xlsx";
    [MenuItem("Tools/ExcelTool/��Excel��(����)")]
    private static void OpenExcel()
    {
        using (FileStream fs = File.Open(EXCEL_PATH, FileMode.Open, FileAccess.Read))
        {
            //ͨ���ļ�����ȡExcel����
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            //��excel���е�����ת��ΪDataSet�������� �������� ��ȡ���е�����
            DataSet result = excelReader.AsDataSet();
            //�õ�Excel�ļ��е����б���Ϣ
            for (int i = 0; i < result.Tables.Count; i++)
            {
                Debug.Log("������" + result.Tables[i].TableName);
                Debug.Log("������" + result.Tables[i].Rows.Count);
                Debug.Log("������" + result.Tables[i].Columns.Count);
            }
            Debug.Log("----------------�ָ���----------------");
            for (int i = 0; i < result.Tables.Count; i++)
            {
                //�õ�����һ�ű�ľ�������
                DataTable table = result.Tables[i];
                //�õ�����һ�е�����
                //DataRow row = table.Rows[0];
                //�õ�����ĳһ�е���Ϣ
                //Debug.Log(row[1].ToString());
                DataRow row;
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    //�õ�ÿһ�е���Ϣ
                    row = table.Rows[j];
                    Debug.Log("*********�µ�һ��************");
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        Debug.Log(row[k].ToString());
                    }
                }
            }
            fs.Close();
        }
    }
}
