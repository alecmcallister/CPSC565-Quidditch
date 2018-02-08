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

	void Update()
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
		Vector3 closest = Vector3.one * 1000f;

		Collider[] hits;

		if (AfraidOfPlayers)
		{
			hits = Physics.OverlapSphere(transform.position, SightDistance, (1 << LayerMask.NameToLayer("Field")) | (1 << LayerMask.NameToLayer("Player")));
		}
		else
		{
			hits = Physics.OverlapSphere(transform.position, SightDistance, 1 << LayerMask.NameToLayer("Field"));
		}

		foreach (Collider other in hits)
		{
			Vector3 away = transform.position - other.ClosestPoint(transform.position);

			if (away == Vector3.zero)
				continue;

			if (away.magnitude < closest.magnitude)
				closest = away;
		}

		closest = (hits.Length > 0) ? closest.normalized : transform.forward;

		transform.forward = Vector3.Lerp(transform.forward, closest.AddNoise(4f).normalized, Time.deltaTime * 10f);
		rigidbody.AddForce(transform.forward * Speed, ForceMode.Acceleration);
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
		rigidbody.velocity = Vector3.zero;
		Vector3 newPos = startLocation + Random.insideUnitSphere * 20f;
		newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, startLocation.y - 1f, startLocation.y + 1f), newPos.z);
		transform.position = newPos;
		transform.forward = Vector3.up;
	}

	public void SetPlaying(bool value)
	{
		playing = value;

		if (playing)
			transform.position = startLocation;
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
