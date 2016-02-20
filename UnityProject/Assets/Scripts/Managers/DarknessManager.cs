using UnityEngine;
using System.Collections;

public class DarknessManager : MonoBehaviour
{
    static private DarknessManager s_Instance;
    static public DarknessManager Instance
    {
        get { return s_Instance; }
    }

    public SiljaBehaviour m_mainCharacter;

    //   private bool inDarkness = false;
  //  private bool DarknessApproaching = false;
 //   private bool thirdPersonInDark = false;





    private int lastCheckPoint = 0;

    public float captureTimeTotal = 2.0f;
    private bool m_characterIsCaught = false;

    private float m_availableTimeInDark = 10f;
		private float m_timeSpentInDark = 0f;

    public float blackscreenTime = 5.0f;

    private void Awake()
    {
        s_Instance = this;
    }

    public void SetLastCheckpointIndex(int idx)
    {
        if (idx > lastCheckPoint)
            lastCheckPoint = idx;
    }


    private void Update()
    {
        UpdateCharacterAudio();
     //   UpdateMonsterMechanics();
				UpdateDarknessCountdown();
    }

    //Sounds of darkness & fear
    private void UpdateCharacterAudio()
    {
        //    m_mainCharacter.charAudio.PlayMonsterLoop();

        //    float monsterVolume = m_mainCharacter.charAudio.MonsterAudioVolume;
        //    if (thirdPersonInDark || m_mainCharacter.flshCtrl._lightIntensity == 0 && monsterVolume < 1f)
        //    {
        //        monsterVolume += Time.deltaTime;
        //    }
        //    else if (monsterVolume > 0)
        //    {
        //        monsterVolume -= Time.deltaTime;
        //    }
        //   
        //    m_mainCharacter.charAudio.SetMonsterVolume(monsterVolume);

        //  if (DarknessApproachingTimer <= TimeInDarknessTotal - (TimeInDarknessTotal / 1.3f) && m_mainCharacter.moveCtrl.isMoving())
        //  {
        //      m_mainCharacter.charAudio.SetMonsterAudioChase();
        //  }
        //  else
        //  {
        //      m_mainCharacter.charAudio.SetMonsterAudioSearch();
        //  }
    }



    //Out of light, monster coming
    public void UpdateMonsterMechanics()
    {
        if (m_mainCharacter.flshCtrl.IsDischarged)
        {
            if (!m_characterIsCaught)
            {								
                m_characterIsCaught = true;
                StartCoroutine("CharacterCaughtSequence");
            }
        }
        else
            StopCoroutine("CharacterCaughtSequence");
    }

		public void UpdateDarknessCountdown()
		{
				if( m_characterIsCaught )
						return;
				
				if (m_mainCharacter.flshCtrl.IsDischarged)
				{
						m_timeSpentInDark += Time.deltaTime;

						if( m_timeSpentInDark > m_availableTimeInDark )
						{
								m_characterIsCaught = true;
								StartCoroutine(CharacterCaughtSequence());
						}
				}
				else
				{
						m_timeSpentInDark = 0f;
				}
		}

    public void isDarknessApproaching(bool boolean)
    {
        m_mainCharacter.flshCtrl.IsDischargeEnabled = boolean;
    }

    private IEnumerator CharacterCaughtSequence()
    {
        m_mainCharacter.IsMoveEnabled = false;
        m_mainCharacter.PlaySiljaCaughtRandomSound();

        yield return new WaitForSeconds(captureTimeTotal - 0.1f);

        m_mainCharacter.SetHeartbeatVolume(0.0f);
        Fader.Instance.FadeScreen(true, 1.0f);

        yield return new WaitForSeconds(blackscreenTime);

        m_mainCharacter.ResetCharacter();

        m_characterIsCaught = false;
				m_timeSpentInDark = 0f;

        ObjectsTranslator.MoveObjectTo(lastCheckPoint);
    }
}
