using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    public IEnumerator FadeIn(float time) {
        // Increase alpha to 100% over time
        yield return image.DOFade(1, time).WaitForCompletion();
    }

    public IEnumerator FadeOut(float time) {
        // Decrease alpha to 0% over time
        yield return image.DOFade(0, time).WaitForCompletion();
    }

}
