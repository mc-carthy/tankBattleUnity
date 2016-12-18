using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

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

	private List<ParticleSystem> allParticles;

	private void Start () {
		allParticles = GetComponentsInChildren<ParticleSystem>().ToList();
	}
}
