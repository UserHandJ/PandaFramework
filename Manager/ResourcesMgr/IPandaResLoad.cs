using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPandaResLoad 
{
    /// <summary>
    /// ͬ������
    /// ���ݷ��ͼ�����Դ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Respath"></param>
    /// <returns></returns>
    T Load<T>(string Respath) where T : UnityEngine.Object;

}
