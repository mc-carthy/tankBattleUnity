using UnityEngine;

public class FaceCamera : MonoBehaviour {

	private Camera cam;

	private void Start () {
		if (cam == null) {
			cam = Camera.main;
		}
	}

	private void Update () {
		transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
	}
}
