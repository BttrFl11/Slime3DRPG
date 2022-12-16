using System.Collections;
using TMPro;
using UnityEngine;

public class FPSLimiter : Singleton<FPSLimiter>
{
    public TextMeshProUGUI _text;

    private void Awake()
    {
        Time.captureFramerate = 30;
        //StartCoroutine(ShowFPS());
    }

    //public IEnumerator ShowFPS()
    //{
    //    var fps = Time.frameCount / Time.time;
    //    _text.text = fps.ToString("0");

    //    yield return new WaitForSeconds(0.5f);

    //    StartCoroutine(ShowFPS());
    //}
}