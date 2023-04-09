using System;
using Enums;
using Managers;
using UnityEngine;
using Enums;
using Waypoint_System.Scripts;

namespace Controllers
{
	public class Wolf : Follower2D
	{
		[SerializeField]
		private Animator animator;

		private Direction moveDirection = Direction.Down;
		[SerializeField]
		private GameObject Puf;
		[SerializeField]
		private GameObject PufSpawn;

		private Dimension _dimension = Dimension.Red;

		private void OnEnable()
		{
			ChangeDimension(_dimension);
			ChangeRoute();
		}

		protected override void Start()
		{
			target = GameManager.Instance.Player.transform;
			base.Start();
		}

		protected override void Update()
		{
			base.Update();
			Vector3 rotatedDirection = Quaternion.Euler(-Model.eulerAngles) * direction;
			
			animator.SetBool("Walk", (_movePosition - transform.position).magnitude > 0.1f);
			
			if(Mathf.Abs(rotatedDirection.z) > Mathf.Abs(rotatedDirection.x))
			{
				if(rotatedDirection.z > 0 && moveDirection is not Direction.Up)
				{
					moveDirection = Direction.Up;
					animator.SetTrigger("Up");
				}
				else if(rotatedDirection.z < 0 && moveDirection is not Direction.Down)
				{
					moveDirection = Direction.Down;
					animator.SetTrigger("Down");
				}
			} 
			else
			{
				if(rotatedDirection.x > 0 && moveDirection is not Direction.Right)
				{
					moveDirection = Direction.Right;
					animator.SetTrigger("Right");
				}
				else if(rotatedDirection.x < 0 && moveDirection is not Direction.Left)
				{
					moveDirection = Direction.Left;
					animator.SetTrigger("Left");
				}
			}
		}
		
		public void ChangeDimension(Dimension dimension)
		{
			Transform parent = GameManager.Instance.DimensionController.GetDimensionTransform(dimension);
			transform.SetParent(parent);
			waypointRoot = GameManager.Instance.DimensionController.GetWaypointRoot(dimension);
			animator.SetFloat("Dimension", (int)dimension / 3f);
			_dimension = dimension;
		}
	}
}