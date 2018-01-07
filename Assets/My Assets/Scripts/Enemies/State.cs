using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected CaptainScript minion;

    public abstract void Tick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(CaptainScript _minion)
    {
        this.minion = _minion;
    }

}
