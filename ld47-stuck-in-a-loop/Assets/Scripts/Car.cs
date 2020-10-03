using PathCreation;
using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
	public PathCreator PathCreator;
	public float MaxSpeed = 10f;
	public float Acceleration = 5f;
	public float Deceleration = 1f;

	public MeshCollider RoadMeshCollider;

	public Transform[] Wheels;
	public Transform[] TurnAxis;

	private bool _isCrashing;
	private HingeJoint _hingeJoin;
	private Rigidbody _rigidbody;
	private Rigidbody _attachedBody;

	public float DistanceTraveled { get; private set; }
	public float CurrentSpeed { get; private set; }

	private void Start()
	{
		RoadMeshCollider.enabled = false;
		_isCrashing = false;
		DistanceTraveled = 0;
		CurrentSpeed = 0;
		_hingeJoin = GetComponent<HingeJoint>();
		_rigidbody = GetComponent<Rigidbody>();
		_attachedBody = _hingeJoin.connectedBody;
		_rigidbody.MovePosition(PathCreator.path.GetPointAtDistance(DistanceTraveled));
	}

	public void Update()
	{
		var rotationSpeed = (_isCrashing ? MaxSpeed : CurrentSpeed) * -360 * Time.deltaTime;

		foreach (var wheel in Wheels)
		{
			wheel.Rotate(0, rotationSpeed, 0);
		}

		foreach (var turnAxis in TurnAxis)
		{
			turnAxis.rotation = PathCreator.path.GetRotationAtDistance(DistanceTraveled + .2f);
		}
	}

	public void FixedUpdate()
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

		_rigidbody.MovePosition(PathCreator.path.GetPointAtDistance(DistanceTraveled) + new Vector3(0, 0.08f, 0));
		_rigidbody.MoveRotation(PathCreator.path.GetRotationAtDistance(DistanceTraveled));

		//transform.position = PathCreator.path.GetPointAtDistance(DistanceTraveled);
		//transform.rotation = PathCreator.path.GetRotationAtDistance(DistanceTraveled);
	}

	public void Crash(float distance)
	{
		_isCrashing = true;

		StartCoroutine(CrashSequence(distance));
	}



	private IEnumerator CrashSequence(float distance)
	{
		// Animate crash
		RoadMeshCollider.enabled = true;
		_hingeJoin.connectedBody = null;
		_attachedBody.AddTorque(new Vector3(Random.Range(-1000f, 1000f), 0, Random.Range(-1000f, 1000f)), ForceMode.VelocityChange);
		_attachedBody.AddForce(0, 150, 0);

		yield return new WaitForSeconds(2f);

		RoadMeshCollider.enabled = false;

		// TODO: Resolve physics teleport

		CurrentSpeed = 0;
		DistanceTraveled = distance;
		UpdateCar();

		_attachedBody.transform.position = _rigidbody.position;
		_attachedBody.transform.rotation = _rigidbody.rotation;

		_attachedBody.velocity = Vector3.zero;
		_attachedBody.angularVelocity = Vector3.zero;
		_hingeJoin.connectedBody = _attachedBody;

		_isCrashing = false;
	}
}
