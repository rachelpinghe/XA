using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
	public Transform target;
	public Vector3 offset;
	public float ease_factor = 0.1f;

	public bool look_in_direction_of_movement = false;

	Vector3 previous_position;

	void LateUpdate()
	{
		if (target == null)
			return;

		transform.position = Vector3.Lerp(transform.position, target.position + offset, ease_factor);

		if (look_in_direction_of_movement)
		{
			// Calculate movement since last frame
			Vector3 change_in_position = GetComponent<Transform>().position - previous_position;
			Vector3 change_in_position_without_y = new Vector3(change_in_position.x, 0, change_in_position.z);
			if (change_in_position_without_y.magnitude > 0.0001f)
			{
				transform.forward = change_in_position_without_y;
			}
		}

		// At end of frame, remember the previous position
		previous_position = GetComponent<Transform>().position;
	}
}
