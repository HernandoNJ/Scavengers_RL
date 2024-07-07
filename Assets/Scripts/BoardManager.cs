using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;
using Unity.VisualScripting;

// Creates Random generated levels based on the current level number
public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count {
		public int minimum, maximum;
		public Count(int min, int max) { minimum = min; maximum = max; }
	}

	// Dimensions of the gameboard
	public int columns = 8;
	public int rows = 8;

	// Using Count to specify a random range
	// for how many walls we want to spawn in each level
	public Count wallCount = new Count(5, 9); // Min 5 walls and max 9 walls per level
	public Count foodCount = new Count(1, 5); // Min 5 walls and max 9 walls per level

	// Prefabs to Spawn
	public GameObject exit;

	// Prefabs with multiple options will be stored in arrays
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	// Used to place spawned gameobjects
	public Transform boardHolder;

	// Track all different possible positions on the game board
	// keep track of whether a gameobject has been spawned
	// in a specific position or not
	public List<Vector3> gridPositions;

	private void Start()
	{
		InitialBoardSetup();
	}

	private void InitializeList()
	{
		gridPositions.Clear();

		// Nested for loops used to fill the gridPositions list
		// List of possible position for placing walls, enemies or pickups
		// The -1 is for allowing some space between the grid and the outer walls,
		// a walkable space
		for (int x = 1; x < columns - 1; x++)
		{
			for (int y = 1; y < rows - 1; y++)
			{
				gridPositions.Add(new Vector3(x, y, 0));
			}
		}
	}

	// Used to create the wall and the floor
	private void InitialBoardSetup()
	{
		boardHolder = new GameObject("Board").transform;

		// the current grid is from 1,1 to 6,6
		// the empty space is a square from 0,0 to 7,7
		// the wall is a square from -1,-1 to 8,8
		for (int x = -1; x < columns + 1; x++)
		{
			for (int y = -1; y < rows + 1; y++)
			{
				// x=0		--> y=0...y=7
				// x=1		--> y=0...y=7
				// ...
				// x=7		--> y=0...y=7

				// obj = random floor tile

				var index = Random.Range(0, floorTiles.Length);
				var tileObj = floorTiles[index];

				if (x == -1 || x == columns || y == -1 || y == rows)
				{
					// x=-1			--> y=-1...y=8
					// x=0...x=7	--> y=-1, y=8
					// x=8			--> y=-1...y=8 

					// obj = random wall tile

					index= Random.Range(0, outerWallTiles.Length);
					tileObj = outerWallTiles[index];
				}

				var position = new Vector3(x, y, 0);
				var instance = Instantiate(tileObj, position, Quaternion.identity);

				instance.transform.SetParent(boardHolder);
			}
		}
	}
}

