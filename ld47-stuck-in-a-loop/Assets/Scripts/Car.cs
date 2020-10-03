using PathCreation;
using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
	public PathCreator PathCreator;
	public float MaxSpeed = 10f;
	public float Acceleration = 5f;
	public float Deceleration = 1f;

	private bool _isCrashing;

	public float DistanceTraveled { get; private set; }
	public float CurrentSpeed { get; private set; }

	private void Start()
	{
		_isCrashing = false;
		DistanceTraveled = 0;
		CurrentSpeed = 0;
	}

	private void Update()
	{
		if (!_isCrashing)
		{
			UpdateSpeed();
			UpdateCar();
		}
	}

	public void UpdateSpeed()
	{
		if (Input.anyKey)
		{
			CurrentSpeed = CurrentSpeed + Acceleration * Time.deltaTime;
		}
		else
		{
			CurrentSpeed = CurrentSpeed - Deceleration * Time.deltaTime;
		}

		CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, MaxSpeed);
	}

	private void UpdateCar()
	{
		DistanceTraveled += CurrentSpeed * Time.deltaTime;

		if (DistanceTraveled > PathCreator.path.length)
		{
			DistanceTraveled -= PathCreator.path.length;
		}

		transform.position = PathCreator.path.GetPointAtDistance(DistanceTraveled);
		transform.rotation = PathCreator.path.GetRotationAtDistance(DistanceTraveled);
	}

	public void Crash(float distance)
	{
		_isCrashing = true;

		StartCoroutine(CrashSequence(distance));
	}

	private IEnumerator CrashSequence(float distance)
	{
		// Animate crash
		yield return new WaitForSeconds(2f);
		CurrentSpeed = 0;
		DistanceTraveled = distance;
		UpdateCar();
		_isCrashing = false;
	}
}
