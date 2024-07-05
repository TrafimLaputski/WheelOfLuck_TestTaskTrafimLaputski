using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelPartData {

    public int ID = 0;
    public Sprite DefaultIcon = null;
    public string DefaultDescription = null;
    public Color DefaultColor = Color.green;
    [Range(0, 100)] public float chance = 50;
    [HideInInspector] public float angle = 0f;
    [HideInInspector] public float size = 0f;
    [HideInInspector] public float oldChance = 50;
}
