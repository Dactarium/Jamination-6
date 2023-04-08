using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class Key : Entity
	{
		[field: SerializeField]
		public Dimension Dimension { get; private set; }

		[SerializeField]
		private float rotateSpeed = 10f;
		
		private void Update()
		{
			Vector3 angles = transform.eulerAngles;
			angles.y = (angles.y + rotateSpeed * Time.deltaTime) % 360f;
			transform.eulerAngles = angles;
		}
	}
}