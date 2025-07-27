using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI�㼶
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System
}

/// <summary>
/// UI������
/// 1.����������ʾ�����
/// 2.�ṩ���ⲿ ��ʾ�����صȵȽӿ�
/// </summary>
public class UIManager : BaseSingleton<UIManager>
{
    private const string UI_RESOURCES_PATH = "UI/";

    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;

    private Camera uiCamera;
    private Canvas uiCanvas;
    private EventSystem uiEventSystem;
    public RectTransform canvasRectTransform;

    public UIManager()
    {
        uiCamera = GameObject.Instantiate(ResMgr.Instance.Load<GameObject>(UI_RESOURCES_PATH + "UICamera")).GetComponent<Camera>();
        GameObject.DontDestroyOnLoad(uiCamera.gameObject);
        //����Canvas �����������ʱ�� �����Ƴ�
        GameObject obj = GameObject.Instantiate(ResMgr.Instance.Load<GameObject>(UI_RESOURCES_PATH + "Canvas"));
        uiCanvas = obj.GetComponent<Canvas>();
        uiCanvas.worldCamera = uiCamera;
        canvasRectTransform = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);



        //�ҵ�����
        bot = canvasRectTransform.Find("Bot");
        mid = canvasRectTransform.Find("Mid");
        top = canvasRectTransform.Find("Top");
        system = canvasRectTransform.Find("System");

        //����EventSystem �����������ʱ�� �����Ƴ�
        uiEventSystem = GameObject.Instantiate(ResMgr.Instance.Load<GameObject>("UI/EventSystem")).GetComponent<EventSystem>();
        GameObject.DontDestroyOnLoad(uiEventSystem.gameObject);
    }

    public void Init() { }

    /// <summary>
    /// ͨ���㼶ö�� �õ���Ӧ�㼶�ĸ�����
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return this.bot;
            case E_UI_Layer.Mid:
                return this.mid;
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.System:
                return this.system;
        }
        return null;
    }
    /// <summary>
    /// ��ʾ���ķ���
    /// ����ֵ����о�ֱ�ӵ��ø�����ShowMe������
    /// ���û�оʹ���Դ�ļ�������س����ٵ��ã����Ҵ����ֵ���
    /// </summary>
    /// <typeparam name="T">�����</typeparam>
    /// <param name="panelName">�������</param>
    /// <param name="layer">��ʾ���Ǹ���</param>
    /// <param name="callBack">UI�����ʾ��Ļص�</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Mid, UnityAction<T> callBack = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            // ������崴����ɺ���߼�
            if (callBack != null)
            {
                callBack(panelDic[panelName] as T);
            }
            //��������ظ����� ������ڸ���� ��ֱ����ʾ ���ûص�������  ֱ��return ���ٴ��������첽�����߼�
            return;
        }

        ResMgr.Instance.LoadAsync<GameObject>(UI_RESOURCES_PATH + panelName, (res) =>
        {
            //��UI�����Ϊ Canvas���Ӷ���
            //���� Ҫ�����������λ��
            //�ҵ������� ����ʾ�ڶ�Ӧ�Ĳ�
            Transform father = this.bot;
            switch (layer)
            {
                case E_UI_Layer.Mid:
                    father = this.mid;
                    break;
                case E_UI_Layer.Top:
                    father = this.top;
                    break;
                case E_UI_Layer.System:
                    father = this.system;
                    break;
            }
            //���ø�����  �������λ�úʹ�С
            res.transform.SetParent(father);
            res.transform.localPosition = Vector3.zero;
            res.transform.localScale = Vector3.one;
            (res.transform as RectTransform).offsetMax = Vector2.zero;
            (res.transform as RectTransform).offsetMin = Vector2.zero;
            //�õ�Ԥ�������ϵ����ű�
            T panel = res.GetComponent<T>();
            callBack?.Invoke(panel);

            panel.ShowMe();
            panelDic.Add(panelName, panel);
        });

    }
    /// <summary>
    /// �������
    /// ��ֱ�Ӱ�UI�����ɱ��
    /// �������Ļ�������GetPanel�õ�����ֱ�ӵ��ø�����HideMe����
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }
    /// <summary>
    /// �õ�ĳһ���Ѿ���ʾ����� �����ⲿʹ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public T GetPanel<T>(string panelName) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }

    /// <summary>
    /// ���ؼ�����Զ����¼�����
    /// </summary>
    /// <param name="control">�ؼ�����</param>
    /// <param name="type">�¼�����</param>
    /// <param name="callBack">�¼�����Ӧ����</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}
