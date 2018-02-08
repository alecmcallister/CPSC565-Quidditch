using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	public event Action<int, int> PlayerScoredEvent = new Action<int, int>((id, score) => { });

	public Team Team { get; private set; }
	public PlayerStats Stats { get; private set; }

	Rigidbody rigidbody;

	public bool playing { get; private set; }
	Vector3 prevVelocity;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if (playing)
			MoveToTarget(FieldManager.Instance.Snitch.gameObject);
		else
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.Sleep();
		}
	}

	public void Init(Team team, PlayerStats stats)
	{
		Team = team;
		Stats = stats;
		GetComponent<MeshRenderer>().material.color = Team.TeamColor;
		transform.localPosition = UnityEngine.Random.insideUnitSphere * 2f;
	}

	public void SetPlaying(bool value)
	{
		playing = value;
	}

	public void SetPaused(bool value)
	{
		if (value)
		{
			//Pause
			playing = false;
			prevVelocity = rigidbody.velocity;
		}
		else
		{
			// Resume
			playing = true;
			rigidbody.velocity = prevVelocity;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		foreach (ContactPoint point in other.contacts)
		{
			Snitch s = point.otherCollider.gameObject.GetComponent<Snitch>();

			if (s)
			{
				PlayerScoredEvent(Stats.PlayerID, 1);
				transform.position = Team.StartingPoint.position;
				rigidbody.velocity = Vector3.zero;
			}
		}
	}

	void MoveToTarget(GameObject target)
	{
		// Change to add more interesting noise
		Vector3 toTarget = (target.transform.position - transform.position).AddNoise(2f).normalized;

		bool atMaxVelocity = rigidbody.velocity.magnitude > Stats.MaxVelocity;
		float dot = Vector3.Dot(rigidbody.velocity, toTarget);

		if (!atMaxVelocity || dot < 0.5f)
		{
			transform.forward = Vector3.Lerp(transform.forward, toTarget, Time.deltaTime * 5f);
			rigidbody.AddForce(transform.forward * Stats.MaxAcceleration, ForceMode.Acceleration);
		}
	}
}

public class PlayerStats
{
	public int PlayerID { get; private set; }
	public float MaxVelocity { get; private set; }
	public float MaxAcceleration { get; private set; }
	public float TacklingProb { get; private set; }

	public PlayerStats(int playerID, float maxVelocity, float maxAcceleration, float tacklingProb)
	{
		PlayerID = playerID;
		MaxVelocity = maxVelocity;
		MaxAcceleration = maxAcceleration;
		TacklingProb = tacklingProb;
	}
}
