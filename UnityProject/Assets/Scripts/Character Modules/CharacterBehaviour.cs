﻿using UnityEngine;
using System.Collections;

public class CharacterBehaviour : MonoBehaviour 
{
    public MovementController m_movementController {get; private set;} 
    public CharacterAudio m_characterAudio {get; private set;} 
    public CharacterAnimation m_characterAnimation {get; private set;} 

    protected void Awake()
    {
        m_movementController = gameObject.AddComponent<MovementController>();
        m_characterAudio = GetComponentInChildren<CharacterAudio>();
        m_characterAnimation = GetComponentInChildren<CharacterAnimation>();
    }

    protected virtual void MoveCharacterTowards(Vector3 direction, Vector2 inputAxis)
    {
        m_movementController.MoveTowardsDirection(direction, inputAxis);

        if(m_characterAnimation.gameObject.activeInHierarchy)
        {
            Vector2 moveSpeed = m_movementController.MoveSpeed;
            m_characterAnimation.SetForwardSpeed (moveSpeed.y);
            m_characterAnimation.SetSidewaysSpeed (moveSpeed.x);
        }

    }

    protected virtual void RotateCharacterTowards(Vector3 direction)
    {
        m_movementController.RotateTowardsDirection(direction);
    }
}
