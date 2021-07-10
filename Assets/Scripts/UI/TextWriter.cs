using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextWriter : MonoBehaviour
{
    public List<float> timings = new List<float>();
    public List<string> lines = new List<string>();
    public TMP_Text textBox;
    public float speed = 0.02f;
    public float fadeoutime = 1;
    public float fadeouduration = 1f;
    public bool onStart = false;
    // Start is called before the first frame update
    private void Start() {
        if(onStart)
            Begin();
    }
    public void Begin()
    {
        for(int i = 0; i < timings.Count; i++)
        {
            float endTime = 0;
            if(i<timings.Count - 1)
                endTime = timings[i+1]-fadeoutime;
            else    
                endTime = timings[i]+fadeoutime;
            StartCoroutine(QueueLine(timings[i],lines[i],endTime));
        }
    }
    private IEnumerator QueueLine(float time,string line,float endTime)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(WriteTextCharacterByCharacter(line, speed)); 
        yield return new WaitForSeconds(endTime - time);
        StartCoroutine(FadeOut());
    }

    private IEnumerator WriteTextCharacterByCharacter(string text, float speed)
    {
        int index = 0;
        string currentText;
        textBox.color = new Color(textBox.color.r,textBox.color.g,textBox.color.b,1);
        while(index < text.Length+1)
        {
            currentText = text.Substring(0, index);
            textBox.text = currentText;
            index++;
            yield return new WaitForSeconds(speed);
        }

    }
    private IEnumerator FadeOut()
    {
        float startTime = Time.time;
        float percent = 0;
        while(percent < 1)
        {
            percent = (startTime-Time.time)/fadeouduration;
            float alpha = Mathf.Lerp(1,0,percent);
            textBox.faceColor  = new Color(textBox.color.r,textBox.color.g,textBox.color.b,alpha);
            yield return null;
        }
        textBox.text = "";
    }
}
