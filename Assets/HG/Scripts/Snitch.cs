using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Snitch : MonoBehaviour
{
	[Range(1f, 20f)]
	public float Speed = 10f;

	[Range(1f, 20f)]
	public float SightDistance = 10f;

	public bool AfraidOfPlayers;

	Rigidbody rigidbody;
	SphereCollider collider;

	public bool playing { get; private set; }
	Vector3 prevVelocity;

	Vector3 startLocation;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		startLocation = transform.position;
	}

	void FixedUpdate()
	{
		if (playing)
		{
			MoveAwayFromThings();
		}
		else
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.Sleep();
		}
	}

	void MoveAwayFromThings()
	{
		Collider[] hits;

		if (AfraidOfPlayers)
			hits = Physics.OverlapSphere(transform.position, SightDistance, (1 << LayerMask.NameToLayer("Field")) | (1 << LayerMask.NameToLayer("Player")));

		else
			hits = Physics.OverlapSphere(transform.position, SightDistance, 1 << LayerMask.NameToLayer("Field"));

		Vector3 away = SmallestAwayVector(hits);

		transform.forward = Vector3.Lerp(transform.forward, away.AddNoise(1f).normalized, Time.deltaTime * 10f);
		rigidbody.AddForce(transform.forward * ((hits.Length > 0) ? Speed : Speed * 0.5f), ForceMode.Acceleration);
	}

	Vector3 SmallestAwayVector(Collider[] others)
	{
		float dist = float.MaxValue;
		Vector3 closest = Vector3.zero;

		foreach (Collider other in others)
		{
			Vector3 point = other.ClosestPointOnBounds(transform.position);
			float otherdist = Vector3.Distance(transform.position, point);
			if (otherdist > 0f && otherdist < dist)
			{
				dist = otherdist;
				closest = transform.position - point;
			}
		}

		return closest;
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Player>() != null)
		{
			SetRandomLocation();
		}
	}

	public void SetRandomLocation()
	{
		Vector3 newPos = startLocation + Random.insideUnitSphere * 20f;
		newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, startLocation.y - 1f, startLocation.y + 1f), newPos.z);
		transform.position = newPos;
		transform.forward = Vector3.up;
		rigidbody.velocity = Vector3.zero;
	}

	public void SetPlaying(bool value)
	{
		playing = value;

		if (playing)
		{
			transform.forward = Vector3.up;
			transform.position = startLocation;
		}
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

}
