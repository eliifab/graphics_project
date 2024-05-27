using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	public int degrees;
	public Transform obj;

	void Update () 
	{
		transform.RotateAround (obj.position, obj.up, degrees * Time.deltaTime);
	}
}
