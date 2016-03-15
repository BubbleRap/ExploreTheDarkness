using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Navigation.Waypoints;

[RAINAction]
public class MoveAction : RAINAction
{
    private CharacterBehaviour m_character;
    private WaypointRig waypointsNetwork;

    public override void Start(RAIN.Core.AI ai)
    {
        m_character = ai.Body.GetComponent<CharacterBehaviour>();
       


        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        //MoveCharacterTowards(dirToChar, Vector2.up);
        //RotateCharacterTowards(dirToChar);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}