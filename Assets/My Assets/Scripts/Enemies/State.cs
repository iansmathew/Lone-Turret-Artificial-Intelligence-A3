using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected MinionScript minion;

    public abstract void Tick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(MinionScript _minion)
    {
        this.minion = _minion;
    }

}
