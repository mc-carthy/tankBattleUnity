using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	[SerializeField]
	private Rigidbody bulletPrefab;
	[SerializeField]
	private Transform bulletSpawn;

	private int shotsPerBurst = 2;
	private int shotsLeft;
	private bool isReloading;
	private float reloadTime = 1f;

	private void Start () {
		shotsLeft = shotsPerBurst;
		isReloading = false;
	}

	private void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Shoot();
		}
	}

	public void Shoot () {
		if (isReloading || bulletPrefab == null) {
			return;
		}

		Bullet bullet = null;
		bullet = bulletPrefab.GetComponent<Bullet>();

		Rigidbody rb = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;

		if (rb != null) {
			rb.velocity = bullet.Speed * bulletSpawn.transform.forward;
		}
	}

}
