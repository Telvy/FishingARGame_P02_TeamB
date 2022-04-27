using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoState : State
{
    protected FishGoSM stateMachine { get; private set; }

    private void Awake()
    {
        stateMachine = GetComponent<FishGoSM>();
    }
}
