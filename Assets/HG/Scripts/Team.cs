using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
	public event Action TeamScoredEvent = new Action(() => { });

	public string TeamName;
	public Color TeamColor;

	[Range(0, 30)]
	public int PlayerCount = 20;

	[Range(1f, 20f)]
	public float MaxVelocity = 10f;

	[Range(1f, 20f)]
	public float MaxAcceleration = 10f;

	[Range(0f, 1f)]
	public float TacklingProb = 0.5f;

	public Transform StartingPoint;

	public List<Player> Players { get; private set; }
	public int Score { get; private set; }

	GameObject playerPrefab;

	void Awake()
	{
		Players = new List<Player>();
		playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
	}

	public void StartGame()
	{
		Score = 0;

		for (int i = StartingPoint.childCount - 1; i > -1; i--)
			Destroy(StartingPoint.GetChild(i).gameObject);

		Players.Clear();

		CreatePlayers();
	}

	public void StopGame()
	{
		foreach (Player player in Players)
		{
			player.SetPlaying(false);
		}
	}

	public void SetPause(bool value)
	{
		foreach (Player player in Players)
		{
			player.SetPaused(value);
		}
	}

	public void CreatePlayers()
	{
		for (int i = 0; i < PlayerCount; i++)
		{
			PlayerStats stats = new PlayerStats(i, MaxVelocity, MaxAcceleration, TacklingProb);

			GameObject playerObj = Instantiate(playerPrefab, StartingPoint);

			Player player = playerObj.GetComponent<Player>();
			player.Init(this, stats);
			player.PlayerScoredEvent += PlayerScoredEvent;
			player.SetPlaying(true);

			Players.Add(player);
		}
	}

	public void PlayerScoredEvent(int id, int points)
	{
		if (Score < FieldManager.Instance.MaxPointsForWin)
		{
			Score += points;
			TeamScoredEvent();
		}
	}
}
