using System;
using System.Collections.Generic;
using Controllers;
using Enums;
using Unity.VisualScripting;
using UnityEngine;
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

		public DimensionController GenerateLevel(int index)
		{
			Level level = Levels[index];
		
			Transform parent = new GameObject($"Level {index}").transform;
			DimensionController dimensionController = parent.AddComponent<DimensionController>();
			dimensionController.Setup(level.Start);

			Transform red = GenerateDimension(Dimension.Red, level.Red);
			Transform green = GenerateDimension(Dimension.Green, level.Green);
			Transform blue = GenerateDimension(Dimension.Blue, level.Blue);
		
			red.SetParent(parent);
			green.SetParent(parent);
			blue.SetParent(parent);

			if(level.Start is not Dimension.Red)
				red.gameObject.SetActive(false);
			if(level.Start is not Dimension.Green)
				green.gameObject.SetActive(false);
			if(level.Start is not Dimension.Blue)
				blue.gameObject.SetActive(false);

			dimensionController.OnDimensionChange += (previous, next) =>
			{
				switch(previous)
				{
					case Dimension.Red:
						red.gameObject.SetActive(false);
						break;
					case Dimension.Blue:
						blue.gameObject.SetActive(false);
						break;
					case Dimension.Green:
						green.gameObject.SetActive(false);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(previous), previous, null);
				}
			
				switch(next)
				{
					case Dimension.Red:
						red.gameObject.SetActive(true);
						break;
					case Dimension.Blue:
						blue.gameObject.SetActive(true);
						break;
					case Dimension.Green:
						green.gameObject.SetActive(true);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(previous), previous, null);
				}
			};
		
			return dimensionController;
		}

		private Transform GenerateDimension(Dimension dimension, TextAsset data)
		{
			Transform parent = new GameObject($"{dimension}").transform;

			EntityType[,] grid = PatternConverter.convert<EntityType>(data.text);

			int lenX = grid.GetLength(0);
			int lenY = grid.GetLength(1);
		
			for(int y = 0; y < lenY; y++)
				for(int x = 0; x < lenX; x++)
				{
					Vector3 position = new Vector3(x - lenX / 2f, 0, lenY - y - lenY / 2f);
					Vector3 angle = new Vector3(0, Random.Range(0f, 360f), 0);
					switch(grid[x, y])
					{
						case EntityType.__:
							break;
						case EntityType.Block:
							Transform block = Instantiate(blocks[Random.Range(0, blocks.Length)], parent).transform;
							block.position = position;
							if(block.TryGetComponent(out Entity entity))
								entity.Model.eulerAngles = angle;
							break;
						case EntityType.Player:
							player.transform.position = position;
							break;
						case EntityType.Wolf:
							GameObject wolf = Instantiate(this.wolf, parent);
							wolf.transform.position = position;
							break;
						case EntityType.Lumberjack:
							GameObject lumberjack = Instantiate(this.lumberjack, parent);
							lumberjack.transform.position = position;
							break;
						case EntityType.RedTree:
							Tree redTree = Instantiate(this.redTree, parent);
							redTree.transform.position = position; 
							break;
						case EntityType.GreenTree:
							Tree greenTree = Instantiate(this.greenTree, parent);
							greenTree.transform.position = position; 
							break;
						case EntityType.BlueTree:
							Tree blueTree = Instantiate(this.blueTree, parent);
							blueTree.transform.position = position; 
							break;
						case EntityType.Key:
							Key key = Instantiate(this.key, parent);
							key.transform.position = position;
							key.Model.eulerAngles = angle;
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