using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detector : MonoBehaviour
{
    public bool roaming = false;
    public abstract bool Detect(AIData data);
}
