using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
	void Update()
	{
		if (Input.anyKeyDown)
		{
			SceneManager.LoadScene(0);
		}
	}
}
