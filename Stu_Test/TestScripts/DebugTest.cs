using UnityEngine;

public class DebugTest : MonoBehaviour
{
    public LogConfig logConfig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        PLoger.InitLog(logConfig);

        PLoger.Log("<color=red>��ӡ</color><color=yellow>����</color>");
        PLoger.LogFormat("{0},{1},{2}", "<color=red>���1</color>", "<color=yellow>���2</color>", "<color=blue>���3</color>");
        PLoger.LogWarning("<color=red>��ӡ</color><color=yellow>����</color>");
        PLoger.LogWarningFormat("{0},{1},{2}", "<color=red>���1</color>", "<color=yellow>���2</color>", "<color=blue>���3</color>");
        PLoger.LogError("<color=red>��ӡ</color><color=yellow>����</color>");
        PLoger.LogErrorFormat("{0},{1},{2}", "<color=red>���1</color>", "<color=yellow>���2</color>", "<color=blue>���3</color>");

        Debug.Log(" Debug.Log");
    }
}
