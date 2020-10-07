using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public GameObject FixedCamera;
	public GameObject FollowCamera;
	public MobileDetect MobileDetect;

	private float _touchCooldown;
	private bool fixedCamera;

	public void Start()
	{
		fixedCamera = true;
		FollowCamera.SetActive(false);
		FixedCamera.SetActive(true);
		_touchCooldown = 0f;
	}

	public void Update()
	{
		if (MobileDetect.isMobile())
		{
			if (Input.touchCount > 1 && Time.time >= _touchCooldown)
			{
				ChangeCamera();
				_touchCooldown = Time.time + 1f;
			}
		}
		else if (Input.GetKeyDown(KeyCode.C))
		{
			ChangeCamera();
		}
	}

	private void ChangeCamera()
	{
		fixedCamera = !fixedCamera;
		FollowCamera.SetActive(!fixedCamera);
		FixedCamera.SetActive(fixedCamera);
	}
}
