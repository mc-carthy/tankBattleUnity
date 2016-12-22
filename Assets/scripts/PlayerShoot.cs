using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerShoot : NetworkBehaviour {

	private bool isAbleToShoot;
	public bool IsAbleToShoot {
		get {
			return isAbleToShoot;
		}
		set {
			isAbleToShoot = value;
		}
	}

	[SerializeField]
	private Rigidbody bulletPrefab;
	[SerializeField]
	private Transform bulletSpawn;
	[SerializeField]
	private ParticleSystem misfireEffect;
	[SerializeField]
	private LayerMask obstacleMask;

	private int shotsPerBurst = 2;
	private int shotsLeft;
	private bool isReloading;
	private float reloadTime = 1f;

	private void Start () {
		shotsLeft = shotsPerBurst;
		isReloading = false;
	}

	public void Shoot () {
		if (isReloading || bulletPrefab == null || !isAbleToShoot) {
			return;
		}

		RaycastHit hit;
		
		Vector3 centre = new Vector3(transform.position.x, bulletSpawn.position.y, transform.position.z);

		Vector3 dir = (bulletSpawn.position - centre).normalized;

		if (Physics.SphereCast(centre, 0.25f, dir, out hit, 2.5f, obstacleMask, QueryTriggerInteraction.Ignore)) {
			if (misfireEffect != null) {
				ParticleSystem effect = Instantiate(misfireEffect, hit.point, Quaternion.identity) as ParticleSystem;
				effect.Stop();
				effect.Play();
				Destroy(effect.gameObject, 3f);
			}
		} else {
			CmdShoot();

			shotsLeft--;

			if (shotsLeft <= 0) {
				StartCoroutine(Reload());
			}
		}
	}

	[CommandAttribute]
	public void CmdShoot () {
		Bullet bullet = null;

		Rigidbody rb = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;
		bullet = rb.GetComponent<Bullet>();

		if (rb != null) {
			rb.velocity = bullet.Speed * bulletSpawn.transform.forward;
			bullet.Owner = GetComponent<PlayerManager>();
			NetworkServer.Spawn(rb.gameObject);
		}
	}

	private IEnumerator Reload () {
		shotsLeft = shotsPerBurst;
		isReloading = true;
		yield return new WaitForSeconds(reloadTime);
		isReloading = false;
	}

}
