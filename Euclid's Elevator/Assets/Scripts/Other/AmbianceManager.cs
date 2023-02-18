using System.Collections;
using UnityEngine;
using System;

public class AmbianceManager : MonoBehaviour
{
    [Header("Sticky footsteps settings")]
    [SerializeField] float time;
    [SerializeField] float randomTimeSurplus;
    [SerializeField] AudioSource stickySource;

    private void Awake()
    {
        stickySource.Play();
        StartCoroutine(WaitAndExec(time + UnityEngine.Random.Range(-randomTimeSurplus, randomTimeSurplus), () => 
        {
            stickySource.Play();
        }, true));
    }

    IEnumerator WaitAndExec(float time, Action exec, bool repeat = false)
    {
        yield return new WaitForSeconds(time);
        exec?.Invoke();

        if (repeat)
        {
            StartCoroutine(WaitAndExec(time, exec, repeat));
        }
    }
}
