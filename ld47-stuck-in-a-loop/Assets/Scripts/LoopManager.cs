using DG.Tweening;
using PathCreation;
using TMPro;
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

	public TextMeshProUGUI PerfectText;
	public TextMeshProUGUI GreatText;
	public TextMeshProUGUI MissText;
	public TextMeshProUGUI CrashText;

	private float _currentScore;
	private float _barFraction;
	private bool _victory;

	public void Start()
	{
		_barFraction = ScoreNeededForLevel / 40f; // My graphic has 40 slots
		ScorebarGlow.SetActive(false);
		_victory = false;
	}

	public void Update()
	{
		if (_victory) return;

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
				AnimateText(CrashText);
				break;

			case CurveOutcome.Miss:
				AnimateText(MissText);
				break;

			case CurveOutcome.Great:
				_currentScore += Car.CurrentSpeed * 0.5f;
				AnimateText(GreatText);
				break;

			case CurveOutcome.Perfect:
				_currentScore += Car.CurrentSpeed;
				AnimateText(PerfectText);
				break;
		}

		UpdateUI();
	}

	private void AnimateText(TextMeshProUGUI text)
	{
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(text.rectTransform.DOLocalMoveX(0, 0.2f))
			.AppendInterval(0.1f)
			.Append(text.rectTransform.DOLocalMoveX(20, 0.2f))
			.Append(text.rectTransform.DOLocalMoveX(-40, 0.1f));

		Sequence mySequence2 = DOTween.Sequence();
		mySequence2.Append(text.DOFade(1, 0.2f))
			.AppendInterval(0.1f)
			.Append(text.DOFade(0, 0.2f));
	}

	public void NewLap()
	{
		if (_currentScore >= ScoreNeededForLevel)
		{
			//Debug.Log("Victory!");
			Car.TimeWarp();
			_victory = true;
		}
	}
}
