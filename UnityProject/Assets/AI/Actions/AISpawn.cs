using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;
using RAIN.Navigation;
using RAIN.Motion;


[RAINAction]
public class AISpawn : RAINAction
{
	public Expression navigationTarget;
	private Vector3 targetPosition;

    public AISpawn()
    {
        actionName = "AISpawn";
    }

    public override void Start(AI ai)
    {
		targetPosition = navigationTarget.Evaluate (ai.DeltaTime, ai.WorkingMemory).GetValue<Vector3>();
	
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		//ai.Motor.MoveTo( targetPosition );
		ai.Body.transform.position = targetPosition;
        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}