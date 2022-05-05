using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class TestBobber : MonoBehaviour
{
    TestBobberPool _testBobberpool = null;
    public Animator testBobberAnimator = null;


    private void Awake()
    {
        testBobberAnimator = GetComponent<Animator>();
    }

    public void AssignPool(TestBobberPool testBobberPool)
    {
        _testBobberpool = testBobberPool;
    }

    private void RemoveSelf()
    {
        if(_testBobberpool != null)
        {
            //return to Pool. instead of Destroy
            _testBobberpool.ReturnToPool(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
