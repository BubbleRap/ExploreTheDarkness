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
    }

    protected virtual void RotateCharacterTowards(Vector3 direction)
    {
        m_movementController.RotateTowardsDirection(direction);
    }
}
