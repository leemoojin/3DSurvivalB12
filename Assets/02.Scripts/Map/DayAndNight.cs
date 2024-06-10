using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float CurrentTime;
    public float FullDayLength;
    public float StartTime = 0.4f;
    public Vector3 Noon;
    private float _timeRate;

    [Header("Sun")]
    public Light Sun;
    public Gradient SunColor;
    public AnimationCurve SunIntensity;

    [Header("Moon")]
    public Light Moon;
    public Gradient MoonColor;
    public AnimationCurve MoonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve LightingIntensityMultiplier;
    public AnimationCurve ReflectIntensityMultiplier;

    public bool IsDay;
    
    private void Start()
    {
        _timeRate = 1.0f / FullDayLength;
        CurrentTime = StartTime;
        IsDay = true;
    }

    private void Update()
    {
        CurrentTime = (CurrentTime + _timeRate * Time.deltaTime) % 1.0f;

        IsDay = CurrentTime is >= 0.3f and <= 0.7f;

        UpdateLighting(Sun, SunColor, SunIntensity);
        UpdateLighting(Moon, MoonColor, MoonIntensity);

        RenderSettings.ambientIntensity = LightingIntensityMultiplier.Evaluate(CurrentTime);
        RenderSettings.reflectionIntensity = ReflectIntensityMultiplier.Evaluate(CurrentTime);
    }

    private void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        var intensity = intensityCurve.Evaluate(CurrentTime);
        lightSource.transform.eulerAngles = Noon * ((CurrentTime - (lightSource == Sun ? 0.25f : 0.75f)) * 4.0f);
        lightSource.color = gradient.Evaluate(CurrentTime);
        lightSource.intensity = intensity;

        var go = lightSource.gameObject;
        switch (lightSource.intensity)
        {
            case 0 when go.activeInHierarchy:
                go.SetActive(false);
                break;
            case > 0 when !go.activeInHierarchy:
                go.SetActive(true);
                break;
        }
    }
}