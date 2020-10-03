using PathCreation;
using UnityEngine;

public class Car : MonoBehaviour
{
	public PathCreator PathCreator;
	public float MaxSpeed = 10f;
	public float Acceleration = 5f;
	public float Deceleration = 1f;

	private float _distanceTraveled;
	private float _currentSpeed;

	public float DistanceTraveled => _distanceTraveled;

	public float CurrentSpeed => _currentSpeed;

	private void Start()
	{
		_distanceTraveled = 0;
		_currentSpeed = 0;
	}

	private void Update()
	{
		if (Input.anyKey)
		{
			_currentSpeed = _currentSpeed + Acceleration * Time.deltaTime;
		}
		else
		{
			_currentSpeed = _currentSpeed - Deceleration * Time.deltaTime;
		}

		_currentSpeed = Mathf.Clamp(_currentSpeed, 0, MaxSpeed);

		_distanceTraveled += _currentSpeed * Time.deltaTime;

		if (_distanceTraveled > PathCreator.path.length)
		{
			_distanceTraveled -= PathCreator.path.length;
		}

		transform.position = PathCreator.path.GetPointAtDistance(_distanceTraveled);
		transform.rotation = PathCreator.path.GetRotationAtDistance(_distanceTraveled);
	}
}
