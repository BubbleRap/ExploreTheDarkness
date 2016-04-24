using UnityEngine;
using System.Collections;
using System;

public class LightStatesMachine : MonoBehaviour {

    public static LightStatesMachine Instance { get; private set;  }

    public AbstractLightState State;

    private LightStateOn stateOn;
    private LightStateOff stateOff;

    void Awake()
    {
        Instance = this;

        stateOn = GetComponentInChildren<LightStateOn>();
        stateOff = GetComponentInChildren<LightStateOff>();
    }

    void Start()
    {
        if (State != null)
            State.OnEnter();
    }

    public void ChangeToOff()
    {
        ChangeState(stateOff);
    }

    public void ChangeToOn()
    {
        ChangeState(stateOn);
    }

    public bool IsLightOn()
    {
        return State is LightStateOn;
    }

    public void ChangeState(AbstractLightState state)
    {
        if (State != null)
            State.OnExit();

        State = state;

        if (State != null)
            State.OnEnter();
    }

    void Update()
    {
        if (State != null)
            State.OnUpdate();
    }
    
}
