using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	public Vector3 rotation_amount;
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(rotation_amount * Time.deltaTime * 60.0f);
	}
}
