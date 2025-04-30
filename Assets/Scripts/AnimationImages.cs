using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationImages : MonoBehaviour {

    public List<Sprite> frames;
    public float delay = 4f;
    public float fade = 2f;

    public Image image;
    public Image image2;

    Coroutine ChangeImages;
    int index = 0;

    float startTime;

    public void StartAnim(List<Sprite> s)
    {
        frames = s;
        ChangeImages = StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        index = 0;
        image.color = new Color(1f, 1f, 1f, 1f);
        image2.color = new Color(1f, 1f, 1f, 0f);

        while (true)
        {
            image.sprite = frames[index];
            yield return new WaitForSeconds(delay);
            index++;
            index = index % frames.Count;

            startTime = Time.time;
            image2.sprite = frames[index];
            while (Time.time < startTime + fade)
            {
                Color clr = image.color;
                Color clr2 = image2.color;
                image.color = new Color(1, 1, 1, clr.a - (Time.deltaTime / fade));
                image2.color = new Color(1, 1, 1, clr2.a + (Time.deltaTime / fade));
                yield return null;
            }

            image2.color = new Color(1f, 1f, 1f, 0f);
            image.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void ResetAnim()
    {
        if (ChangeImages != null) StopCoroutine(ChangeImages);

        index = 0;
        image.color = new Color(1f, 1f, 1f, 1f);
        image2.color = new Color(1f, 1f, 1f, 0f);
    }
}
