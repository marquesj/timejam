using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float effectDuration = 1;
    
    public bool startOnAwake = true;
    public bool destroyWhenInvisible = true;
    private SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(startOnAwake)
            StartEffect();

    }
    public void StartEffect()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float percent = 0;
        float startTime = Time.realtimeSinceStartup;
        float startalpha = spriteRenderer.color.a;
        float alpha = 0;
        while(percent < 1)
        {
            percent = (Time.realtimeSinceStartup - startTime)/ effectDuration;
            alpha = Mathf.Lerp(startalpha, 0, percent);
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b, alpha);
            yield return null;
        }
        if(destroyWhenInvisible)
            Destroy(gameObject);
    }

}
