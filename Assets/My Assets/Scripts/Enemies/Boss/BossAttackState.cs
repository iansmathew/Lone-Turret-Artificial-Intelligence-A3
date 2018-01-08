using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : State<BossScript> {

    public BossAttackState(BossScript minion) : base(minion) { }

    public override void Tick()
    {
        minion.Attack();
        //minion.CheckIfNeedDefense();
    }

    public override void OnStateEnter()
    {
        minion.EnterAttack();
    }

    public override void OnStateExit()
    {
        minion.ExitAttack();
    }
}
