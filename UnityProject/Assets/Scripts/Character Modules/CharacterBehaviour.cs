using UnityEngine;
using System.Collections;

public class CharacterBehaviour : MonoBehaviour 
{
    protected MovementController m_movementController; 
    protected CharacterAudio m_characterAudio;
    protected CharacterAnimation m_characterAnimation;

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
