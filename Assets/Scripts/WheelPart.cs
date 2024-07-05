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

    private float _angle = 0;
    private WheelPartData _partData = null;

    public float Angle
    {
        set { _angle = value; }
    }

    public void GeneratePart(WheelPartData partData)
    {
        _partData = partData;

        _angle = partData.angle;
        _image.color = partData.DefaultColor;
        _image.fillAmount = partData.size;

        _icon.sprite = partData.DefaultIcon;
        _descriptionText.text = partData.DefaultDescription;

        float informationAngle = ((1f - partData.size) * 360f / 2f) + 270;
        _informationPoint.transform.rotation = Quaternion.Euler(0f, 0f, informationAngle);

        transform.rotation = Quaternion.Euler(0f, 0f, _angle);

        Debug.Log(_angle);
    }
}