using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [SerializeField] private float globalShakeForce = 1f;
    [SerializeField] private CinemachineImpulseListener impulseListener;

    private CinemachineImpulseDefinition impulseDefinition;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }

    public void ScreenShakeFromProfle(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        //apply settings
        SetupScreenShakeSettings(profile, impulseSource);

        //scrrenshake
        impulseSource.GenerateImpulseWithForce(profile.impactForce);
    } 

    private void SetupScreenShakeSettings(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        impulseDefinition = impulseSource.m_ImpulseDefinition;

        //change the impact source settings
        impulseDefinition.m_ImpulseDuration = profile.impactTime;
        impulseSource.m_DefaultVelocity = profile.defaultVelocity;
        impulseDefinition.m_CustomImpulseShape = profile.impactCurve;

        //change the impulse source settngs
        impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
        impulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrequency;
        impulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration;
    }
}
