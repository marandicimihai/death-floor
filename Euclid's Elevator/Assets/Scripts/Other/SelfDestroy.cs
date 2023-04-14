using System.Collections;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] AudioSource source;

    void Awake()
    {
        StartCoroutine(SelfDestroyC(source.clip.length));
    }

    IEnumerator SelfDestroyC(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}