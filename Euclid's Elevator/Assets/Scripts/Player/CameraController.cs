using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(FpsController))]
public class CameraController : MonoBehaviour
{
    public bool canLook;
    public Transform Camera;
    [SerializeField] FpsController controller;
    [SerializeField] Volume bloomVolume;
    [SerializeField] Volume blurVolume;
    [SerializeField] Volume blackScreenVolume;
    [SerializeField] Vector2 sensitivities;
    [SerializeField] float turnTime;
    [SerializeField] float animationTime;

    Vector2 rotation;

    Transform initialParent;

    Vector3 initialLocalPosition;
    Quaternion rotationOnEnter;

    IEnumerator blackScreenCor;

    private void Awake()
    {
        canLook = true;
        initialParent = Camera.parent;
        initialLocalPosition = Camera.localPosition;
    }

    private void Start()
    {
        GameManager.MakePausable(this);
    }

    private void Update()
    {
        if (!canLook)
            return;

        Vector2 input = controller.PlayerInputActions.General.Look.ReadValue<Vector2>();

        rotation.x -= input.y * sensitivities.y;
        rotation.y += input.x * sensitivities.x;

        rotation.x = Mathf.Clamp(rotation.x, -80, 80);

        Camera.localEulerAngles = new Vector3(rotation.x, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotation.y, 0);
    }

    public void Turn(Vector3 pos)
    {
        StartCoroutine(TurnC(pos));
    }

    IEnumerator TurnC(Vector3 pos)
    {
        float t = 0;

        Quaternion to = Quaternion.LookRotation((pos - Camera.position).normalized);
        Quaternion from = Quaternion.Euler(new Vector3(Camera.localEulerAngles.x, transform.localEulerAngles.y, 0));

        while (t < 1)
        {
            t += Time.deltaTime / turnTime;

            t = Mathf.Clamp01(t);

            Vector3 step = Quaternion.Slerp(from, to, t).eulerAngles;

            Camera.localEulerAngles = new Vector3(step.x, 0, 0);
            transform.localEulerAngles = new Vector3(0, step.y, 0);

            if (new Vector3(Camera.localEulerAngles.x, transform.localEulerAngles.y, 0) == Vector3.Scale(to.eulerAngles, new Vector3(1, 1, 0)))
            {
                rotation = new Vector2(Camera.localEulerAngles.x, transform.localEulerAngles.y);
            }
            yield return null;
        }
    }

    public void EnterAnimation(Transform followTarget)
    {
        canLook = false;

        rotationOnEnter = Camera.rotation;
        Camera.parent = followTarget;

        StartCoroutine(EnterAnimationPosition());
        StartCoroutine(EnterAnimationRotation());
    }

    IEnumerator EnterAnimationPosition()
    {
        Vector3 initial = Camera.localPosition;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / animationTime;

            Camera.localPosition = Vector3.Slerp(initial, Vector3.zero, t);

            yield return null;
        }
    }

    IEnumerator EnterAnimationRotation()
    {
        Quaternion initial = Camera.localRotation;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / animationTime;

            Camera.localRotation = Quaternion.Slerp(initial, Quaternion.identity, t);

            yield return null;
        }
    }

    public void ExitAnimation()
    {
        Camera.parent = initialParent;

        StartCoroutine(ExitAnimationPosition());
        StartCoroutine(ExitAnimationRotation());
    }

    IEnumerator ExitAnimationPosition()
    {
        Vector3 initial = Camera.localPosition;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / animationTime;

            Camera.localPosition = Vector3.Slerp(initial, initialLocalPosition, t);

            yield return null;
        }
    }

    IEnumerator ExitAnimationRotation()
    {
        Quaternion initial = Camera.localRotation;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / animationTime;

            Camera.localRotation = Quaternion.Slerp(initial, rotationOnEnter, t);

            yield return null;
        }
    }

    public void ResetAngle(float yrotation)
    {
        rotation.x = 0;
        rotation.y = yrotation;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 initial = Camera.localPosition;
        float elapsed = 0;

        while (elapsed < duration)
        {
            Camera.localPosition = initial + Random.insideUnitSphere * magnitude * Time.timeScale;

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.localPosition = initial;
    }

    public void BlackScreen(float time)
    {
        if (blackScreenCor != null)
        {
            StopCoroutine(blackScreenCor);
        }
        blackScreenCor = VolumeFade(blackScreenVolume, time, 1);
        StartCoroutine(blackScreenCor);
    }

    public void ClearBlackScreen(float time)
    {
        if (blackScreenCor != null)
        {
            StopCoroutine(blackScreenCor);
        }
        blackScreenCor = VolumeFade(blackScreenVolume, time, 0);
        StartCoroutine(blackScreenCor);
    }

    IEnumerator VolumeFade(Volume vol, float time, float finalValue)
    {
        float initial = vol.weight;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / time;

            vol.weight = Mathf.Lerp(initial, finalValue, t);

            yield return null;
        }
    }

    public void SetBloom(bool bloom)
    {
        if (bloom)
        {
            bloomVolume.weight = 1;
        }
        else
        {
            bloomVolume.weight = 0;
        }
    }

    public void SetBlur(bool blur)
    {
        if (blur)
        {
            blurVolume.weight = 1;
        }
        else
        {
            blurVolume.weight = 0;
        }
    }

    public void SetSens(float sens)
    {
        sensitivities *= sens;
    }
}