using UnityEngine;

public class Machine : MonoBehaviour
{
	public Transform WheelRight;
	public Transform WheelLeft;

	public float MaxRotationSpeed;

	private LoopManager LoopManager;

	public void Start()
	{
		LoopManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LoopManager>();
	}

	public void Update()
	{
		WheelRight.Rotate(0, 5 + MaxRotationSpeed * LoopManager.CurrentScore, 0);
		WheelLeft.Rotate(0, -5 - MaxRotationSpeed * LoopManager.CurrentScore, 0);
	}
}
