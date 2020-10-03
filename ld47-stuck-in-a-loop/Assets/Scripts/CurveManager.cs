using PathCreation;
using UnityEngine;

public class CurveManager : MonoBehaviour
{
	public PathCreator PathCreator;
	public Car Car;

	public float PerfectReleaseDistance;
	public float GreatReleaseDistance;
	public float ReleaseDistanceCheck;

	public float DriftOffAccelerationThresholdTime = 0.5f;

	public Curve[] Curves;

	private int _currentCurveIndex;
	private float _speedOverflow;
	private bool _driftOffCheck;

	public void Start()
	{
		_currentCurveIndex = 0;
		_speedOverflow = 0;
		_driftOffCheck = true;
	}

	public bool check = false;

	public void Update()
	{
		// Remover esse lixo
		if (Car.DistanceTraveled < 5) check = false;

		if (check) return;

		// Drift off check
		if (_driftOffCheck && Car.DistanceTraveled > Curves[_currentCurveIndex].EntranceDistance)
		{
			if (Car.CurrentSpeed < Car.MaxSpeed * Curves[_currentCurveIndex].MaxSpeedThreshold)
			{
				_driftOffCheck = false;
			}

			if (Input.anyKey)
			{
				_speedOverflow += Time.deltaTime;
			}

			if (_speedOverflow > ((Curves[_currentCurveIndex].ExitDistance - Curves[_currentCurveIndex].EntranceDistance) / Car.MaxSpeed) * DriftOffAccelerationThresholdTime)
			{
				Debug.Log("Drift off");
				ResetLap();
			}
		}

		ReleaseCheck();
	}

	private void ReleaseCheck()
	{
		// Car is in range for checking score
		if (Car.DistanceTraveled > Curves[_currentCurveIndex].ExitDistance - ReleaseDistanceCheck)
		{
			var distanceFromExitPoint = Mathf.Abs(Car.DistanceTraveled - Curves[_currentCurveIndex].ExitDistance);

			if (Input.anyKey)
			{
				if (distanceFromExitPoint < PerfectReleaseDistance)
				{
					Debug.Log("Perfect");
				}
				else if (distanceFromExitPoint < GreatReleaseDistance)
				{
					Debug.Log("Great");
				}
				else
				{
					Debug.Log("Miss");
				}

				ResetLap();
			}
			else
			{
				if (distanceFromExitPoint > ReleaseDistanceCheck)
				{
					Debug.Log("Failure");
					ResetLap();
				}
			}
		}
	}

	private void ResetLap()
	{
		_speedOverflow = 0;
		check = true;
		_driftOffCheck = true;
	}

	public void OnDrawGizmos()
	{
		if (PathCreator == null) return;

		foreach (var curve in Curves)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(curve.EntranceDistance), 0.5f);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(curve.ExitDistance), 0.5f);
		}

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(Curves[_currentCurveIndex].ExitDistance - PerfectReleaseDistance), 0.2f);
		Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(Curves[_currentCurveIndex].ExitDistance + PerfectReleaseDistance), 0.2f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(Curves[_currentCurveIndex].ExitDistance - GreatReleaseDistance), 0.2f);
		Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(Curves[_currentCurveIndex].ExitDistance + GreatReleaseDistance), 0.2f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(Curves[_currentCurveIndex].ExitDistance - ReleaseDistanceCheck), 0.2f);
		Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(Curves[_currentCurveIndex].ExitDistance + ReleaseDistanceCheck), 0.2f);
	}
}
