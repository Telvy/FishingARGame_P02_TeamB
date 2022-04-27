using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoSM : StateMachine
{
    private void Start()
    {
        ChangeState<StartState>();
    }
}
