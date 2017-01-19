using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
{
    public string m_forwardSpeedKey = "speed";
    public string m_sidewaysSpeedKey = "sidespeed";
    public string m_turningSpeedKey = "turningspeed";

    private Animator m_animator;

    private Vector3 m_currentLookDirection;
    private Vector3 m_targetLookDirection;

    private Vector3 m_currentRightHandPos;
    private Vector3 m_targetRightHandPos;

    private float m_currentRightHandIKWeight;
    private float m_targetRightHandIKWeight;

    private float m_currentLookIKWeight;
    private float m_targetLookIKWeight;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();

       
    }

    public void Start()
    {

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!m_animator.isInitialized)
            return;

        Transform m_headBone = m_animator.GetBoneTransform(HumanBodyBones.Head);

        m_currentLookIKWeight = Mathf.MoveTowards(m_currentLookIKWeight, m_targetLookIKWeight, Time.deltaTime);
        m_currentLookDirection = Vector3.MoveTowards(m_currentLookDirection, m_targetLookDirection, Time.deltaTime);
        m_currentRightHandPos = Vector3.MoveTowards(m_currentRightHandPos, m_targetRightHandPos, Time.deltaTime);
        m_currentRightHandIKWeight = Mathf.MoveTowards(m_currentRightHandIKWeight, m_targetRightHandIKWeight, Time.deltaTime);
       

        m_animator.SetLookAtPosition(transform.position + m_currentLookDirection);
        m_animator.SetLookAtWeight(m_currentLookIKWeight);

        m_animator.SetIKPosition(AvatarIKGoal.RightHand, m_currentRightHandPos);
        m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, m_currentRightHandIKWeight);
    }

    public void SetForwardSpeed(float speed)
    {
        m_animator.SetFloat(m_forwardSpeedKey, speed);
    }

    public void SetSidewaysSpeed(float speed)
    {
        m_animator.SetFloat(m_sidewaysSpeedKey, speed);
    }

    public void SetTurningSpeed(float speed)
    {
        m_animator.SetFloat(m_turningSpeedKey, speed);
    }

    public void SetLookingPoint(Vector3 dir, float weight = 1f)
    {
        m_targetLookDirection = dir;
        m_targetLookIKWeight = weight;
    }

    public void SetRightHandIK(Vector3 pos, float weight = 1f)
    {
        m_targetRightHandPos = pos;
        m_targetRightHandIKWeight = weight;
    }

    public void SetAnimationState(string name)
    {
        m_animator.CrossFade(name, 0f);
    }
}
