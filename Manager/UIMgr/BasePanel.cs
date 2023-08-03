using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ������
/// ͨ��������ٵ��ҵ����е��ӿؼ�����Լ�ҿؼ��Ĺ�����
/// �������д����߼� 
/// </summary>
public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// UI�������
    /// UIBehaviour������UI����Ļ��࣬UI�������ֱ�ӻ��߼�Ӽ̳�UIBehaviour���������ģ�
    /// ���̳���MonoBehavior������ӵ�к�Unity��ͬ����������
    /// </summary>
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();


    protected virtual void Awake()
    {
        FindChildrenUIComponent<Button>();
        FindChildrenUIComponent<Slider>();
        FindChildrenUIComponent<Toggle>();
        FindChildrenUIComponent<Image>();
        FindChildrenUIComponent<Text>();
        FindChildrenUIComponent<ScrollRect>();
        FindChildrenUIComponent<InputField>();
    }
    /// <summary>
    /// ��������д��ʾ�߼�
    /// </summary>
    public virtual void ShowMe() { }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void HideMe() { }
    /// <summary>
    /// ��ť�����¼�����дʱ�Լ����ݴ���İ�ť���ִ����Ӧ��ť���߼�
    /// </summary>
    /// <param name="btnName">��ť����</param>
    protected virtual void Button_OnClick(string btnName) { }
    /// <summary>
    /// Toggle��������¼���Ҳ�Ǹ��ݴ�������ִ����Ӧ���¼�
    /// </summary>
    /// <param name="toggleName">Toggle����</param>
    /// <param name="value">Toggle��ֵ</param>
    protected virtual void Toggle_OnValueChanged(string toggleName, bool value) { }
    /// <summary>
    /// Slider��������¼�
    /// </summary>
    /// <param name="SliderName"></param>
    /// <param name="value"></param>
    protected virtual void Slider_OnValueChanged(string SliderName, float value) { }

    /// <summary>
    /// InputField�����������ֵ�ı��¼�
    /// </summary>
    /// <param name="InputFieldName"></param>
    /// <param name="value"></param>
    protected virtual void InputField_OnValueChanged(string InputFieldName, string value) { }
    /// <summary>
    /// InputField������������ύ�¼�
    /// </summary>
    /// <param name="InputFieldName"></param>
    /// <param name="value"></param>
    protected virtual void InputField_onSubmit(string InputFieldName, string value) { }
    /// <summary>
    /// InputField���������������¼�
    /// </summary>
    /// <param name="InputFieldName"></param>
    /// <param name="value"></param>
    protected virtual void InputField_onEndEdit(string InputFieldName, string value) { }

    /// <summary>
    /// �ҵ���Ӧ��UI���������������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenUIComponent<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            string objName = controls[i].gameObject.name;
            if (controlDic.ContainsKey(objName))
            {
                controlDic[objName].Add(controls[i]);
            }
            else
            {
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            }
            //��Button��Toggle��Slider����������¼�
            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    Button_OnClick(objName);
                });
            }
            else if (controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((isSelect) =>
                {
                    Toggle_OnValueChanged(objName, isSelect);
                });
            }
            else if (controls[i] is Slider)
            {
                (controls[i] as Slider).onValueChanged.AddListener((value) =>
                {
                    Slider_OnValueChanged(objName, value);
                });
            }
            else if (controls[i] is InputField)
            {
                (controls[i] as InputField).onValueChanged.AddListener((value) =>
                {
                    InputField_OnValueChanged(objName, value);
                    InputField_onSubmit(objName, value);
                    InputField_onEndEdit(objName, value);
                });
            }
        }
    }
    /// <summary>
    /// ����������ض�������ֻ�ö�ӦUI���
    /// </summary>
    /// <typeparam name="T">���ض��������</typeparam>
    /// <param name="controlName">��ӦUI���</param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            for (int i = 0; i < controlDic[controlName].Count; i++)
            {
                if (controlDic[controlName][i] is T)
                {
                    return controlDic[controlName][i] as T;
                }
            }
        }
        return null;
    }
}
