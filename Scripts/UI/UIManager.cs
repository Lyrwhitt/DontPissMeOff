using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 현재 스크립트 인스턴스를 할당
        }
        else
        {
            Destroy(gameObject); // 이미 다른 인스턴스가 존재하면 현재 인스턴스 파괴
        }
    }

    private Canvas canvas;

    private void Start()
    {
        if(canvas == null)
        {
            Canvas obj = Resources.Load(string.Concat("UI/", "Canvas"), typeof(Canvas)) as Canvas;
            if (obj == null)
            {
                return;
            }
            else
                canvas = Instantiate(obj, this.transform, false);
        }
    }

    private List<UIPopup> popups = new List<UIPopup>();

    private UIPopup ShowPopup(string popupname)
    {
        GameObject obj = Resources.Load("UI/Popups/" + popupname, typeof(GameObject)) as GameObject;
        if (!obj)
        {
            return null;
        }
        return ShowPopupWithPrefab(obj, popupname);
    }

    public T ShowPopup<T>() where T : UIPopup
    {
        return ShowPopup(typeof(T).Name) as T;
    }

    public UIPopup ShowPopupWithPrefab(GameObject prefab, string popupName)
    {
        GameObject obj = Instantiate(prefab);
        obj.name = popupName;
        return ShowPopup(obj, popupName);
    }

    public UIPopup ShowPopup(GameObject obj, string popupname)
    {
        UIPopup popup = obj.GetComponent<UIPopup>();
        popup.SetPopupName(popupname);
        popups.Add(popup);

        obj.SetActive(true);
        obj.transform.SetParent(canvas.transform, false);

        return popup;
    }

    public void ClosAllPopup()
    {
        foreach (UIPopup popup in popups)
        {
            Destroy(popup.gameObject);
        }

        popups.Clear();
    }

    public void ClosePopup()
    {
        if(popups.Count > 0)
        {
            Destroy(popups[popups.Count - 1].gameObject);
            popups.RemoveAt(popups.Count - 1);
        }
    }

    public UIPopup GetPopup()
    {
        if (popups.Count == 0)
        {
            return null;
        }
        else
            return popups[popups.Count - 1];
    }

    public bool IsOpenedPopup(string popupname)
    {
        if (popups.Find(x => x.gameObject.name == popupname) != null)
        {
            return true;
        }
        else
            return false;
    }

    public int GetActivePopupCount()
    {
        return popups.Count;
    }
}