using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public GameObject FixedCamera;
	public GameObject FollowCamera;

	private bool fixedCamera;

	public void Start()
	{
		fixedCamera = true;
		FollowCamera.SetActive(false);
		FixedCamera.SetActive(true);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			fixedCamera = !fixedCamera;
			FollowCamera.SetActive(!fixedCamera);
			FixedCamera.SetActive(fixedCamera);
		}
	}
}
