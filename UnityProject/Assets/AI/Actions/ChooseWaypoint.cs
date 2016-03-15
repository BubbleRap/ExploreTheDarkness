using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using RAIN.Motion;
using RAIN.Navigation;
using RAIN.Navigation.Graph;
using RAIN.Navigation.Waypoints;

[RAINAction("Choose Next Waypoint")]
public class ChooseWaypoint : RAINAction
{
    public Expression WaypointNetwork = new Expression(); //either the name of the network or a variable containing a network
    public Expression MoveTargetVariable = new Expression(); //the variable you want to use for the output move target

    private MoveLookTarget moveTarget = new MoveLookTarget();
    private int lastWaypoint = -1;
    private WaypointSet lastWaypointSet = null;

    public override ActionResult Execute(AI ai)
    {
        if (!MoveTargetVariable.IsValid || !MoveTargetVariable.IsVariable)
            return ActionResult.FAILURE;

        WaypointSet waypointSet = GetWaypointSetFromExpression(ai);
        if (waypointSet == null)
            return ActionResult.FAILURE;

        if (waypointSet != lastWaypointSet)
        {
            lastWaypoint = -1;
            lastWaypointSet = waypointSet;
        }

        if (lastWaypoint == -1)
        {
            lastWaypoint = waypointSet.GetClosestWaypointIndex(ai.Kinematic.Position);
            if (lastWaypoint < 0)
                return ActionResult.FAILURE;

            moveTarget.VectorTarget = waypointSet.Waypoints[lastWaypoint].position;
            moveTarget.CloseEnoughDistance = Mathf.Max(waypointSet.Waypoints[lastWaypoint].range, ai.Motor.CloseEnoughDistance);
            if (!ai.Motor.IsAt(moveTarget))
            {
                ai.WorkingMemory.SetItem<MoveLookTarget>(MoveTargetVariable.VariableName, moveTarget);
                return ActionResult.SUCCESS;
            }
        }

        //**REMOVED EXTRA LINE HERE
        NavigationGraphNode tNode = waypointSet.Graph.GetNode(lastWaypoint);
        if (tNode.OutEdgeCount > 0)
        {
            int tRandomEdge = UnityEngine.Random.Range(0, tNode.OutEdgeCount); //**FIXED THIS LINE
            lastWaypoint = ((VectorPathNode)tNode.EdgeOut(tRandomEdge).ToNode).NodeIndex;
        }
        moveTarget.VectorTarget = waypointSet.Waypoints[lastWaypoint].position;
        moveTarget.CloseEnoughDistance = Mathf.Max(waypointSet.Waypoints[lastWaypoint].range, ai.Motor.CloseEnoughDistance);
        ai.WorkingMemory.SetItem<MoveLookTarget>(MoveTargetVariable.VariableName, moveTarget);

        return ActionResult.SUCCESS;
    }

    private WaypointSet GetWaypointSetFromExpression(AI ai)
    {
        WaypointSet waypointSet = null;

        if (WaypointNetwork != null && WaypointNetwork.IsValid)
        {
            if (WaypointNetwork.IsVariable)
            {
                string varName = WaypointNetwork.VariableName;
                if (ai.WorkingMemory.ItemExists(varName))
                {
                    Type t = ai.WorkingMemory.GetItemType(varName);
                    if (t == typeof(WaypointRig) || t.IsSubclassOf(typeof(WaypointRig)))
                    {
                        WaypointRig wgComp = ai.WorkingMemory.GetItem<WaypointRig>(varName);
                        if (wgComp != null)
                            waypointSet = wgComp.WaypointSet;
                    }
                    else if (t == typeof(WaypointSet) || t.IsSubclassOf(typeof(WaypointSet)))
                    {
                        waypointSet = ai.WorkingMemory.GetItem<WaypointSet>(varName);
                    }
                    else if (t == typeof(GameObject))
                    {
                        GameObject go = ai.WorkingMemory.GetItem<GameObject>(varName);
                        if (go != null)
                        {
                            WaypointRig wgComp = go.GetComponentInChildren<WaypointRig>();
                            if (wgComp != null)
                                waypointSet = wgComp.WaypointSet;
                        }
                    }
                    else
                    {
                        string setName = ai.WorkingMemory.GetItem<string>(varName);
                        if (!string.IsNullOrEmpty(setName))
                            waypointSet = NavigationManager.Instance.GetWaypointSet(setName);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(varName))
                        waypointSet = NavigationManager.Instance.GetWaypointSet(varName);
                }
            }
            else if (WaypointNetwork.IsConstant)
            {
                waypointSet = NavigationManager.Instance.GetWaypointSet(WaypointNetwork.Evaluate<string>(0, ai.WorkingMemory));
            }
        }

        return waypointSet;
    }
}