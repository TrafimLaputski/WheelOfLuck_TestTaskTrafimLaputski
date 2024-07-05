using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Prize icon on the victory screent")]
    [SerializeField] private Image _prizeIcon = null;

    [Tooltip("Prize description on the victory screen")]
    [SerializeField] private TextMeshProUGUI _prizeDescription = null;

    [Tooltip("Victory screen")]
    [SerializeField] private GameObject _winScreen = null;

    [Tooltip("Free button at the start of the game")]
    [SerializeField] private Button _freeButton = null;

    [Tooltip("Paid button at the end of the game")]
    [SerializeField] private Button _adsButton = null;

    [Tooltip("Wheel Managere")]
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