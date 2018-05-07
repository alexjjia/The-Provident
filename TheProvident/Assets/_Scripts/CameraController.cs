using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	public float height, border, speed;
	public GameObject ground;

	private float bottom = 0;
	private float x, z = 0;

	// Use this for initialization
	void Start () {
		RaycastHit hit;
		if (Physics.Raycast (new Vector3(0, 500f, 0), Vector3.down, out hit, Mathf.Infinity)) {
			bottom = hit.point.y;
			transform.position = new Vector3(0.0f, bottom + 2*height, 0.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		x = transform.position.x;
		z = transform.position.z;

		if (Mathf.Abs(x)<((5*ground.transform.localScale.x) - border) && 
			Mathf.Abs(z)<((5*ground.transform.localScale.z) - border)) {
			if (Input.GetKey (KeyCode.S)) {
				transform.position += -1 * transform.forward* Time.deltaTime * speed;
			}
			if (Input.GetKey (KeyCode.W)) {
				transform.position += transform.forward * Time.deltaTime * speed;
			}
		} else {
			transform.position = Vector3.Lerp (transform.position, Vector3.zero, .0001f);
		}

		if (Input.GetKey (KeyCode.D)) {
			transform.RotateAround (transform.position, Vector3.up, 1.2f*speed*Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.RotateAround (transform.position, Vector3.up, -1.2f*speed*Time.deltaTime);
		}

		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, Mathf.Infinity)) {
			bottom = hit.point.y;
			if (hit.transform.gameObject.CompareTag("ground")){
				transform.position =  new Vector3(transform.position.x, bottom + height, transform.position.z);
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			RaycastHit target;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if (Physics.Raycast (ray, out target, Mathf.Infinity)) {
				Debug.Log (target.transform.gameObject.name);
				if (target.transform.tag.StartsWith("Pr")){
					//target.transform.gameObject.GetComponentInChildren<ParticleSystem> ().Emit (20);
					Destroy (target.transform.gameObject, 1f);
				}
			}
		}
	}
}
