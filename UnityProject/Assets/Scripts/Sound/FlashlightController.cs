using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour
{
    public Light teddyLight = null;
    public Light teddyLightFlash = null;
    public Light teddyLightFlash2 = null;

    

    [Range(0f, 0.020f)]
    public float fadingOutSpeed = 0.002f;

    [Range(0f, 0.020f)]
    public float fadingInSpeed = 0.012f;

    [Range(0f, 2.50f)]
    public float maximumIntensity = 1f;

    [Range(0f, 1.00f)]
    public float mimimumIntensity = 0f;

    private float maxFlickerIntensity = 0;
    private float minFlickerIntensity = 0;



    private float _lightIntensity = 0f;
   

   

    private float lightFlickerInterval = 0.0f;

    public float rechargeSpeed = 5.0f;

    private bool m_flashlightEnabled = false;

    private float m_bigSpotlightIntensity = 0f;
    private float m_smallSpotlightIntensity = 0f;

    public float m_batteryLife = 40.0f;
    private bool m_hasRecievedChargeNow = false;
    private bool m_isDischarging = false;


    private float m_chargeLeft = 0f;
    private float m_dischargeSpeed = 1f;
    private bool m_dischargeEnabled = true;

    private bool m_isDischared = false;

    public float ChargeLeft
    {
        get { return m_chargeLeft; }
    }

    public bool IsDischarging
    {
        get { return m_dischargeEnabled && m_isDischarging; }
    }

    public bool IsDischarged
    {
        get { return m_isDischared; }
    }

    public bool IsEnabled
    {
        get { return m_flashlightEnabled; }
    }

    public bool IsDischargeEnabled
    {
        set { m_dischargeEnabled = value; }
    }

    private void Start()
    {
        maxFlickerIntensity = mimimumIntensity * 2;
        minFlickerIntensity = mimimumIntensity * 1.5f;
    }

    public void SetDischargeSpeed(float val)
    {
        m_dischargeSpeed = val;
    }

    public void EnableFlashlight(bool state)
    {
        m_flashlightEnabled = state;
        teddyLight.enabled = state;
    }


    public void SetLightIntensity()
    {
        _lightIntensity = Mathf.Clamp(_lightIntensity, mimimumIntensity, maximumIntensity);
    }

    public void SetFlashlightValues(float val1, float val2, float val3)
    {
        SetPointLightIntensity(val1);
        SetBigSpotlightIntensity(val2);
        SetSmallSpotlightIntensity(val3);
    }

    public void SetPointLightIntensity(float intensity)
    {
        _lightIntensity = intensity;
        teddyLight.intensity = intensity;
    }

    public void SetBigSpotlightIntensity(float intensity)
    {
        m_bigSpotlightIntensity = teddyLightFlash.intensity = intensity;
    }

    public void SetSmallSpotlightIntensity(float intensity)
    {
        m_smallSpotlightIntensity = teddyLightFlash2.intensity = intensity;
    }

    public void ChargeFlashlight()
    {
         m_hasRecievedChargeNow = true;
    }

    public void RechargeFlashlight()
    {
        m_chargeLeft = m_batteryLife;
        _lightIntensity = maximumIntensity;

        SetFlashlightValues(1f, 2f, 2f);

        SetLightIntensity();
    }

    private void LateUpdate()
    {
        if (!m_dischargeEnabled)
        {
            RechargeFlashlight();
            return;
        }

        ChargingControl();
        FlickerControl();
    }

    private void ChargingControl()
    {
        m_isDischarging = !m_hasRecievedChargeNow;
				m_isDischared = m_chargeLeft <= 0f;

        if (!m_hasRecievedChargeNow)
        {
            _lightIntensity -= fadingOutSpeed;

            m_chargeLeft -= m_dischargeSpeed * Time.deltaTime;
            m_chargeLeft = Mathf.Clamp(m_chargeLeft, 0f, m_batteryLife);
        }
        else
        {
            _lightIntensity += fadingInSpeed;

            m_chargeLeft += rechargeSpeed * Time.deltaTime;
            m_chargeLeft = Mathf.Clamp(m_chargeLeft, 0f, m_batteryLife);
        }

        SetLightIntensity();

        // reset the state for the current frame
        m_hasRecievedChargeNow = false;
    }


    private void FlickerControl()
    {
        if (m_isDischarging)
        {
            // in seconds
            if (m_chargeLeft >= 7.14f && m_chargeLeft <= 7.69f)
            {
                lightFlickerInterval += Time.deltaTime;

                if (lightFlickerInterval < 0.2f)
                {
                    SetFlashlightValues(0.3f, 1f, 1f);
                }
                else if (lightFlickerInterval >= 0.2f && lightFlickerInterval < 0.29f)
                {
                    SetFlashlightValues(1f, 2f, 2f);
                }
                else
                {
                    SetFlashlightValues(0.3f, 1f, 1f);
                }
            }
            else
            if (m_chargeLeft > 0f && m_chargeLeft <= 1.0f)
            {
                float lightIntensity = Mathf.Clamp01(m_chargeLeft);

                SetBigSpotlightIntensity(lightIntensity);
                SetSmallSpotlightIntensity(lightIntensity);
            }
            else
            if( m_chargeLeft <= 0f )
            {
                SetFlashlightValues(0.3f, 0f, 0f);
            }
        }
        else
        {
            lightFlickerInterval = 0.0f;
     
            float lightIntensity = m_chargeLeft * 2f / m_batteryLife;
            SetFlashlightValues((lightIntensity / 2), lightIntensity, lightIntensity);
        }
    }
}

