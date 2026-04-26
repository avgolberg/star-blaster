using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeDuration = 0.3f;
    [SerializeField] float shakeMagnitde = 0.2f;
    float timeElapsed = 0f;
    Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    public void Play()
    {
        if (!isEnabled) return;
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        while (timeElapsed < shakeDuration)
        {
            transform.position = initialPosition + (Vector3)Random.insideUnitCircle * shakeMagnitde;
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = initialPosition;
        timeElapsed = 0f;
    }

    private bool isEnabled = true;

    public void ApplyConfig(bool enableShake, float duration, float magnitude)
    {
        isEnabled = enableShake;
        shakeDuration = duration;
        shakeMagnitde = magnitude;
    }
}
