using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class WheelManager : MonoBehaviour
{
    [SerializeField] private WheelPart wheelPartPrefab = null;
    [SerializeField] private WheelPartData[] _wheelPartsData = null;
    [SerializeField] private AnimationCurve _wheelCurve = null;

    private List<WheelPart> _wheelParts = new List<WheelPart>();

    private bool _wheelStart = false;

    private float _startAngle;
    private float _endAngle;
    private int _randomRewardIndex = 0;
    private float _currentRotationTime;
    private float _maxRotationTime;

    private float _ta = 90;
    void Start()
    {
        GenerateWheel();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TurnWheel();
        }

        if (_wheelStart)
        {
            float t = _currentRotationTime / _maxRotationTime;
            t = _wheelCurve.Evaluate(t);

            float angle = Mathf.Lerp(_startAngle, _endAngle, t);
            transform.eulerAngles = new Vector3(0, 0, angle);

            if (angle <= _endAngle)
            {
                _wheelStart = false;
            }

            _currentRotationTime += Time.deltaTime;
        }
    }

    private void GenerateWheel()
    {
        float size = 1f / _wheelPartsData.Length;
        float angleIncrement = 360f / _wheelPartsData.Length;

        for (int i = 0; i < _wheelPartsData.Length; i++)
        {
            WheelPart tempWheelPart = Instantiate(wheelPartPrefab.gameObject, transform).GetComponent<WheelPart>();
            float angle = i * angleIncrement;

            _wheelPartsData[i].angle = angle;
            _wheelPartsData[i].size = size;

            tempWheelPart.GeneratePart(_wheelPartsData[i]);

            _wheelParts.Add(tempWheelPart);
        }
    }

    public void TurnWheel()
    {
        if (_wheelStart)
            return;

        _wheelStart = true;
        _startAngle = transform.localEulerAngles.z;
        int totalSlots = _wheelParts.Count;
        _randomRewardIndex = Random.Range(0, totalSlots);

        int rotationCount = Random.Range(10, 15);
        _endAngle = -(rotationCount * 360 + _ta);

        _currentRotationTime = 0.0f;
        _maxRotationTime = Random.Range(5.0f, 9.0f);

    }
}