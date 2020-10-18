using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
	[SerializeField] private Vector3 rotation;

	private void Update()
	{
		transform.Rotate(rotation, rotation.magnitude * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		Block block = other.GetComponent<Block>();
		if (block)
		{
			block.DestroyBlock();
		}
	}
}
