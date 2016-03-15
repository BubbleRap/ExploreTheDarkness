using UnityEngine;
using System.Collections;

public class MonsterBehaviour : CharacterBehaviour, IAgent
{
    public enum MonsterState
    {
        Idling,
        Creeping,
        Crawling
    }
        
    private MonsterState m_state = MonsterState.Crawling;
    private SiljaBehaviour m_character;

    private new void Awake()
    {
        base.Awake();

        m_movementController.InitializeCharacterController(45f, 0.3f, 0.01f, 0.15f, 2.0f);
        m_movementController.InitializeCharacterMotor(false, 0.5f, 0.5f, 0.5f, false, false, false);

        SetBehaviourState(m_state);
    }

    private void Start()
    {
        m_character = DarknessManager.Instance.m_mainCharacter;
    }

    private void Update()
    {
        if(m_character == null)
            return;
        
        Vector3 dirToChar = m_character.transform.position - transform.position;
        dirToChar.y = 0f;
        dirToChar.Normalize();

        switch( m_state )
        {
        case MonsterState.Idling:
            break;

        case MonsterState.Creeping:
            
            RotateCharacterTowards(dirToChar);
            break;

        case MonsterState.Crawling:

            if( CanSeeCharacter() )
            {
                MoveCharacterTowards(dirToChar, Vector2.up);
                RotateCharacterTowards(dirToChar);
            }

            break;
        }
    }

    public void SetBehaviourState( MonsterState state )
    {
        switch(state)
        {
        case MonsterState.Idling:
            m_characterAnimation.SetAnimationState("Idle");
            break;
        case MonsterState.Creeping:
            m_characterAnimation.SetAnimationState("Idle");
            break;
        case MonsterState.Crawling:
            m_characterAnimation.SetAnimationState("Crawl");
            break;
        }

        m_state = state;
    }

    public bool CanSeeCharacter()
    {
        bool isVisible = false;

        // cast Default layer only
        LayerMask mask = 1 << 0;

        if( !Physics.Linecast(m_character.transform.position, transform.position, mask ) )
        {
            isVisible = true;
        }

        return isVisible;
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractableObject interactable = other.GetComponent<IInteractableObject>();
        interactable.Activate();

        Debug.Log(interactable.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractableObject interactable = other.GetComponent<IInteractableObject>();
        interactable.Activate();
    }
}
