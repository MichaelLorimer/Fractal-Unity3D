using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour 
{
	public float Speed;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		float xpos = Input.GetAxis ("Horizontal") * Speed;
		float zpos = Input.GetAxis ("Vertical") * Speed;
		float ypos = 0.0f;

		transform.position = new Vector3 (transform.position.x + xpos, transform.position.y + ypos, transform.position.z + zpos);

		if(Input.GetKey(KeyCode.Q))
		{
			float pos = 0.5f;
			transform.position = new Vector3 (transform.position.x, transform.position.y + pos * Speed, transform.position.z);
		}

		if(Input.GetKey(KeyCode.E))
		{
			float pos = 0.5f;
			transform.position = new Vector3 (transform.position.x, transform.position.y - pos * Speed, transform.position.z);
		}

		if(Input.GetKey(KeyCode.R))
		{
			transform.Rotate (0,-1, 0, Space.World);
		}

		if(Input.GetKey(KeyCode.T))
		{
			transform.Rotate (0,1, 0, Space.World);
		}
	}
}
