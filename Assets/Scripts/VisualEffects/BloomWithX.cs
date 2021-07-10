using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class BloomWithX : MonoBehaviour
{
    private Bloom bloom;
    private Volume volume;
    public float maxDistance = 30;
    public float maxBloom = 1000;
    private float normalBloom;
    private GameObject target;
    private float targetStartX;
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet<Bloom>(out bloom);
        normalBloom = bloom.intensity.value;
        target = GameObject.FindGameObjectWithTag("Player");
        targetStartX = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(bloom.intensity.value < 10)
        {

            float percent = (target.transform.position.x - targetStartX)/maxDistance;
            bloom.intensity.value = Mathf.Lerp(normalBloom, maxBloom,percent );
        }
        else
        {
            bloom.intensity.value = bloom.intensity.value +0.05f;
            Time.timeScale = 0;
        }
    }
}
