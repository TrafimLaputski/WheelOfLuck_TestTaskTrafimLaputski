using UnityEngine;

[System.Serializable]
public class WheelPartData {

    public bool forRepetPrizes = false;
    public PrizeData prizeData = null;
    public Color DefaultColor = Color.green;
    [HideInInspector] public float angle = 0f;
    [HideInInspector] public float size = 0f;
}