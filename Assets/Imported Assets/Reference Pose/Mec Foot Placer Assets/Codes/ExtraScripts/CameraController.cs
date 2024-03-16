using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject mTarget;
	public Vector3 mDistance;
	public float mSpeed = 5;
	
	// Update is called once per frame
	void Update () 
	{
		GetComponent<Camera>().transform.LookAt(mTarget.transform.position);
		GetComponent<Camera>().transform.position = GetComponent<Camera>().transform.position + mSpeed * Time.deltaTime * (mTarget.transform.position + mDistance - GetComponent<Camera>().transform.position);
	}
}
