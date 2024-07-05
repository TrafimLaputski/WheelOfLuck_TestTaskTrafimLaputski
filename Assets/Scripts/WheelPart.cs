using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelPart : MonoBehaviour
{
    [SerializeField] private Image _image = null;
    [SerializeField] private Image _icon = null;
    [SerializeField] private TextMeshProUGUI _descriptionText = null;
    [SerializeField] private GameObject _informationPoint = null;

    private WheelPartData _partData = null;

    public int ID
    {
        get { return _partData.prizeData.ID; }
    }

    public float Angle
    {
        get { return _partData.angle; }
    }

    public float Size
    {
        get { return _partData.size; }
    }

    public WheelPartData PartData 
    { 
        get { return _partData; } 
    }

    public Sprite Icon
    {
        get { return _partData.prizeData.DefaultIcon; }
    }


    public string Description
    {
        get { return _partData.prizeData.DefaultDescription; }
    }

    public void GeneratePart(WheelPartData partData)
    {
        _partData = partData;

        _image.color = partData.DefaultColor;
        _image.fillAmount = partData.size;

        float informationAngle = ((1f - partData.size) * 360f / 2f) + 270;
        _informationPoint.transform.rotation = Quaternion.Euler(0f, 0f, informationAngle);

        transform.rotation = Quaternion.Euler(0f, 0f, partData.angle);
    }

    public void UpdatePrizeData(PrizeData prizeData)
    {
        PartData.prizeData = prizeData;
        _icon.sprite = prizeData.DefaultIcon;
        _descriptionText.text = prizeData.DefaultDescription;
    }
}