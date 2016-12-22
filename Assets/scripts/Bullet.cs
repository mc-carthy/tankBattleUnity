﻿using UnityEngine;
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

	private PlayerController owner;
	public PlayerController Owner {
		set {
			owner = value;
		}
	}

	[SerializeField]
	private ParticleSystem explosionFx;
	[SerializeField]
	private float lifetime = 5f;
	[SerializeField]
	private int bounces = 2;
	[SerializeField]
	private List<string> bounceTags;
	[SerializeField]
	private List<string> damageTags;

	private Rigidbody rb;
	private Collider col;
	private List<ParticleSystem> allParticles;
	private float damage = 1f;
	private float fuseTime = 0.025f;

	private void Awake () {
		rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		allParticles = GetComponentsInChildren<ParticleSystem>().ToList();
	}

	private void Start () {
		StartCoroutine(SelfDestruct());
	}

	private void OnCollisionEnter (Collision other) {
		CheckCollisions(other);

		if (bounceTags.Contains(other.gameObject.tag)) {
			if (bounces <= 0) {
				Explode();
			}
			bounces--;
		}
	}

	private void OnCollisionExit (Collision other) {
		if (rb.velocity != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation(rb.velocity);
		}
	}

	private IEnumerator SelfDestruct() {
		
		col.enabled = false;

		yield return new WaitForSeconds (fuseTime);

		col.enabled = true;

		yield return new WaitForSeconds(lifetime);
		Explode();
	}

	private void Explode () {
		col.enabled = false;
		rb.velocity = Vector3.zero;
		rb.Sleep();

		foreach (ParticleSystem ps in allParticles) {
			ps.Stop();
		}

		if (explosionFx != null) {
			explosionFx.transform.parent = null;
			explosionFx.Play();
		}

		if (isServer) {
			foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
				mr.enabled = false;
			}
			Destroy(gameObject);
		}
	}

	private void CheckCollisions (Collision other) {
		if (damageTags.Contains(other.gameObject.tag)) {
			Explode();
			PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

			if (playerHealth != null) {
				playerHealth.Damage(damage, owner);
			}
		}
	}
}
