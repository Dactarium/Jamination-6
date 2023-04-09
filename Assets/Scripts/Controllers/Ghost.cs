using Enums;
using Managers;
using UnityEngine;
using Waypoint_System.Scripts;

namespace Controllers
{
	public class Ghost : Follower2D
	{
		[field:SerializeField]
		public Dimension Dimension { get; private set; }

		protected override Vector3 TargetPosition => GameManager.Instance.DimensionController.GetSpawnPoint(EntityType.Player, Dimension);
		
		public void SetWaypointRoot(WaypointRoot waypointRoot)
		{
			this.waypointRoot = waypointRoot;
		}
		

		public void OnDimensionChange(Dimension previous, Dimension next)
		{
			if(previous == Dimension)
			{
				transform.position = GameManager.Instance.DimensionController.GetSpawnPoint(EntityType.Player, Dimension);
				gameObject.SetActive(true);
			}
				
			
			if(next == Dimension)
				gameObject.SetActive(false);
		}
	}
}