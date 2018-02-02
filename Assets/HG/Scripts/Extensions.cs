using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static Vector3 AddNoise(this Vector3 v, float noise)
	{
		return new Vector3(v.x + Random.Range(-noise, noise), v.y + Random.Range(-noise, noise), v.z + Random.Range(-noise, noise));
	}
}
