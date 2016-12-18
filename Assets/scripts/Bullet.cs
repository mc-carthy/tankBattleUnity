using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : NetworkBehaviour {

	[SerializeField]
	private float speed = 20f;
	public float Speed {
		get {
			return speed;
		}
	}

	[SerializeField]
	private float lifetime = 5f;

	private Rigidbody rb;
	private Collider col;
	private List<ParticleSystem> allParticles;

	private void Awake () {
		rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		allParticles = GetComponentsInChildren<ParticleSystem>().ToList();
	}

	private void OnCollisionExit (Collision other) {
		if (rb.velocity != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation(rb.velocity);
		}
	}
}
