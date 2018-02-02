using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; protected set; }

	public Transform UIRoot;

	Text t1ScoreText;
	Text t2ScoreText;

	Text t1TeamNameText;
	Text t2TeamNameText;

	Text pauseText;

	ScoreVisualizer visualizer;

	void Awake()
	{
		if (!Instance)
			Instance = this;

		if (!UIRoot)
			enabled = false;

		t1TeamNameText = UIRoot.Find("Info/Team1/TeamName").GetComponent<Text>();
		t2TeamNameText = UIRoot.Find("Info/Team2/TeamName").GetComponent<Text>();

		t1ScoreText = UIRoot.Find("Info/Team1/Score").GetComponent<Text>();
		t2ScoreText = UIRoot.Find("Info/Team2/Score").GetComponent<Text>();

		pauseText = UIRoot.Find("Buttons/PauseButton/Pause").GetComponent<Text>();

		visualizer = UIRoot.GetComponentInChildren<ScoreVisualizer>();
	}

	void Start()
	{
		Init();
	}

	void Init()
	{
		t1TeamNameText.text = FieldManager.Instance.Team1.TeamName;
		t2TeamNameText.text = FieldManager.Instance.Team2.TeamName;

		t1ScoreText.color = FieldManager.Instance.Team1.TeamColor;
		t2ScoreText.color = FieldManager.Instance.Team2.TeamColor;

		UpdateScoreText();
	}

	public void UpdateScoreText()
	{
		t1ScoreText.text = FieldManager.Instance.Team1.Score.ToString();
		t2ScoreText.text = FieldManager.Instance.Team2.Score.ToString();
		visualizer.Init(FieldManager.Instance.Team1.TeamColor, FieldManager.Instance.Team2.TeamColor, FieldManager.Instance.Team1.Score, FieldManager.Instance.Team2.Score);
	}

	public void StartPressed()
	{
		FieldManager.Instance.StartGame();
		UpdateScoreText();
	}

	public void StopPressed()
	{
		FieldManager.Instance.StopGame();
	}

	bool paused = false;

	public void PausePressed()
	{
		if (paused)
		{
			paused = false;
			pauseText.text = "Pause";
		}
		else
		{
			paused = true;
			pauseText.text = "Resume";
		}

		FieldManager.Instance.SetPause(paused);
	}
}