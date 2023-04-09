using System;
using System.Collections.Generic;
using Controllers;
using Enums;
using Unity.VisualScripting;
using UnityEngine;
using Waypoint_System.Scripts;
using Random = UnityEngine.Random;
using Tree = Controllers.Tree;

namespace Managers {
	public class LevelGenerator : Helpers.Singleton<LevelGenerator>
	{
		[field: SerializeField]
		public List<Level> Levels { get; private set; } = new List<Level>();
	
	

		[SerializeField]
		private Player player;

		[SerializeField]
		private GameObject wolf;
	
		[SerializeField]
		private GameObject lumberjack;
	
		[SerializeField]
		private Tree redTree;
	
		[SerializeField]
		private Tree greenTree;
	
		[SerializeField]
		private Tree blueTree;

		[SerializeField]
		private GameObject[] blocks;

		[SerializeField]
		private Key key;

		[SerializeField]
		private Door door;

		public void GenerateLevel(int index)
		{
			Level level = Levels[index];
		
			Transform parent = new GameObject($"Level {index}").transform;
			DimensionController dimensionController = parent.AddComponent<DimensionController>();
			dimensionController.Setup(level.Start);
			GameManager.Instance.SetDimensionController(dimensionController);

			Transform red = GenerateDimension(Dimension.Red, level, ref dimensionController);
			Transform green = GenerateDimension(Dimension.Green, level, ref dimensionController);
			Transform blue = GenerateDimension(Dimension.Blue, level, ref dimensionController);
		
			red.SetParent(parent);
			green.SetParent(parent);
			blue.SetParent(parent);

			if(level.Start is not Dimension.Red)
				red.gameObject.SetActive(false);
			if(level.Start is not Dimension.Green)
				green.gameObject.SetActive(false);
			if(level.Start is not Dimension.Blue)
				blue.gameObject.SetActive(false);
			
		}

		private Transform GenerateDimension(Dimension dimension, Level level, ref DimensionController dimensionController)
		{
			Transform parent = new GameObject($"{dimension}").transform;
			
			dimensionController.OnDimensionChange += (previous, next) =>
			{
				if(previous == dimension)
					parent.gameObject.SetActive(false);
				
				if(next == dimension)
					parent.gameObject.SetActive(true);
			};
			

			EntityType[,] grid = PatternConverter.convert<EntityType>(dimension switch {
				Dimension.Red => level.Red.text, 
				Dimension.Green => level.Green.text,
				Dimension.Blue => level.Blue.text
			});
			
			if(dimension == level.Start)
				GameManager.Instance.SetGridSize(grid.GetLength(0), grid.GetLength(1));
			
			Waypoint[,] waypoints = new Waypoint[grid.GetLength(0), grid.GetLength(1)];

			int lenX = grid.GetLength(0);
			int lenY = grid.GetLength(1);

			WaypointRoot waypointRoot = new GameObject("Waypoint Root").AddComponent<WaypointRoot>();
			waypointRoot.transform.SetParent(parent);
			
			dimensionController.SetWaypointRoot(dimension, waypointRoot);
			
			for(int y = 0; y < lenY; y++)
				for(int x = 0; x < lenX; x++)
				{
					Vector3 position = new Vector3(x - lenX / 2f, 0, (lenY - y - 1) - lenY / 2f);
					Vector3 angle = new Vector3(0, Random.Range(0f, 360f), 0);
					switch(grid[x, y])
					{
						case EntityType.__:
							CreateWaypoint(x, y, ref waypoints, ref waypointRoot, position);
							dimensionController.SetEmpty(x, y, dimension);
							break;
						case EntityType.Block:
							Transform block = Instantiate(blocks[Random.Range(0, blocks.Length)], parent).transform;
							block.position = position;
							if(block.TryGetComponent(out Entity entity))
								entity.Model.eulerAngles = angle;
							break;
						case EntityType.Player:
							if(level.Start == dimension)
								player.transform.position = position;
							dimensionController.SetSpawnIndex(new Vector2Int(x, y), dimension);
							CreateWaypoint(x, y, ref waypoints, ref waypointRoot, position);
							dimensionController.SetEmpty(x, y, dimension);
							
							break;
						case EntityType.Wolf:
							GameObject wolf = Instantiate(this.wolf, parent);
							wolf.transform.position = position;
							CreateWaypoint(x, y, ref waypoints, ref waypointRoot, position);
							dimensionController.SetEmpty(x, y, dimension);
							break;
						case EntityType.Lumberjack:
							GameObject lumberjack = Instantiate(this.lumberjack, parent);
							lumberjack.transform.position = position;
							CreateWaypoint(x, y, ref waypoints, ref waypointRoot, position);
							dimensionController.SetEmpty(x, y, dimension);
							break;
						case EntityType.RedTree:
							Tree redTree = Instantiate(this.redTree, parent);
							redTree.transform.position = position; 
							CreateWaypoint(x, y, ref waypoints, ref waypointRoot, position);
							dimensionController.SetEmpty(x, y, dimension);
							break;
						case EntityType.GreenTree:
							Tree greenTree = Instantiate(this.greenTree, parent);
							greenTree.transform.position = position; 
							CreateWaypoint(x, y, ref waypoints, ref waypointRoot, position);
							dimensionController.SetEmpty(x, y, dimension);
							break;
						case EntityType.BlueTree:
							Tree blueTree = Instantiate(this.blueTree, parent);
							blueTree.transform.position = position; 
							CreateWaypoint(x, y, ref waypoints, ref waypointRoot, position);
							dimensionController.SetEmpty(x, y, dimension);
							break;
						case EntityType.Key:
							Key key = Instantiate(this.key, parent);
							key.transform.position = position;
							key.Model.eulerAngles = angle;
							CreateWaypoint(x, y, ref waypoints, ref waypointRoot, position);
							dimensionController.SetEmpty(x, y, dimension);
							break;
						case EntityType.Door:
							Door door = Instantiate(this.door, parent);
							door.transform.position = position;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

			return parent;
		}

		private void CreateWaypoint(int x, int y, ref Waypoint[,] waypoints, ref WaypointRoot waypointRoot, Vector3 position)
		{
			Waypoint waypoint = new GameObject("Waypoint").AddComponent<Waypoint>();
			waypoint.Radius = 0.2f;
			waypoints[x, y] = waypoint;
			waypoint.transform.SetParent(waypointRoot.transform);
			waypoint.transform.position = position;

			if(x > 0 && waypoints[x - 1, y])
			{
				waypoint.Connect(waypoints[x - 1, y]);
				waypoints[x - 1, y].Connect(waypoint);
			}

			if(y > 0 && waypoints[x, y - 1])
			{
				waypoint.Connect(waypoints[x, y - 1]);
				waypoints[x, y - 1].Connect(waypoint);
			}

			if(x > 0 && y > 0 && waypoints[x - 1, y - 1])
			{
				waypoint.Connect(waypoints[x - 1, y - 1]);
				waypoints[x - 1, y - 1].Connect(waypoint);
			}
		}
		private void SetEmpty(int x, int y, ref DimensionController dimensionController, Dimension dimension)
		{
			
		}
	}

	[Serializable]
	public struct Level
	{
		public Dimension Start;
		public TextAsset Red;
		public TextAsset Green;
		public TextAsset Blue;
	}
}