using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelPartData {

    public Sprite DefaultIcon = null;
    public string DefaultDescription = null;
    public Color DefaultColor = Color.green;

    [HideInInspector] public float angle = 0f;
    [HideInInspector] public float size = 0f;
}
