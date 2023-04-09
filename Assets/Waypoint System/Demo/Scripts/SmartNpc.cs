using System.Collections.Generic;
using UnityEngine;
using Waypoint_System.Scripts;

namespace Waypoint_System.Demo.Scripts
{
	public class SmartNpc : Npc
	{
		[SerializeField] private Color _color;
		public bool PriorityMode;
		public string[] Tags;

		protected override void Start(){
			base.Start();

			_previousBias = 0f;
		}

		protected override void SetColor()
		{
			base.SetColor(_color);
		}

		protected override List<Waypoint> GetConnections(){
			List<Waypoint> connections = new List<Waypoint>();
        
			foreach(string tag in Tags){
				List<Waypoint> withTag = CurrentWaypoint.FindConnectionsWithTag(tag);

				if(withTag != null){
					connections.AddRange(withTag);
					if(PriorityMode) break;
				} 

			}
			if(connections.Count == 0) connections = CurrentWaypoint.Connections;

			connections.Remove(_previousWaypoint);
        
			if(connections.Count == 0) OnNoConnections();

			return connections;
		}

		void OnNoConnections(){
			CurrentWaypoint.transform.parent.GetComponent<NpcSpawner>().Spawn();
			Destroy(gameObject);
		}

    
	}
}
