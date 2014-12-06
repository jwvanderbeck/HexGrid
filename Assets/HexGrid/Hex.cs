using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {

	// This class defines an individual hex on the grid
	public float size;
	public float level;
	public Texture texture;

	[HideInInspector]
	public Vector2 axialCoordinates;
	[HideInInspector]
	public Vector2 worldCoordinates;
	[HideInInspector]
	public float height
	{
		get { return size * 2f; }
	}
	[HideInInspector]
	public float width
	{
		get { return Mathf.Sqrt(3f)/2f * height; }
	}
	[HideInInspector]
	public float hDistance
	{
		get { return width; }
	}
	[HideInInspector]
	public float vDistance
	{
		get { return 3f/4f * height; }
	}

	Vector3 hexCenter;
	Vector3[] vertices;
	Vector2[] uvs;
	int[] triangles;

	public void DrawHex()
	{
		// Generate our vertices given a center and size
		//      1
		//     __ 
		//  2 /  \ 0
		//   |    |
		//  3 \  / 5
		//     --
		//      4
		hexCenter = this.transform.localPosition;
		vertices = new Vector3[6];
		for (int i = 0; i < 6; i++) 
		{
			float angle = 2.0f * Mathf.PI / 6.0f * (i + 0.5f);
			float xPos = size * Mathf.Cos(angle);
			float yPos = size * Mathf.Sin(angle);
			vertices[i] = new Vector3(xPos, level, yPos);
//			Debug.Log (xPos + "," + yPos);
		}
		
		// Generate triangles connecting the vertices into a hexagon
		// Vertices go clockwise
		triangles = new int[]
		{
			1,0,5,
			1,5,2,
			2,5,4,
			2,4,3
		};
		
		// Apply UV coordinates
		uvs = new Vector2[]
		{
			new Vector2(1.0f, 0.75f),
			new Vector2(0.5f, 1.0f),
			new Vector2(0.25f, 0.75f),
			new Vector2(0.0f, 0.5f),
			new Vector2(0.25f, 0.25f),
			new Vector2(0.0f, 0.5f),
		};
		
		// Create the Mesh pieces on our GameObject
		// We need a Mesh Filter and a Mesh Renderer
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		meshFilter.mesh = new Mesh();
		meshFilter.mesh.vertices = vertices;
		meshFilter.mesh.triangles = triangles;
		meshFilter.mesh.uv = uvs;
		meshFilter.mesh.RecalculateNormals();
		// Apply texture for test rendering
		renderer.material.mainTexture = texture;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
