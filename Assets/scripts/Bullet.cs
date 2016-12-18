using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
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
	private ParticleSystem explosionFx;
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

	private void Start () {
		StartCoroutine(SelfDestruct());
	}

	private void OnCollisionExit (Collision other) {
		if (rb.velocity != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation(rb.velocity);
		}
	}

	private IEnumerator SelfDestruct() {
		yield return new WaitForSeconds(lifetime);
		col.enabled = false;
		rb.velocity = Vector3.zero;
		rb.Sleep();

		foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
			mr.enabled = false;
		}

		foreach (ParticleSystem ps in allParticles) {
			ps.Stop();
		}

		if (explosionFx != null) {
			explosionFx.transform.parent = null;
			explosionFx.Play();
		}

		Destroy(gameObject);
	}
}
