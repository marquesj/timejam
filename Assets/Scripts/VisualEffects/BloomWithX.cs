using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class BloomWithX : MonoBehaviour
{
    private Bloom bloom;
    private Volume volume;
    public float maxDistance = 30;
    public float maxBloom = 1000;
    private float normalBloom;
    private GameObject target;
    private float targetStartX;
    private AudioSource[] playerAudios;
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet<Bloom>(out bloom);
        normalBloom = bloom.intensity.value;
        target = GameObject.FindGameObjectWithTag("Player");
        targetStartX = transform.position.x;
        playerAudios = target.GetComponentsInChildren<AudioSource>();
            foreach(AudioSource audio in playerAudios)
            {
                audio.volume = .5f;
            }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(bloom.intensity.value < 10)
        {
            float percent = (target.transform.position.x - targetStartX)/maxDistance;
            foreach(AudioSource audio in playerAudios)
            {
                audio.volume = Mathf.Lerp(.5f,0,percent*1.5f);
            }

            bloom.intensity.value = Mathf.Lerp(normalBloom, maxBloom,percent );
        }
        else
        {
            bloom.intensity.value = bloom.intensity.value +0.05f;
            Time.timeScale = 0;
            EndScene();
        }
    }

    private void EndScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex + 1);
    }
}
