using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABDownTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ABUpdataMgr.Instance.CheckUpdata((isOver) =>
        {
            if(isOver)
            {
                print("���½���");
            }
            else
            {
                print("����ʧ�ܣ��������");
            }
        },
        (info) =>
        {
            print(info);
        });
    }


}
