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

	public AudioClip VoicePerfectSound;
	public AudioClip VoiceGreatSound;
	public AudioClip VoiceMissSound;
	public AudioClip VoiceCrashSound;
	public AudioClip CuePerfectSound;
	public AudioClip CueGreatSound;
	public AudioClip CueMissSound;

	public AudioSource AudioSource;

	private float _barFraction;
	private bool _victory;

	public float CurrentScore { get; private set; }

	public void Start()
	{
		_barFraction = ScoreNeededForLevel / 40f; // My graphic has 40 slots
		ScorebarGlow.SetActive(false);
		_victory = false;
	}

	public void Update()
	{
		if (_victory) return;

		CurrentScore -= ScoreDecreaseRate * Time.deltaTime;

		if (CurrentScore < 0)
		{
			CurrentScore = 0;
		}

		UpdateUI();
	}

	private void UpdateUI()
	{
		int barSlots = (int)(CurrentScore / _barFraction);

		Scorebar.fillAmount = (barSlots * _barFraction) / ScoreNeededForLevel;

		ScorebarGlow.SetActive(CurrentScore >= ScoreNeededForLevel);
	}

	public void CurveDone(CurveOutcome outcome)
	{
		switch (outcome)
		{
			case CurveOutcome.Crash:
				CurrentScore -= Car.MaxSpeed * 0.2f;
				AnimateText(CrashText);
				PlaySound(VoiceCrashSound, 0.5f);
				break;

			case CurveOutcome.Miss:
				AnimateText(MissText);
				PlaySound(VoiceMissSound, 0.7f);
				PlaySound(CueMissSound, 0.2f);
				break;

			case CurveOutcome.Great:
				CurrentScore += Car.CurrentSpeed * 0.5f;
				AnimateText(GreatText);
				PlaySound(VoiceGreatSound, 0.7f);
				PlaySound(CueGreatSound, 0.2f);
				break;

			case CurveOutcome.Perfect:
				CurrentScore += Car.CurrentSpeed;
				AnimateText(PerfectText);
				PlaySound(VoicePerfectSound, 0.7f);
				PlaySound(CuePerfectSound, 0.2f);
				break;
		}

		UpdateUI();
	}

	private void PlaySound(AudioClip audioClip, float volume)
	{
		AudioSource.volume = volume;
		AudioSource.PlayOneShot(audioClip);
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
		if (CurrentScore >= ScoreNeededForLevel)
		{
			//Debug.Log("Victory!");
			Car.TimeWarp();
			_victory = true;
		}
	}
}
