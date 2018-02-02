using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
	public static FieldManager Instance { get; protected set; }

	public Team Team1;
	public Team Team2;

	[Range(1, 20)]
	public int MaxPointsForWin = 10;

	public Snitch Snitch;

	GameObject snitchPrefab;

	void Awake()
	{
		if (!Instance)
			Instance = this;

		snitchPrefab = Resources.Load<GameObject>("Prefabs/Snitch");

		Team1.TeamScoredEvent += TeamScored;
		Team2.TeamScoredEvent += TeamScored;
	}

	public void StartGame()
	{
		Team1.StartGame();
		Team2.StartGame();
		Snitch.SetPlaying(true);
	}

	public void StopGame()
	{
		Team1.StopGame();
		Team2.StopGame();
		Snitch.SetPlaying(false);
	}

	public void SetPause(bool value)
	{
		Team1.SetPause(value);
		Team2.SetPause(value);
	}

	public void TeamScored()
	{
		UIManager.Instance.UpdateScoreText();
	}
}
