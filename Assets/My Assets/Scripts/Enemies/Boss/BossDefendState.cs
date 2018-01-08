using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefendState : State<BossScript> {

    public BossDefendState(BossScript minion) : base(minion) { }

    public override void Tick()
    {

    }

    public override void OnStateEnter()
    {
        minion.EnterDefend();
    }

    public override void OnStateExit()
    {
        minion.ExitDefend();
    }

}
