using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source - https://gist.github.com/ftvs/5822103

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	[SerializeField] private Transform camTransform;

	// How long the object should shake for.
	[SerializeField] private float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	[SerializeField] private float shakeAmount = 0.7f;
	[SerializeField] private float decreaseFactor = 1.0f;

	private float currentShakeDuration = 0f;

	Vector3 originalPos;

	private void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	private void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	private void Update()
	{
		if (currentShakeDuration > 0) Shake();
		else ResetShake();
	}

	public void StartShake() => currentShakeDuration = shakeDuration;

	private void Shake()
    {
		camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, originalPos + Random.insideUnitSphere * shakeAmount, Time.deltaTime);

		currentShakeDuration -= Time.deltaTime * decreaseFactor;
	}

    private void ResetShake()
    {
		currentShakeDuration = 0f;
		camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, originalPos, Time.deltaTime);
	}
}
