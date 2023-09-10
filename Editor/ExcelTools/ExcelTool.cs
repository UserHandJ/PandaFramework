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
    /// excel文件存放的路径
    /// </summary>
    public static string EXCEL_PATH = Application.dataPath + "/ArtAssets/Excel";
    /// <summary>
    /// 数据结构类脚本存储位置路径
    /// </summary>
    public static string DATA_CLASS_PATH = Application.dataPath + "/Scripts/ExcelData/DataClass/";
    /// <summary>
    /// 容器类脚本存储位置路径
    /// </summary>
    public static string DATA_CONTAINER_PATH = Application.dataPath + "/Scripts/ExcelData/Container/";
    /// <summary>
    /// 2进制数据存储位置路径
    /// </summary>
    public static string DATA_BINARY_PATH = Application.streamingAssetsPath + "/Binary/";
    /// <summary>
    /// Excel表中真正内容开始的行号
    /// </summary>
    public static int BEGIN_INDEX = 4;

    [MenuItem("Tools/ExcelTool/生成数据类和数据容器类")]
    private static void GenerateExcelInfo()
    {
        //记下在指定路径中的所有Excel文件 用于生成对应的3个文件
        DirectoryInfo dInfo = Directory.CreateDirectory(EXCEL_PATH);
        //得到指定路径中的所有文件信息 相当于就是得到所有的Excel表
        FileInfo[] files = dInfo.GetFiles();
        //数据表容器
        DataTableCollection tableCollection;
        for (int i = 0; i < files.Length; i++)
        {
            //如果不是Excel文件就不处理了
            if (files[i].Extension != ".xlsx" && files[i].Extension != ".xls") continue;
            using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read))
            {
                //IExcelDataReader类，从流中读取Excel数据
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                tableCollection = excelReader.AsDataSet().Tables;
                fs.Close();
            }
            foreach (DataTable table in tableCollection)
            {
                Debug.Log(table.TableName);
                //生成数据结构类

                //生成容器类

                //生成2进制数据
            }
        }
    }

}
