using UnityEngine;

public class Machine : MonoBehaviour
{
	public Transform WheelRight;
	public Transform WheelLeft;

	public ParticleSystem[] Lightnings;

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

		foreach (var particleSystem in Lightnings)
		{
			var emission = particleSystem.emission;
			emission.rateOverTime = Random.Range(2, 4) + (LoopManager.CurrentScore / LoopManager.ScoreNeededForLevel) * 76;
		}
	}
}
