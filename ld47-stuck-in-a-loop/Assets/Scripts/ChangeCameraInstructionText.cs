using TMPro;
using UnityEngine;

public class ChangeCameraInstructionText : MonoBehaviour
{
	public TextMeshProUGUI Text;
	public MobileDetect MobileDetect;

	public void Start()
	{
		if (MobileDetect.isMobile())
		{
			Text.text = "[Two-Finger Touch] Change Camera";
		}
		else
		{
			Text.text = "[C] Change Camera";
		}
	}
}
