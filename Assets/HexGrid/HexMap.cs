using UnityEngine;
using System.Collections.Generic;

// A collection of multiple hexes to form a map
public class HexMap : MonoBehaviour {
	
//	public int numberOfRows;
	public int mapWidth;
	public int numberOfLevels;
	public float hexRadius;

	[HideInInspector]
	public Dictionary<string, Hex> tiles;
	[HideInInspector]
	public int[][] axialOffsets;

	Hex GetHexAtAxialLocation(int q, int r)
	{
		// q = x, r = z
		return tiles[q + "," + r];
	}
	void SetHexAtAxialLocation(int q, int r, Hex tile)
	{
		tiles[q + "," + r] = tile;
	}
	Hex Neighbour(Hex startingTile, int direction)
	{
		int qNeighbour = (int)startingTile.axialCoordinates.x + Direction(direction)[0];
		int rNeighbour = (int)startingTile.axialCoordinates.y + Direction(direction)[1];
		Hex neighbour = GetHexAtAxialLocation(qNeighbour, rNeighbour);
		return neighbour;
	}
	Hex Neighbour(int[] startingLocation, int direction)
	{
		int qNeighbour = startingLocation[0] + Direction(direction)[0];
		int rNeighbour = startingLocation[1] + Direction(direction)[1];
		Hex neighbour = GetHexAtAxialLocation(qNeighbour, rNeighbour);
		return neighbour;
	}
	int[] NeighbourPosition(int[] startingLocation, int direction)
	{
		int qNeighbour = startingLocation[0] + Direction(direction)[0];
		int rNeighbour = startingLocation[1] + Direction(direction)[1];
		return new int[]{qNeighbour, rNeighbour};
	}
	int[] NeighbourPosition(Hex startingTile, int direction)
	{
		int qNeighbour = (int)startingTile.axialCoordinates.x + Direction(direction)[0];
		int rNeighbour = (int)startingTile.axialCoordinates.y + Direction(direction)[1];
		return new int[]{qNeighbour, rNeighbour};
	}
	int[] Direction(int direction)
	{
		if (axialOffsets == null)
		{
			axialOffsets = new int[][]
			{
				new int[] {1,0},
				new int[] {1,-1},
				new int[] {0,-1},
				new int[] {-1,0},
				new int[] {-1,1},
				new int[] {0,1}
			};
		}
		if (direction < 0 || direction > 5)
		{
			return new int[] {0,0};
		}
		return axialOffsets[direction];
	}
	int[] Scale(int[] offset, int scale)
	{
		return new int[] { offset[0] * scale, offset[1] * scale };
	}
	// Use this for initialization
	void Start () 
	{
		// Init our tile storage
		tiles = new Dictionary<string, Hex>();
		// Compute map dimensions given a set width at center
		// We compute to build a map that is the specified width at the center, and resembles an overall hexagon shape in total
		// All maps must be odd tiles wide so that we have a nice center to work with
		if (mapWidth % 2 == 0)
			mapWidth += 1;

		CreateHexAtAxialLocation(0,0);
		for (int h = 1; h <= mapWidth; h++)
		{
			// walk out to first tile (southwest)
			int[] offsets = Scale(Direction(4), h);
//			Debug.Log ("Current Offsets = " + offsets[0] + "," + offsets[1]);
			Hex currentTile = CreateHexAtAxialLocation(offsets[0], offsets[1]);
//			Debug.Log ("Current Tile = " + currentTile.axialCoordinates.x + "," + currentTile.axialCoordinates.y);
			// Starting from that tile, walk around in a ring - this is easily
			// achieved by incrementing our direction 0-6 (We start with tile 4 because the first move after that is in direction 0
			for (int d = 0; d < 6; d++)
			{
				for (int n = 0; n < h; n++)
				{
					int[] tileLocation = NeighbourPosition(currentTile, d);
					currentTile = CreateHexAtAxialLocation(tileLocation[0], tileLocation[1]);
				}
			}
		}
	}
	Hex CreateHexAtAxialLocation(int q, int r)	
	{
		GameObject goTile = new GameObject("Tile[" + q + "," + r + "]");
		Hex tile = goTile.AddComponent("Hex") as Hex;
		tile.size = hexRadius;
		float xPos = (q * tile.hDistance) + ((r * tile.hDistance)/2);
		float zPos = r * tile.vDistance * -1;
		goTile.transform.position = new Vector3(xPos, 0, zPos);
		tile.DrawHex();
		tile.axialCoordinates.x = q;
		tile.axialCoordinates.y = r;
		SetHexAtAxialLocation(q, r, tile);
		return tile;
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
}
