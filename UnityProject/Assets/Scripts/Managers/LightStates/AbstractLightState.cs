using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public abstract class AbstractLightState : MonoBehaviour {

    public UnityEvent OnEnterEvents;
    public UnityEvent OnExitEvents;
    public UnityEvent OnUpdateEvents;

    public virtual void OnEnter()
    {
        OnEnterEvents.Invoke();
    }

    public virtual void OnExit()
    {
        OnExitEvents.Invoke();
    }

    public virtual void OnUpdate()
    {
        OnUpdateEvents.Invoke();
    }
}
