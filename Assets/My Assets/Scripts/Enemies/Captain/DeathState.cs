using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State<CaptainScript> {

    public DeathState(CaptainScript minion) : base(minion) { }

    public override void Tick()
    {
        
    }

    public override void OnStateEnter()
    {
        minion.EnterDeath();
    }

    public override void OnStateExit()
    {
        
    }
}
