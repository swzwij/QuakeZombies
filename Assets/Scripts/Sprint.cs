using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sprint : MonoBehaviour
{
    [SerializeField] private TextMesh timerTxt;
    [SerializeField] private TextMesh topTimeTxt;

    private float timer;
    private float topTime = 1000f;

    private bool count;

    private void Update()
    {
        if(count) timer += Time.deltaTime;
    }

    public void StartTimer()
    {
        timer = 0;
        count = true;
    }

    public void EndTimer()
    {
        count = false;
  
        if (timer <= topTime) topTime = timer;

        print(timer);
        print(topTime);

        timerTxt.text = "TIME : " + timer;
        topTimeTxt.text = "TOP TIME : " + topTime;
    }
}
