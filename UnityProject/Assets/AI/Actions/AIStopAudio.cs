using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

[RAINAction]
public class AIStopAudio : RAINAction
{
	public Expression audioSource;
	private AudioSource source;

    public AIStopAudio()
    {
        actionName = "AIStopAudio";
    }

    public override void Start(AI ai)
    {
		source = ai.Body.transform.FindChild(audioSource.ExpressionAsEntered).GetComponent<AudioSource>();
		if( source == null )
		{
			Debug.Log("non found");
		}

        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		if( source.isPlaying )
			source.Stop();

        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}