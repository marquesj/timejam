using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private Health health;
    private void Start() {
        health = GetComponent<Health>();
        health.DamageEvent+= PlayEffect;
    }

    private void PlayEffect()
    {
        StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        float startTime = Time.time;
        float percent = 0;
        float duration= 0.2f;
        Color startColor= Color.red;
        Color goalColor= Color.white;
        SetColor(startColor);
        while(percent < 1)
        {
            percent= (Time.time-startTime)/duration;
            SetColor(Color.Lerp(startColor,goalColor,percent*percent));
            yield return null;
        }
    }

    private void SetColor(Color color)
    {
        foreach(SpriteRenderer sprite in sprites)
            sprite.color = color;
    }
}

