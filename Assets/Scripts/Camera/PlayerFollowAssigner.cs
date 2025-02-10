using Cinemachine;
using System;
using UnityEngine;

public class PlayerFollowAssigner : MonoBehaviour
{
    public static Func<Transform> OnAssignPlayerAsFollowTarget;
    public static Func<CinemachineVirtualCamera> GetVirtualCamera;
    public static Func<Vector2> GetAimingOffset;
    public static Action<bool> TurnOnOffVirtualCamera;
    public static Action<float, float> CameraShake;

    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin channelPerlin;

    [SerializeField] private float cameraAimingOffsetX;
    [SerializeField] private float cameraAimingOffsetY;

    private float shakeTimer;
    private float startingIntensity;
    private float shakeTimerTotal;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        AssignPlayerToFollowTargetParameter();
    }

    private void Update()
    {
        ShakeNoLerp();
        //ShakeWithLerp();
    }

    public void ShakeEffect(float intensity, float time)
    {
        channelPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void ShakeNoLerp()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer < 0)
            {
                channelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    private void ShakeWithLerp()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer < 0)
            {
                channelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0, 1 - (shakeTimer / shakeTimerTotal));

            }
        }
    }

    public void AssignPlayerToFollowTargetParameter()
    {
        virtualCamera.Follow = OnAssignPlayerAsFollowTarget?.Invoke();
    }

    public CinemachineVirtualCamera ReturnVirtualCamera()
    {
        return virtualCamera;
    }

    public Vector2 ReturnAimingOffset()
    {
        return new Vector2(cameraAimingOffsetX, cameraAimingOffsetY);
    }

    private void OnEnable()
    {
        CameraShake += ShakeEffect;
        GetAimingOffset += ReturnAimingOffset;
        GetVirtualCamera += ReturnVirtualCamera;
        TurnOnOffVirtualCamera += TurnOnOfVirtualCamera;
    }

    private void OnDisable()
    {
        CameraShake -= ShakeEffect;
        GetAimingOffset -= ReturnAimingOffset;
        GetVirtualCamera -= ReturnVirtualCamera;
        TurnOnOffVirtualCamera -= TurnOnOfVirtualCamera;
    }

    private void TurnOnOfVirtualCamera(bool on)
    {
        if (on)
        {
            AssignPlayerToFollowTargetParameter();
        }
        else
        {
            virtualCamera.Follow = null;
        }
    }
}
