using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Snitch : MonoBehaviour
{
	[Range(1f, 20f)]
	public float Speed = 10f;

	[Range(1f, 20f)]
	public float SightDistance = 10f;

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
		//RaycastHit hit;
		//if (rigidbody.SweepTest(transform.forward, out hit, SightDistance))
		//{
		//	Vector3 away = (transform.position - hit.point).normalized;
		//	transform.forward = away;
		//	rigidbody.AddForce(away * Speed, ForceMode.Acceleration);
		//	Debug.DrawLine(transform.position, transform.position + away, Color.cyan, 5f);
		//}

		Vector3 closest = Vector3.one * 1000f;

		Collider[] result = Physics.OverlapSphere(transform.position, SightDistance);

		foreach (Collider other in result)
		{
			if (other == collider)
				continue;

			Vector3 away = transform.position - other.ClosestPoint(transform.position);

			if (away == Vector3.zero)
				continue;

			if (away.magnitude < closest.magnitude)
				closest = away;
		}


		if (result.Length > 0)
		{
			transform.forward = Vector3.Lerp(transform.forward, closest.normalized, Time.deltaTime * 10f);
			rigidbody.AddForce(closest.normalized * Speed, ForceMode.Acceleration);
		}

		
		//RaycastHit hitInfo;
		//if (Physics.SphereCast(transform.position, SightDistance, transform.forward, out hitInfo))
		//{
		//	Vector3 away = hitInfo.normal;
		//	transform.forward = away;
		//	rigidbody.AddForce(away * Speed, ForceMode.Acceleration);
		//	Debug.DrawLine(transform.position, transform.position + away, Color.cyan, 5f);
		//}
	}

	void OnCollisionEnter(Collision other)
	{

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
