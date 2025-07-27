using UnityEditor;
public class PLogerEditor
{
    [MenuItem("UPandaGF/��־ϵͳ/������־", false, 0)]
    public static void EnablePLoger()
    {
        ScriptingDefineSymbols.AddScriptingDefineSymbol("OPEN_PLOG");
    }

    [MenuItem("UPandaGF/��־ϵͳ/�ر���־", false, 1)]
    public static void ClosePLoger()
    {
        ScriptingDefineSymbols.RemoveScriptingDefineSymbol("OPEN_PLOG");
    }
}
