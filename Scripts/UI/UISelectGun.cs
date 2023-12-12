using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UISelectGun : MonoBehaviour
{
    private GameUIController _gameUIController;

    private PlayerInput _playerInput;

    public bool isShowingUI = false;

    private RectTransform _rectTransform;

    private int _currentWeaponIdx = 0;

    public List<UIWeapon> weapons;

    private void Awake()
    {
        _gameUIController = GetComponentInParent<GameUIController>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _playerInput = _gameUIController.player.GetComponent<PlayerInput>();

        _playerInput.uiActions.ShowGun.started += OnShowGunButtonDown;
        _playerInput.uiActions.ShowGun.canceled += OnShowGunButtonUp;
        _playerInput.uiActions.SelectGunNumberKey.started += OnShowGunNumberButtonDown;

        AddToggleEvents();

        weapons[_currentWeaponIdx].toggle.isOn = true;
        GameManager.Instance.onRetry += OnRetry;
        //GameManager.Instance.onResume += OnResume;
    }

    private void Update()
    {
        if (!isShowingUI || GameManager.Instance.isPause)
            return;

        float value = _playerInput.uiActions.SelectGun.ReadValue<float>();

        if (value != 0f)
        {
            ChangeWeapon(value);
        }
    }

    private void AddToggleEvents()
    {
        foreach(UIWeapon weapon in weapons)
        {
            weapon.toggle.onValueChanged.AddListener((bool isOn) =>
            {
                RectTransform weaponRect = weapon.GetComponent<RectTransform>();

                if (isOn)
                {
                    weapon.iconImg.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutQuad).SetUpdate(true);
                    weapon.toggleImg.sprite = weapon.toggleOnSprite;
                }
                else
                {
                    weapon.iconImg.transform.DOScale(1.0f, 0.2f).SetEase(Ease.OutQuad).SetUpdate(true);
                    weapon.toggleImg.sprite = weapon.toggleOffSprite;
                }
            });
        }
    }

    private void DisableAllWeaponToggles() //���� ȹ��� �ٸ� ����UI��� ��Ȱ��ȭ�ϴµ� ���
    {
        foreach (UIWeapon weapon in weapons)
        {
            weapon.toggle.isOn = false;
        }
    }

    public void ChangeWeapon(float wheelValue)
    {
        UIWeapon changeWeapon = weapons[_currentWeaponIdx];

        if (wheelValue > 0.1f)
        {
            changeWeapon = GetRightWeapon();
        }
        else if (wheelValue < -0.1f)
        {
            changeWeapon = GetLeftWeapon();
        }
        changeWeapon.toggle.isOn = true;
    }

    public UIWeapon GetCurrentSelectedWeapon() 
    {
        return weapons[_currentWeaponIdx];
    }

    // ���콺 �� �÷����� ���������� ���⼱��
    private UIWeapon GetRightWeapon()
    {
        if (_currentWeaponIdx == weapons.Count - 1)
            return weapons[_currentWeaponIdx];

        for (int i = _currentWeaponIdx + 1; i < weapons.Count; i++)
        {
            if (weapons[i].gameObject.activeSelf && weapons[i].isUnlocked) // ����� �κ�
            {
                _currentWeaponIdx = i;
                return weapons[i];
            }
        }
        return weapons[_currentWeaponIdx];
    }

    // ���콺 �� �÷����� �������� ���⼱��
    public UIWeapon GetLeftWeapon()
    {
        if (_currentWeaponIdx == 0)
            return weapons[_currentWeaponIdx];

        for (int i = _currentWeaponIdx - 1; i >= 0; i--)
        {
            if (weapons[i].gameObject.activeSelf && weapons[i].isUnlocked) // ����� �κ�
            {
                _currentWeaponIdx = i;
                return weapons[i];
            }
        }
        return weapons[_currentWeaponIdx];
    }

    public void OnShowGunButtonDown(InputAction.CallbackContext context)
    {
        isShowingUI = true;

        _rectTransform.DOAnchorPosY(0, 0.4f).SetEase(Ease.OutQuad).SetUpdate(true);
    }
    public void OnShowGunButtonUp(InputAction.CallbackContext context)
    {
        isShowingUI = false;

        _rectTransform.DOAnchorPosY(-225, 0.4f).SetEase(Ease.OutQuad).SetUpdate(true);

        SetChangedWeapon();
    }
    public void OnShowGunNumberButtonDown(InputAction.CallbackContext context)
    {
        _currentWeaponIdx = (int)_playerInput.uiActions.SelectGunNumberKey.ReadValue<float>();

        if (_currentWeaponIdx >= weapons.Count)
            return;

        if (weapons[_currentWeaponIdx].gameObject.activeSelf && weapons[_currentWeaponIdx].isUnlocked)
        {
            ChangeWeapon(0f);
            SetChangedWeapon();
        }      
    }

    public void SetChangedWeapon()
    {
        _gameUIController.player.ChangeWeapon(GetCurrentSelectedWeapon().gunType);
        _gameUIController.playerStatusUI.UpdateBulletState(_gameUIController.player.currentWeapon.GetCurrentAmmo());
        _gameUIController.playerStatusUI.ChangeWeaponImg(GetCurrentSelectedWeapon().gunType);
        _gameUIController.crossHairUI.ChangeCrossHair(GetCurrentSelectedWeapon().gunType);
    }

    public void UnlockWeapon(GunType gunType)
    {
        UIWeapon weaponToUnlock = weapons.Find(weapon => weapon.gunType == gunType);
        if (weaponToUnlock != null)
        {
            DisableAllWeaponToggles();
            weaponToUnlock.toggle.isOn = true;
            weaponToUnlock.isUnlocked = true;
            weaponToUnlock.iconImg.enabled = true; // �ѱ� ��� ���� �� �ش� �ѱ��� UI �̹����� Ȱ��ȭ�մϴ�.
            weaponToUnlock.gameObject.SetActive(true);

            // ���⸦ ���ʷ� ����� �� �ش� ����� �ڵ����� ��ü
            _currentWeaponIdx = weapons.IndexOf(weaponToUnlock);
            SetChangedWeapon();

            if (!GameManager.Instance.saveData.UnlockedWeapons.Contains(gunType))
                GameManager.Instance.saveData.unlockedWeapons.Add(gunType);
        }
    }
    private void OnRetry()
    {
        SaveData saveData = GameManager.Instance.LoadLastData();

        if (saveData.UnlockedWeapons.Count != 0)
        {
            for (int i = 0; i < saveData.UnlockedWeapons.Count; i++)
            {
                UnlockWeapon(saveData.UnlockedWeapons[i]);

            }
        }
    }
}
