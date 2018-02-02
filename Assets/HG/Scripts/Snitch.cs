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

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
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
		RaycastHit hitInfo;
		if (Physics.SphereCast(transform.position, SightDistance, transform.forward, out hitInfo))
		{
			Vector3 away = hitInfo.normal;
			rigidbody.AddForce(away * Speed, ForceMode.Acceleration);
		}
	}

	void OnCollisionEnter(Collision other)
	{

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

}
