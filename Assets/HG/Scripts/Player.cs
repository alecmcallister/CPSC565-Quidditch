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

	public bool paused { get; private set; }
	Vector3 prevVelocity;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if (playing && !paused)
		{
			MoveToTarget(FieldManager.Instance.Snitch.gameObject);
		}
	}

	public void Init(Team team, PlayerStats stats)
	{
		Team = team;
		Stats = stats;
		GetComponent<MeshRenderer>().material.color = Team.TeamColor;
		transform.localPosition = UnityEngine.Random.insideUnitSphere * 3f;
	}

	public void SetPlaying(bool value)
	{
		playing = value;
	}

	public void SetPaused(bool value)
	{
		paused = value;

		if (paused)
		{
			prevVelocity = rigidbody.velocity;
			rigidbody.isKinematic = true;
		}
		else
		{
			rigidbody.isKinematic = false;
			rigidbody.velocity = prevVelocity;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.name == "Floor")
		{
			rigidbody.velocity = Vector3.zero;
			transform.localPosition = UnityEngine.Random.insideUnitSphere * 3f;
			playing = true;
			rigidbody.useGravity = false;
			return;
		}

		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			Player otherPlayer = other.gameObject.GetComponent<Player>();

			if (otherPlayer.Team != Team)
			{
				//rigidbody.AddForce(other.relativeVelocity, ForceMode.Impulse);

				if (UnityEngine.Random.Range(0f, 1f) < Stats.TacklingProb)
				{
					otherPlayer.GotTackled();
				}
			}
		}

		if (!playing)
			return;

		if (other.gameObject.layer == LayerMask.NameToLayer("Snitch"))
		{
			Snitch s = other.gameObject.GetComponent<Snitch>();

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
		Vector3 toTarget = (target.transform.position - transform.position).AddNoise(2f).normalized;

		bool atMaxVelocity = rigidbody.velocity.magnitude > Stats.MaxVelocity;
		float dot = Vector3.Dot(rigidbody.velocity, toTarget);

		if (!atMaxVelocity || dot < 0.5f)
		{
			transform.forward = Vector3.Lerp(transform.forward, toTarget, Time.deltaTime * 5f);
			rigidbody.AddForce(transform.forward * Stats.MaxAcceleration, ForceMode.Acceleration);
		}
	}

	public void GotTackled()
	{
		playing = false;
		rigidbody.useGravity = true;
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
