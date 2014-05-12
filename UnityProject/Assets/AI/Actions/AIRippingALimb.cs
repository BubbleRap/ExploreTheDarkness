using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

[RAINAction]
public class AIRippingALimb : RAINAction
{
	public Expression characterForm;
	private SiljaBehaviour silja = null;
	private GameObject siljaGO = null;

    public AIRippingALimb()
    {
        actionName = "AIRippingALimb";
    }

    public override void Start(AI ai)
    {
		siljaGO = characterForm.Evaluate (ai.DeltaTime, ai.WorkingMemory).GetValue<GameObject> ();
		if (siljaGO != null)
			silja = siljaGO.GetComponent<SiljaBehaviour> ();
		else
			Debug.LogWarning ("Silja hasnt been found");

        base.Start(ai);

    }

    public override ActionResult Execute(AI ai)
    {
		if (silja == null)
			return ActionResult.FAILURE;

		silja.TakeALimb (ai.Body.transform);

		NavigationTarget spawnPoint = NavigationManager.instance.GetNavigationTarget(spawnPoints[ Random.Range(0, spawnPoints.Length) ]);
		GameObject.FindObjectOfType<AIBehaviour>().transform.position = spawnPoint.Position;

        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}