using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PopUpGenerator : MonoBehaviour
{
    public enum PopupType
    {
        GameOverPopup,
        GameStartPopup,
        GameIntroducePopup
    }

    [SerializeField]
    protected Canvas Canvas;
    [SerializeField]
    protected GameObject panelPrefab;

    public static PopUpGenerator Instance
    {
        get
        {
            return instance;
        }
    }

    private static PopUpGenerator instance;

    private readonly Stack<GameObject> currentPanels = new Stack<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            throw new UnityException("已有实例：" + name);
        }
    }

    public void ClosePopup()
    {

        var topmostPanel = currentPanels.Pop();
        if (topmostPanel != null)
        {
            var seq = DOTween.Sequence();
            seq.Append(topmostPanel.GetComponent<Image>().DOFade(0.0f, 0.2f));
            seq.AppendCallback(() => Destroy(topmostPanel));
        }
    }


    public void OpenPopup<T>(string popupName, Action<T> onOpened = null, float delay = 0) where T : PopUp
    {
        string str = "Prefabs/";
        popupName = str + popupName;
        StartCoroutine(OpenPopupAsync(popupName, onOpened, delay));
    }

    IEnumerator OpenPopupAsync<T>(string popupName, Action<T> onOpened, float delay) where T : PopUp
    {
        yield return new WaitForSeconds(delay);

        GameObject obj = Instantiate(panelPrefab);
        obj.name = "panel";
        //obj.transform.SetParent(Canvas.transform);
        currentPanels.Push(obj);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        var request = Resources.LoadAsync<GameObject>(popupName);
        while (!request.isDone)
        {
            yield return null;
        }
        var popup = Instantiate(request.asset, Canvas.transform, false) as GameObject;
        Assert.IsNotNull(popup);
        popup.GetComponent<PopUp>().generator = this;
    }


}
