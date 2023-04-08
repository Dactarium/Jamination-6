using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enums;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
	[field: SerializeField]
	public List<Level> Levels { get; private set; } = new List<Level>();
	
	[SerializeField]
	private int levelIndex = 0;

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
	private Key redKey;
	
	[SerializeField]
	private Key greenKey;
	
	[SerializeField]
	private Key blueKey;

	[SerializeField]
	private Door door;
	
	private void Start()
	{
		GenerateLevel(levelIndex);
	}

	private void GenerateLevel(int index)
	{
		Level level = Levels[index];
		
		Transform parent = new GameObject($"Level {index}").transform;

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
		
	}

	private Transform GenerateDimension(Dimension dimension, TextAsset data)
	{
		Transform parent = new GameObject($"{dimension}").transform;

		Entity[,] grid = PatternConverter.convert<Entity>(data.text);

		int lenX = grid.GetLength(0);
		int lenY = grid.GetLength(1);
		
		for(int y = 0; y < lenY; y++)
			for(int x = 0; x < lenX; x++)
			{
				Vector3 position = new Vector3(x - lenX / 2f, 0, lenY - y - lenY / 2f);
				switch(grid[x, y])
				{
					case Entity.__:
						break;
					case Entity.Block:
						Transform block = Instantiate(blocks[Random.Range(0, blocks.Length)], parent).transform;
						block.position = position;
						break;
					case Entity.Player:
						player.transform.position = position;
						break;
					case Entity.Wolf:
						GameObject wolf = Instantiate(this.wolf, parent);
						wolf.transform.position = position;
						break;
					case Entity.Lumberjack:
						GameObject lumberjack = Instantiate(this.lumberjack, parent);
						lumberjack.transform.position = position;
						break;
					case Entity.RedTree:
						Tree redTree = Instantiate(this.redTree, parent);
						redTree.transform.position = position; 
						break;
					case Entity.GreenTree:
						Tree greenTree = Instantiate(this.redTree, parent);
						greenTree.transform.position = position; 
						break;
					case Entity.BlueTree:
						Tree blueTree = Instantiate(this.redTree, parent);
						blueTree.transform.position = position; 
						break;
					case Entity.Key:
						Key key = Instantiate(
							dimension switch
							{
								Dimension.Red => redKey,
								Dimension.Blue => blueKey,
								Dimension.Green => greenKey,
								_ => redKey
							},
							parent
						);
						key.transform.position = position;
						break;
					case Entity.Door:
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
