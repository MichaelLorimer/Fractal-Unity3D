using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalCont : MonoBehaviour 
{
	public Mesh [] meshes;		  // Var to store the mesh reference
	public Material material; // Var to store the material reference
	private Material[,] materials; //2D array for storing meterials

	// Fractal properties 
	public int maxDepth; // Maximum depth to prevent recourseexhaustion
	private int depth;	 // Current depth of the fractal

	public float childScale; // Scale ofthe children
	public float spawnProbability;

	public  float maxRotationSpeed;
	private float rotationSpeed;
	public float maxTwist;

	private void InitializeMaterials ()
	{
		materials = new Material[maxDepth + 1,2];
		for (int i = 0; i <= maxDepth; i++) 
		{
			float t = i / (maxDepth - 1f);
			t += t;

			// Material assignments for colours
			materials [i, 0] = new Material (material);
			materials [i, 0].color = Color.Lerp (Color.white, Color.yellow, t);
			materials [i, 1] = new Material (material);
			materials [i, 1].color = Color.Lerp (Color.white, Color.cyan, t);
		}

		//Adjust colour dependant on depth
		materials [maxDepth, 0].color = Color.magenta;
		materials [maxDepth, 1].color = Color.red;
	}

	private void Start ()
	{
		rotationSpeed = Random.Range (-maxRotationSpeed, maxRotationSpeed);
		transform.Rotate (Random.Range (-maxTwist, maxTwist), 0f, 0f);
		//Check a meterialhas been initializedandif not initializeone 
		if (materials == null) 
		{
			InitializeMaterials ();
		}

		gameObject.AddComponent<MeshFilter> ().mesh = meshes[Random.Range(0,meshes.Length)]; // Adding a mesh component to the fractal
		gameObject.AddComponent<MeshRenderer> ().material = material; //Adding a mesh renderer to the fractal

		GetComponent<MeshRenderer> ().material = materials [depth, Random.Range(0,2)];

		// If to create children
		if (depth < maxDepth)
		{
			StartCoroutine (CreateChildren ());
		}
	}

	private void Update ()
	{
		// Add rotation to fractals
		transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
	}

	// Directions for creating fractles
	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};

	// Angles for creating/ adjusting fractals
	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler ( 0f,  0f,-90f), 
		Quaternion.Euler ( 0f,  0f, 90f),
		Quaternion.Euler (90f,  0f, 0f),
		Quaternion.Euler (-90f, 0f, 0f)
	};
		
	private IEnumerator CreateChildren()
	{
		for (int i = 0; i < childDirections.Length; i++) 
		{
			if(Random.value < spawnProbability)
			{
				yield return new WaitForSeconds(Random.Range(0.1f, 1f));
				new GameObject ("Fractal Child").AddComponent<FractalCont> ().Initialise(this, i);   //Create fractal in 'i' direction from the chilfOrientationcontainer
			}
		}
	}

	// Copy the references from theparent for the child fractal
	private void  Initialise(FractalCont parent, int childIndex)
	{
		meshes = parent.meshes;					 //Copy Mesh
		materials = parent.materials; 		 //Copy Material 
		maxDepth = parent.maxDepth; 		 //Copy Depth
		depth = parent.depth + 1;   		 //Increase the current depth by 1
		childScale = parent.childScale;		 //Setting the child scale to theparents
		spawnProbability = parent.spawnProbability;
		maxRotationSpeed = parent.maxRotationSpeed;
		maxTwist = parent.maxTwist;

		transform.parent = parent.transform; //Set the transform component to the parents
		transform.localScale = Vector3.one* childScale; //Move the child up by'One' as it is a cube
		transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale); //Scale the cubeto behalf sized
		transform.localRotation = childOrientations[childIndex];
	}
}
