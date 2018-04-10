using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GolfBase
{

    public string tagName = "Untagged";
    public int strokes = 0;
    public float minPower = 1f;
    public float maxPower = 15f;
    public bool isMyTurn = false;
}
