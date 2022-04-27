using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupBehaviour : MonoBehaviour
{
    protected PieceBehaviour _owningPiece;

    public void Awake()
    {
        _owningPiece = GetComponent<PieceBehaviour>();  
    }

    public abstract void ProcessPowerup();
}
