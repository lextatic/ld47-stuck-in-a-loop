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
	public GameObject ScorebarGlow;

	public float ScoreNeededForLevel;
	public float ScoreDecreaseRate;

	private float _currentScore;
	private float _barFraction;

	public void Start()
	{
		_barFraction = ScoreNeededForLevel / 40f; // My graphic has 40 slots
		ScorebarGlow.SetActive(false);
	}

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
		int barSlots = (int)(_currentScore / _barFraction);

		Scorebar.fillAmount = (barSlots * _barFraction) / ScoreNeededForLevel;

		ScorebarGlow.SetActive(_currentScore >= ScoreNeededForLevel);
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
