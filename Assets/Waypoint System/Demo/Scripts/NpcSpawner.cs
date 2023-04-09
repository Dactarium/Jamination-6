using UnityEngine;
using Waypoint_System.Scripts;

namespace Waypoint_System.Demo.Scripts
{
	public class NpcSpawner : MonoBehaviour
	{
		public GameObject[] Npc; 
		[Tooltip("You dont have to set spawn waypoints.\nA random child will be assigned.")]
		public Waypoint[] spawnPoint;
		public int SpawnCount = 1;
		public float NpcSpeed = 1f;
		void Awake()
		{
			Spawn(SpawnCount);
		}

		public void Spawn() => Spawn(1);
		public void Spawn(int spawnCount) => Spawn(spawnCount, -1);
		public void Spawn(int spawnCount, int index){
			for(int i = 0; i < spawnCount; i++){
            
				GameObject spawned = Instantiate(Npc[(index > 0)? index: Random.Range(0, Npc.Length)]);
				spawned.GetComponent<Npc>().CurrentWaypoint = (spawnPoint == null || spawnPoint.Length == 0)? transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Waypoint>(): spawnPoint[Random.Range(0, spawnPoint.Length)];
				spawned.GetComponent<Npc>().Speed = NpcSpeed;
			}
		}
	}
}
