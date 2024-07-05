using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WheelManager : MonoBehaviour
{
    public System.Action<WheelPart> winAction;

    [SerializeField] private WheelPart wheelPartPrefab = null;
    [SerializeField] private bool _setNativeChance = false;
    [SerializeField] private bool _setRandomPrizes = false;
    [SerializeField] private List<WheelPartData> _wheelPartsData = null;
    [SerializeField] private List<PrizeData> _prizesData = null;
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

    private void OnValidate()
    {
        if (_setNativeChance)
        {
            SetNativeChance();
        }
        else
        {
            CalculateChance();
        }
    }
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
        float size = 1f / _wheelPartsData.Count;
        float angleIncrement = 360f / _wheelPartsData.Count;

        for (int i = 0; i < _wheelPartsData.Count; i++)
        {
            WheelPart tempWheelPart = Instantiate(wheelPartPrefab.gameObject, transform).GetComponent<WheelPart>();
            float angle = i * angleIncrement;

            _wheelPartsData[i].angle = angle;
            _wheelPartsData[i].size = size;

            tempWheelPart.GeneratePart(_wheelPartsData[i]);

            if (_wheelPartsData[i].prizeData == null || _setRandomPrizes)
            {
                tempWheelPart.UpdatePrizeData(GetRandomPrizeData(_wheelPartsData[i].forRepetPrizes));
            }
            else
            {
                tempWheelPart.UpdatePrizeData(_wheelPartsData[i].prizeData);
            }

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
            
            int prizeId = _prizeQueue[_rotationCount];
            bool prizeFound = false;

            for (int i = 0; i < _wheelParts.Count; i++)
            {
                if (_wheelParts[i].PartData.prizeData.ID == prizeId)
                {
                    _prizeNum = i;
                    prizeFound = true;
                }
            }

            if (_prizeNum >= _wheelParts.Count || !prizeFound)
            {
                _prizeNum = Random.Range(0, _wheelParts.Count);
                _rotationCount--;
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

    private void CalculateChance()
    {
        float difference = 0;

        List<WheelPartData> partsData = new List<WheelPartData>();

        if (_wheelParts.Count > 0)
        {
            foreach (var part in _wheelParts)
            {
                partsData.Add(part.PartData);
            }
        }
        else
        {
            partsData = _wheelPartsData;
        }

        WheelPartData tempPartData = null;

        foreach (var part in partsData)
        {
            difference = part.prizeData.chance - part.prizeData.hashChance;

            if (difference != 0)
            {
                part.prizeData.hashChance = part.prizeData.chance;
                tempPartData = part;
                break;
            }
        }

        if (difference != 0)
        {
            difference /= partsData.Count;
            foreach (var part in partsData)
            {
                if (part == tempPartData)
                {
                    continue;
                }
                else
                {
                    part.prizeData.chance -= difference;
                    part.prizeData.hashChance = part.prizeData.chance;
                }
            }

        }
    }

    private void SetNativeChance()
    {
        float chance = 0;
        List<WheelPartData> partsData = new List<WheelPartData>();

        if (_wheelParts.Count > 0)
        {
            chance = 100f / _wheelParts.Count;
            foreach (var part in _wheelParts)
            {
                partsData.Add(part.PartData);
            }
        }
        else
        {
            chance = 100f / _wheelPartsData.Count;
            partsData = _wheelPartsData;
        }

        foreach (var part in partsData)
        {
            part.prizeData.chance = chance;
            part.prizeData.hashChance = chance;
        }
    }

    private void Win()
    {
        _wheelStart = false;
        winAction.Invoke(_wheelParts[_prizeNum]);
        _wheelParts[_prizeNum].PartData.prizeData.isWin = true;

        PrizeData newPrize = GetRandomPrizeData(_wheelParts[_prizeNum].PartData.forRepetPrizes);

        if (newPrize != null)
        {
            PrizeData tempPrize = _wheelParts[_prizeNum].PartData.prizeData;

            if (!_prizesData.Contains(tempPrize))
            {
                _prizesData.Add(tempPrize);
            }

            _wheelParts[_prizeNum].UpdatePrizeData(newPrize);
        }

        if (_setNativeChance)
        {
            SetNativeChance();
        }
        else
        {
            CalculateChance();
        }
    }

    private PrizeData GetRandomPrizeData(bool repetPrize)
    {
        List<PrizeData> tempPrizesData = new List<PrizeData>();

        foreach (var prize in _prizesData)
        {
            if (prize.repeatPrize && repetPrize)
            {
                tempPrizesData.Add(prize);
            }
            else if (!prize.repeatPrize && !prize.isWin)
            {
                tempPrizesData.Add(prize);
            }
        }

        if (tempPrizesData.Count > 0)
        {
            PrizeData newPrize = tempPrizesData[Random.Range(0, tempPrizesData.Count)];

            _prizesData.Remove(newPrize);
            return newPrize;
        }
        else
        {
            if (!repetPrize)
            {
                return GetRandomPrizeData(true);
            }

            return null;
        }
    }
}