using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State: MonoBehaviour
{
    public bool _isFacingRight;
    public abstract State RunState();
}
