using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    // Scale the UI HP bar by hpNormalized
    public void SetHP(float hpNormalized) {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHPSmooth(float newHp) {
        float currentHp = health.transform.localScale.x;
        float changeAmount = currentHp - newHp;

        // Smoothly update hp until it reaches the new value
        while(currentHp - newHp > Mathf.Epsilon) {
            currentHp -= changeAmount * Time.deltaTime;
            health.transform.localScale = new Vector3(currentHp, 1f);
            yield return null;
        }

        health.transform.localScale = new Vector3(newHp, 1f);
    }
}
