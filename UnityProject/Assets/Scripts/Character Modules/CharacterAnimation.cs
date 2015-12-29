using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
{
    public string forwardSpeedKey = "speed";
    public string sidewaysSpeedKey = "sidespeed";
    public string turningSpeedKey = "turningspeed";

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
  //      m_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPositionWeight);
    //    m_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotationWeight);

      //  m_animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootObj.position);
        //m_animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootObj.rotation);
    }

    public void SetForwardSpeed(float speed)
    {
        m_animator.SetFloat(forwardSpeedKey, speed);
    }

    public void SetSidewaysSpeed(float speed)
    {
        m_animator.SetFloat(sidewaysSpeedKey, speed);
    }

    public void SetTurningSpeed(float speed)
    {
        m_animator.SetFloat(turningSpeedKey, speed);
    }
}
