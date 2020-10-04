using PathCreation;
using UnityEngine;
using UnityEngine.UI;

public enum CurveOutcome
{
	Crash,
	Miss,
	Great,
	Perfect
}

public class LoopManager : MonoBehaviour
{
	public PathCreator PathCreator;
	public Car Car;

	public Image Scorebar;

	public float ScoreNeededForLevel;
	public float ScoreDecreaseRate;

	private float _currentScore;

	public void Update()
	{
		_currentScore -= ScoreDecreaseRate * Time.deltaTime;

		if (_currentScore < 0)
		{
			_currentScore = 0;
		}

		UpdateUI();
	}

	private void UpdateUI()
	{
		var barFraction = ScoreNeededForLevel / 40f; // My graphic has 40 slots

		int barSlots = (int)(_currentScore / barFraction);

		Scorebar.fillAmount = (barSlots * barFraction) / ScoreNeededForLevel;

		//Scorebar.fillAmount = _currentScore / ScoreNeededForLevel;
	}

	public void CurveDone(CurveOutcome outcome)
	{
		switch (outcome)
		{
			case CurveOutcome.Crash:
				_currentScore -= Car.MaxSpeed * 0.2f;
				break;

			case CurveOutcome.Miss:
				break;

			case CurveOutcome.Great:
				_currentScore += Car.CurrentSpeed * 0.5f;
				break;

			case CurveOutcome.Perfect:
				_currentScore += Car.CurrentSpeed;
				break;
		}

		UpdateUI();
	}

	public void NewLap()
	{
		if (_currentScore >= ScoreNeededForLevel)
		{
			Debug.Log("Victory!");
		}
	}
}
