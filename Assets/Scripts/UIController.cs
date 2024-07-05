using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image _prizeIcon = null;
    [SerializeField] private TextMeshProUGUI _prizeDescription = null;
    [SerializeField] private GameObject _winScreen = null;
    [SerializeField] private Button _freeButton = null;
    [SerializeField] private Button _adsButton = null;
    [SerializeField] private WheelManager _wheelManager = null;

    private void Start()
    {
        _wheelManager.winAction += FillWinScreen;
        _freeButton.onClick.AddListener(FreeButonClick);
        _adsButton.onClick.AddListener(ADSButtonClick);
    }

    private void FreeButonClick()
    {
        _wheelManager.TurnWheel();
        _freeButton.gameObject.SetActive(false);
    }

    private void ADSButtonClick()
    {

        /*
          показ интерстишела 
         */

        Debug.Log("¬ы посмотрели рекламу");

        _wheelManager.TurnWheel();
        _winScreen.SetActive(false);
    }

    public void FillWinScreen(WheelPart prize)
    {
        _winScreen.SetActive(true);
        _prizeIcon.sprite = prize.Icon;
        _prizeDescription.text = prize.Description;
    }
}