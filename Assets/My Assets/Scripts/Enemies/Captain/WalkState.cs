using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State {

	public WalkState(CaptainScript minion) : base(minion) { }

    public override void Tick()
    {
        minion.CheckIfReachTarget();
    }

    public override void OnStateEnter()
    {
        minion.EnterWalk();
    }

    public override void OnStateExit()
    {
        minion.ExitWalk();
    }
}
