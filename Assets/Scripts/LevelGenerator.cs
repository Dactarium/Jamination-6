using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	[field: SerializeField]
	public List<Level> Levels { get; private set; } = new List<Level>();
}

[Serializable]
public struct Level
{
	public TextAsset Red;
	public TextAsset Green;
	public TextAsset Blue;
}
