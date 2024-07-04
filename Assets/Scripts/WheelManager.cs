using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WheelManager : MonoBehaviour
{
    [SerializeField] private WheelPart wheelPartPrefab = null;
    [SerializeField] private WheelPartData[] _wheelParts = null;


    // Start is called before the first frame update
    void Start()
    {
        GenerateWheel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateWheel()
    {
        float size = 1f / _wheelParts.Length;
        float angleIncrement = 360f / _wheelParts.Length;

        for (int i = 0; i < _wheelParts.Length; i++)
        {
            WheelPart tempWheelPart = Instantiate(wheelPartPrefab.gameObject, transform).GetComponent<WheelPart>();
            float angle = i * angleIncrement;

            _wheelParts[i].angle = angle;
            _wheelParts[i].size = size;

            tempWheelPart.GeneratePart(_wheelParts[i]);
        }
    }
}
