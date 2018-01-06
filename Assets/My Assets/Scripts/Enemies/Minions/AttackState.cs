using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public AttackState(MinionScript minion) : base(minion) { }

    public override void Tick()
    {
        minion.FireGun();
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
