using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
{
    public string m_forwardSpeedKey = "speed";
    public string m_sidewaysSpeedKey = "sidespeed";
    public string m_turningSpeedKey = "turningspeed";

    private Animator m_animator;
    private Vector3 m_lookDirection;
    private float m_lookWeight;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();

       
    }

    public void Start()
    {
        Debug.Log("hasTransformHierarchy: " + m_animator.hasTransformHierarchy);
        Debug.Log("isHuman: " + m_animator.isHuman);
        Debug.Log("isInitialized: " + m_animator.isInitialized);
        Debug.Log("spine: " + (m_animator.GetBoneTransform(HumanBodyBones.Spine) != null));

        Debug.Log("avatar.isHuman: " + m_animator.avatar.isHuman);
        Debug.Log("avatar.isValid: " + m_animator.avatar.isValid);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!m_animator.isInitialized)
            return;

        Transform m_headBone = m_animator.GetBoneTransform(HumanBodyBones.Head);

        if (m_headBone != null)
        {
            m_animator.SetLookAtPosition(transform.position + m_lookDirection);
            m_animator.SetLookAtWeight(m_lookWeight);
        }
    }

    public void SetForwardSpeed(float speed)
    {
        if( m_animator.isInitialized )
            m_animator.SetFloat(m_forwardSpeedKey, speed);
    }

    public void SetSidewaysSpeed(float speed)
    {
        if (m_animator.isInitialized)
            m_animator.SetFloat(m_sidewaysSpeedKey, speed);
    }

    public void SetTurningSpeed(float speed)
    {
        if (m_animator.isInitialized)
            m_animator.SetFloat(m_turningSpeedKey, speed);
    }

    public void SetLookingPoint(Vector3 dir, float weight = 1f)
    {
        m_lookDirection = dir;
        m_lookWeight = weight;
    }
}
