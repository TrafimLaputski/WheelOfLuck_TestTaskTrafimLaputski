using System.Collections.Generic;
using UnityEngine;

public class WheelManager : MonoBehaviour
{
    public System.Action<WheelPart> winAction;

    [SerializeField] private WheelPart wheelPartPrefab = null;
    [SerializeField] private WheelPartData[] _wheelPartsData = null;
    [SerializeField] private AnimationCurve _wheelCurve = null;

    [SerializeField] private int[] _prizeQueue = null;
    private List<WheelPart> _wheelParts = new List<WheelPart>();

    private bool _wheelStart = false;

    private float _startAngle;
    private float _endAngle;
    private int _randomRewardIndex = 0;
    private float _currentRotationTime;
    private float _maxRotationTime;
    
    private int _rotationCount = 0;
    private int _prizeNum = 0;

    private void Start()
    {
        GenerateWheel();
    }

    private void Update()
    {
        if (_wheelStart)
        {
            float t = _currentRotationTime / _maxRotationTime;
            t = _wheelCurve.Evaluate(t);

            float angle = Mathf.Lerp(_startAngle, _endAngle, t);
            transform.eulerAngles = new Vector3(0, 0, angle);

            if (angle <= _endAngle)
            {
                Win();
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

        int rotateCount = Random.Range(10, 15);

        if (_prizeQueue.Length > 0 && _prizeQueue.Length > _rotationCount)
        {
            _prizeNum = _prizeQueue[_rotationCount];

            if (_prizeNum >= _wheelParts.Count)
            {
                _prizeNum = Random.Range(0, _wheelParts.Count);
            }
        }
        else
        {
            _prizeNum = Random.Range(0, _wheelParts.Count);
        }

        float extraAngle = Random.Range(1, _wheelParts[_prizeNum].Size * 360);
        _endAngle = -(rotateCount * 360 + _wheelParts[_prizeNum].Angle - extraAngle);

        _currentRotationTime = 0.0f;
        _maxRotationTime = Random.Range(5.0f, 9.0f);

        _rotationCount++;
    }

    private void Win()
    {
        _wheelStart = false;
        winAction.Invoke(_wheelParts[_prizeNum]);
    }
}