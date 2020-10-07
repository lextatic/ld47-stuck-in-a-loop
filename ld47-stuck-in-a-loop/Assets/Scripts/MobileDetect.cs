using System.Runtime.InteropServices;
using UnityEngine;

public class MobileDetect : MonoBehaviour
{
	[DllImport("__Internal")]
	private static extern bool IsMobile();


	//Whether your webgl is being playing on mobile devices or not.
	public bool isMobile()
	{
#if !UNITY_EDITOR && UNITY_WEBGL
		return IsMobile();
#endif

		return false;
	}
}
