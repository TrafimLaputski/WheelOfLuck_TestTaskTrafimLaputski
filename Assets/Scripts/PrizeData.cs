using UnityEngine;

[System.Serializable]
public class PrizeData
{
    public int ID = 0;
    public Sprite DefaultIcon = null;
    public string DefaultDescription = null;
    public bool repeatPrize = true;

    [Range(0, 100)] public float chance = 50;
    [HideInInspector] public float hashChance = 0;
    [HideInInspector] public bool isWin = false;
}
