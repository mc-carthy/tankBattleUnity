using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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

		shotsLeft--;

		if (shotsLeft <= 0) {
			StartCoroutine(Reload());
		}
	}

	private IEnumerator Reload () {
		shotsLeft = shotsPerBurst;
		isReloading = true;
		yield return new WaitForSeconds(reloadTime);
		isReloading = false;
	}

}
