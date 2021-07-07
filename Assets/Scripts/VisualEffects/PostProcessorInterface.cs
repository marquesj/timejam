using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Volume))]
public class PostProcessorInterface : MonoBehaviour
{
    public TimeEvents timeEvents;
    private Coroutine slowEffectRoutine;
    private Volume volume;
    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private LensDistortion lensDistortion;
    private ColorAdjustments colorAdjustments;

    private float defaultChromaticAberration;
    private float defaultLensDistortion;
    private float defaultFilmGrain;
    private Color defaultColorAdjustments;
    void Awake()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<FilmGrain>(out filmGrain);
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);

        filmGrain.active = false;
        lensDistortion.active = false;
        colorAdjustments.active = false;
        chromaticAberration.active = false;

        defaultChromaticAberration = chromaticAberration.intensity.value;
        defaultLensDistortion = lensDistortion.intensity.value;
        defaultColorAdjustments = colorAdjustments.colorFilter.value;
        defaultFilmGrain = filmGrain.intensity.value;

        filmGrain.intensity.value = 0;           
        lensDistortion.intensity.value = 0;
        colorAdjustments.colorFilter.value = Color.white;
        chromaticAberration.intensity.value = 0;

        timeEvents.SlowTimeEvent += ActivateSlowMode;
        timeEvents.RestoreTimeEvent += DeactivateSlowMode;
    }
    void Update()
    {

    }

    private void SetSlowMode(bool state)
    {
        filmGrain.active = state;
        lensDistortion.active = state;
        colorAdjustments.active = state;
        chromaticAberration.active = state;
        if(slowEffectRoutine != null)
            StopCoroutine(slowEffectRoutine);
        slowEffectRoutine = StartCoroutine(LerpSlowEffect(state));

    }

    private IEnumerator LerpSlowEffect(bool state)
    {
        float transitionDuration = 0.2f;
        float startTime = Time.realtimeSinceStartup;
        float percent = 0;

        float startChromaticAberration = chromaticAberration.intensity.value;
        float startLensDistortion = lensDistortion.intensity.value;
        Color startColorAdjustments = colorAdjustments.colorFilter.value;
        float startFilmGrain = filmGrain.intensity.value;

        float goalChromaticAberration;
        float goalLensDistortion;
        Color goalColorAdjustments;
        float goalFilmGrain; 

        if(state)
        {
            goalChromaticAberration = defaultChromaticAberration;
            goalLensDistortion = defaultLensDistortion;
            goalColorAdjustments = defaultColorAdjustments;
            goalFilmGrain = defaultFilmGrain;
        }
        else
        {
            goalChromaticAberration =0;
            goalLensDistortion = 0;
            goalColorAdjustments = Color.white;
            goalFilmGrain = 0;
        }
        
        while(percent < 1)
        {
            percent = (Time.realtimeSinceStartup - startTime)/transitionDuration;

            filmGrain.intensity.value = Mathf.Lerp(startFilmGrain,goalFilmGrain,percent);
            lensDistortion.intensity.value = Mathf.Lerp(startLensDistortion,goalLensDistortion,percent);
            chromaticAberration.intensity.value = Mathf.Lerp(startChromaticAberration,goalChromaticAberration,percent);
            colorAdjustments.colorFilter.value = Color.Lerp(startColorAdjustments,goalColorAdjustments, percent);

            yield return null;
        }
    }
    private void ActivateSlowMode()
    {
        SetSlowMode(true);
    }
    private void DeactivateSlowMode()
    {
        SetSlowMode(false);
    }


}
