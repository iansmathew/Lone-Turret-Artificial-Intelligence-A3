using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkState : State<BossScript> {

    public BossWalkState(BossScript minion) : base(minion) { }

    public override void Tick()
    {
        minion.CheckIfReachTarget();
        minion.CheckIfNeedDefense();
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
