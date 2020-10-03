using PathCreation;
using UnityEngine;

public class CurveManager : MonoBehaviour
{
	public PathCreator PathCreator;
	public Car Car;

	public float PerfectReleaseDistance;
	public float GreatReleaseDistance;
	public float ReleaseDistanceCheck;

	public float CrashAccelerationThresholdTime = 0.5f;

	public GameObject MarkPrefab;

	public Curve[] Curves;

	private int _currentCurveIndex;
	private float _speedOverflow;
	private bool _crashCheck;
	private bool _waitingNewLap;

	public void Start()
	{
		_currentCurveIndex = 0;
		_speedOverflow = 0;
		_crashCheck = true;
		_waitingNewLap = false;

		foreach (var curve in Curves)
		{
			var mark1 = Instantiate(MarkPrefab);
			mark1.transform.position = PathCreator.path.GetPointAtDistance(curve.EntranceDistance);
			mark1.transform.rotation = PathCreator.path.GetRotationAtDistance(curve.EntranceDistance);

			var mark2 = Instantiate(MarkPrefab);
			mark2.transform.position = PathCreator.path.GetPointAtDistance(curve.ExitDistance);
			mark2.transform.rotation = PathCreator.path.GetRotationAtDistance(curve.ExitDistance);
		}
	}

	public void Update()
	{
		if (_waitingNewLap)
		{
			if (Car.DistanceTraveled < Curves[0].EntranceDistance)
			{
				_waitingNewLap = false;
			}
			else
			{
				return;
			}
		}
		CrashCheck();
		ReleaseCheck();
	}

	private void CrashCheck()
	{
		// Crash check
		if (_crashCheck && Car.DistanceTraveled > Curves[_currentCurveIndex].EntranceDistance)
		{
			if (Car.CurrentSpeed < Car.MaxSpeed * Curves[_currentCurveIndex].MaxSpeedThreshold)
			{
				_crashCheck = false;
			}

			if (Input.anyKey)
			{
				_speedOverflow += Time.deltaTime;
			}

			if (_speedOverflow > ((Curves[_currentCurveIndex].ExitDistance - Curves[_currentCurveIndex].EntranceDistance) / Car.MaxSpeed) * CrashAccelerationThresholdTime)
			{
				Car.Crash(Curves[_currentCurveIndex].ExitDistance);
				NextCurve();
			}
		}
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

				NextCurve();
			}
			else
			{
				if (distanceFromExitPoint > ReleaseDistanceCheck)
				{
					Debug.Log("Failure");
					NextCurve();
				}
			}
		}
	}

	private void NextCurve()
	{
		_currentCurveIndex++;

		if (_currentCurveIndex >= Curves.Length)
		{
			_currentCurveIndex = 0;
			_waitingNewLap = true;
		}

		_crashCheck = true;
		_speedOverflow = 0;
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

			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(curve.ExitDistance - PerfectReleaseDistance), 0.2f);
			Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(curve.ExitDistance + PerfectReleaseDistance), 0.2f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(curve.ExitDistance - GreatReleaseDistance), 0.2f);
			Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(curve.ExitDistance + GreatReleaseDistance), 0.2f);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(curve.ExitDistance - ReleaseDistanceCheck), 0.2f);
			Gizmos.DrawWireSphere(PathCreator.path.GetPointAtDistance(curve.ExitDistance + ReleaseDistanceCheck), 0.2f);
		}
	}
}
