using UnityEngine;

public class UIPopup : MonoBehaviour
{
    private string _popupName;

    public void SetPopupName(string name)
    {
        _popupName = name;
    }
    public string GetPopupName()
    {
        return _popupName;
    }
}
