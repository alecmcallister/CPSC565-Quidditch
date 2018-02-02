using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowThatObject : MonoBehaviour
{
	public Transform Follow;

	[Range(0f, 20f)]
	public float LerpSpeed = 10f;

	Vector3 offset;

	void Awake()
	{
		if (!Follow)
			enabled = false;

		offset = transform.position - Follow.position;
	}

	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, Follow.position + offset, Time.deltaTime * LerpSpeed);
	}
}
