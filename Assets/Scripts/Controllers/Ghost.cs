using System;
using Enums;
using Managers;
using UnityEngine;
using Waypoint_System.Scripts;

namespace Controllers
{
	public class Ghost : Entity2D
	{
		[field:SerializeField]
		public Dimension Dimension { get; private set; }

		//protected override Vector3 TargetPosition => GameManager.Instance.DimensionController.GetSpawnPoint(EntityType.Player, Dimension);
		
		public void SetWaypointRoot(WaypointRoot waypointRoot)
		{
			//this.waypointRoot = waypointRoot;
		}

		private void Update()
		{
			transform.position = GameManager.Instance.DimensionController.GetSpawnPoint(EntityType.Player, Dimension);
			//TODO: Optimizde edilecek
			Model.gameObject.SetActive(Dimension switch {
				Dimension.Red => GameManager.Instance.Player.RedApple > 0,
				Dimension.Green => GameManager.Instance.Player.GreenApple > 0,
				Dimension.Blue => GameManager.Instance.Player.BlueApple > 0
		});
		}

		public override void OnDimensionChange(Dimension previous, Dimension next)
		{
			if(previous == Dimension)
				gameObject.SetActive(true);
			
			
			if(next == Dimension)
				gameObject.SetActive(false);
			
			base.OnDimensionChange(previous, next);
		}
	}
}