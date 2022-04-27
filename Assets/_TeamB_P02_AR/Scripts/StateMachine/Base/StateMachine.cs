using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateMachine : MonoBehaviour
{
    public State CurrentState => currentState;
    State currentState;

    protected State previousState;

    protected bool InTransition { get; private set; }

    public event Action<State> ChangeStateEvent;
    public void ChangeState<T>() where T : State
    {
        T targetState = GetComponent<T>();
        if (targetState == null)
        {
            Debug.LogWarning("Cannot change to state. State does not exist on State Machine Object.");
            return;
        }
        InitiateStateChange(targetState);
    }

    public void RevertState()
    {
        if (previousState != null)
        {
            InitiateStateChange(previousState);
        }
    }

    private void InitiateStateChange(State targetState)
    {
        if (currentState != targetState && !InTransition)
        {
            Transition(targetState);
        }
        else if (currentState == targetState)
        {
            Debug.LogWarning("Attempted to transition from " + currentState.GetType().Name + " to itself.");
        }
        else if (InTransition)
        {
            Debug.LogWarning("Attempted to transition from " + currentState.GetType().Name + " to " + targetState.GetType().Name + " while in transition.");
        }
    }

    void Transition(State newState)
    {
        InTransition = true;

        currentState?.Exit();
        currentState = newState;
        ChangeStateEvent?.Invoke(newState);
        currentState?.Enter();

        InTransition = false;
    }

    private void Update()
    {
        if (CurrentState != null && !InTransition)
        {
            CurrentState.Tick();
        }
    }
}