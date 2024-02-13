using System.Collections;
using UnityEngine;
using TMPro;

public class ShowFPS : MonoBehaviour
{
    TMP_Text txt;

    void Start()
    {
        txt = GetComponent<TMP_Text>();
        StartCoroutine(Loop());
    }

    int frameCount = 0;

    IEnumerator Loop()
    {
        while (Application.isPlaying)
        {
            frameCount = Time.frameCount;
            yield return new WaitForSeconds(1f);

            txt.text = $"{Time.frameCount - frameCount} fps";
        }
    }
}