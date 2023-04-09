using System;
using Enums;
using Managers;
using UnityEngine;
using Enums;

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
		protected override void Start()
		{
			target = GameManager.Instance.Player.transform;
			waypointRoot = GameManager.Instance.DimensionController.WaypointRoot;
			base.Start();
		}

		protected override void Update()
		{
			base.Update();
			Vector3 rotatedDirection = Quaternion.Euler(-Model.eulerAngles) * direction;

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
		
		public void appleTouch(Dimension dimension)
		{
			print(dimension);
			Transform parent = GameManager.Instance.DimensionController.GetDimensionTransform(dimension);
			transform.SetParent(parent);
			
		}
	}
}