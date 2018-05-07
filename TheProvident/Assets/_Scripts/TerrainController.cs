using UnityEngine;
using System.Collections;

public class TerrainController : MonoBehaviour {

	public float coff, range;
	public float treeSize, flowerSize;
	public GameObject[] placements;
	public GameObject flowers;
	public GameObject bats;
	public GameObject rabbits;

	void Start () {
		
//		Mesh mesh = GetComponent<MeshFilter> ().mesh;
//		Vector3[] vertices = mesh.vertices;
//		Vector3[] normals = mesh.normals;
//
//		for (int i = 0; i < vertices.Length; i++) {
//			if (Random.Range (0.0f, 1.0f) > .5f) {
//				vertices [i] += (coff * Random.Range(0, range) * normals[i]);
//			}
//		}
//
//		mesh.vertices = vertices;
//		mesh.RecalculateBounds ();
//		mesh.RecalculateNormals ();
//
//		GetComponent<MeshCollider> ().sharedMesh = null;
//		GetComponent<MeshCollider> ().sharedMesh = mesh;

		for (int x = -5*(int)transform.localScale.x + 100; x < 5*(int)transform.localScale.x - 100; x+=10) {
			for (int z = -5*(int)transform.localScale.z + 100; z < 5*(int)transform.localScale.z - 100; z+=10) {
				if (Random.Range (0, 100) < 1) {
					RaycastHit hit;
					if (Physics.Raycast ( new Vector3((float)x, 500f, (float)z), Vector3.down, out hit, Mathf.Infinity)) {
						if (hit.transform.gameObject.CompareTag ("ground")) {
							int index = Random.Range (0, 7);
							if (index < 4) {
								GameObject tree = Instantiate (placements [index]);
								tree.tag = "tree";

								tree.transform.localScale = Random.Range (.5f, 1.5f) * treeSize * Vector3.one;
								tree.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
								tree.transform.position = hit.point;
								tree.transform.RotateAround (tree.transform.position, transform.up, Random.Range (0, 360));

								Color ogColor = tree.GetComponent<Renderer> ().material.color;
								Color newColor = Color.Lerp (ogColor, Color.blue, Random.Range (0.0f, .5f));
								tree.GetComponent<Renderer> ().material.color = newColor;

								tree.transform.parent = transform;
							} else if (index == 4) {
								GameObject flower = Instantiate (placements [index]);
								flower.tag = "flower";

								flower.transform.localScale = flowerSize * Vector3.one;
								flower.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
								flower.transform.position = hit.point + flower.transform.up * 1.05f;
								flower.transform.RotateAround (flower.transform.position, transform.up, Random.Range (0, 360));

								flower.AddComponent<SphereCollider> ().radius = .05f;
								flower.transform.parent = flowers.transform;
							} else if (index == 5) {
								GameObject bat = Instantiate (placements [index]);
								//bat.tag = "Predator";

								bat.transform.localScale = Vector3.one * 500f;
								bat.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
								bat.transform.position = hit.point+Vector3.up*2f;

								bat.transform.parent = bats.transform;
							} else {
								GameObject rabbit = Instantiate (placements [index]);
								//rabbit.tag = "Prey";

								rabbit.transform.localScale = Vector3.one * 500f;
								rabbit.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
								rabbit.transform.position = hit.point+Vector3.up*1.05f;

								rabbit.transform.parent = rabbits.transform;
							}
						}
					}
				}
			}
		}
	}
}