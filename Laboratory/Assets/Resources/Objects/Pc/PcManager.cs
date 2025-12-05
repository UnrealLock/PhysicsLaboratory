using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PcManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Camera PcCamera;
    GameObject app;
    bool isOpen = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenApp()
    {
        if (isOpen || !PcCamera.isActiveAndEnabled) return;
        app = Resources.Load<GameObject>("Objects/Pc/App/AppWindow");
        var objectsData = new List<RectTransformData>();
        for (int i = 0; i < app.transform.childCount; i++)
        {
            objectsData.Add(new RectTransformData());
            objectsData[i].CopyRectTransform(app.transform.GetChild(i));
        }
        app = Instantiate(app, new Vector3(0, 0, -0.05f), Quaternion.identity);
        app.GetComponent<MoveAppScreen>().MainCamera = PcCamera;
        var screen = GameObject.Find("Screen");
        app.transform.SetParent(screen.transform);
        app.transform.localPosition = new Vector3(0, 0, -0.05f);
        app.transform.localScale = new Vector3(60, 60, 1);
        for (int i = 0; i < app.transform.childCount; i++)
        {
            objectsData[i].PasteRectTransform(app.transform.GetChild(i));
            if (i == 0)
                app.transform.GetChild(i).localPosition = new Vector3(0, 0.02f, 0.01f);
            else
                app.transform.GetChild(i).localPosition = new Vector3(0, 0, -0.01f);
        }
        SetFrameButtons();
        SetGraphicPageButtons();
        SetResultPageButtons();
        isOpen = true;
    }

    void SetGraphicPageButtons()
    {
        var graphicPage = app.transform.GetChild(1);
        var appScript = app.GetComponent<AppScript>();
    }

    void SetResultPageButtons()
    {
        var resultPage = app.transform.GetChild(2);
        var appScript = app.GetComponent<AppScript>();
    }

    void SetFrameButtons()
    {
        var buttons = app.transform.GetChild(0).transform.GetChild(0);
        var fullScreenButton = buttons.GetChild(1).GetComponent<UnityEngine.UI.Button>();
        var hideButton = buttons.GetChild(2).GetComponent<UnityEngine.UI.Button>();
        var closeButton = buttons.GetChild(0).GetComponent<UnityEngine.UI.Button>();
        buttons.localPosition = Vector3.zero;
        buttons.localScale = new Vector3(1, 1, 1);
        buttons.GetChild(0).transform.localPosition = new Vector3(0.4695f, 0.4781f, -0.02f);
        buttons.GetChild(1).transform.localPosition = new Vector3(0.4205f, 0.4781f, -0.02f);
        buttons.GetChild(2).transform.localPosition = new Vector3(0.3707f, 0.4781f, -0.02f);
        fullScreenButton.onClick.AddListener(MakeFullScreen);
        hideButton.onClick.AddListener(HideApp);
        closeButton.onClick.AddListener(CloseApp);
    }
    public void CloseApp()
    {
        Destroy(app);
        app = null;
        isOpen = false;
    }

    public void HideApp()
    {
        app.gameObject.SetActive(false);
        var screen = GameObject.Find("Screen");
        var showButton = screen.transform.GetChild(1);
        showButton.gameObject.SetActive(true);
    }

    public void MakeFullScreen()
    {
        app.transform.localScale = new Vector3(100, 95, 1);
        app.transform.localPosition = new Vector3(0, -1, -0.02f);
        var buttons = app.transform.GetChild(0).transform.GetChild(0);
        var fullScreenButton = buttons.GetChild(1).GetComponent<UnityEngine.UI.Button>();
        app.GetComponent<MoveAppScreen>().CanBeDriven = false;
        fullScreenButton.onClick.RemoveAllListeners();
        fullScreenButton.onClick.AddListener(MakeNormal);
    }

    public void ShowApp() {
        app.gameObject.SetActive(true);
        var screen = GameObject.Find("Screen");
        var showButton = screen.transform.GetChild(1);
        showButton.gameObject.SetActive(false);
    }

    public void MakeNormal()
    {
        app.transform.localScale = new Vector3(60, 60, 1);
        app.transform.localPosition = new Vector3(0, 0, -0.02f);
        var buttons = app.transform.GetChild(0).transform.GetChild(0);
        var fullScreenButton = buttons.GetChild(1).GetComponent<UnityEngine.UI.Button>();
        app.GetComponent<MoveAppScreen>().CanBeDriven = true;
        fullScreenButton.onClick.RemoveAllListeners();
        fullScreenButton.onClick.AddListener(MakeFullScreen);
    }
}

class RectTransformData
{
    Vector2 anchorMin;
    Vector2 anchorMax;
    Vector2 anchoredPosition;
    Vector2 sizeDelta;
    Vector2 pivot;
    Vector3 localScale;

    public void CopyRectTransform(Transform objectTransform)
    {
        var rectTransform = objectTransform.GetComponent<RectTransform>();
        anchorMin = rectTransform.anchorMin;
        anchorMax = rectTransform.anchorMax;
        anchoredPosition = rectTransform.anchoredPosition;
        sizeDelta = rectTransform.sizeDelta;
        pivot = rectTransform.pivot;
        localScale = rectTransform.localScale;
    }

    public void PasteRectTransform(Transform objectTransform)
    {
        var rectTransform = objectTransform.GetComponent<RectTransform>();
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.pivot = pivot;
        rectTransform.localScale = localScale;
        rectTransform.localPosition = new Vector3(0, 0, 0);
    }
}
