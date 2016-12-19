using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerSetup))]
[RequireComponent(typeof(PlayerShoot))]
public class PlayerController : NetworkBehaviour {

	private PlayerHealth pHealth;
	private PlayerMotor pMotor;
	private PlayerSetup pSetup;
	private PlayerShoot pShoot;

	private float respawnTime = 3f;

	private void Awake () {
		pHealth = GetComponent<PlayerHealth>();
		pMotor = GetComponent<PlayerMotor>();
		pSetup = GetComponent<PlayerSetup>();
		pShoot = GetComponent<PlayerShoot>();
	}

	private void Update () {
		if (!isLocalPlayer || pHealth.IsDead) {
			return;
		}

		if (Input.GetMouseButtonDown(0)) {
			pShoot.Shoot();
		}

		Vector3 inputDirection = GetInput();
		if (inputDirection.sqrMagnitude > 0.25f) {
			pMotor.RotateChassis(inputDirection);
		}
		Vector3 turretDir = Utility.GetWorldPointFromScreenPoint(Input.mousePosition, pMotor.transform.position.y) - pMotor.transform.position;
		pMotor.RotateTurret(turretDir);
	}

	private void FixedUpdate () {
		if (!isLocalPlayer || pHealth.IsDead) {
			return;
		}

		Vector3 inputDirection = GetInput();
		pMotor.MovePlayer(inputDirection);
	}

	private Vector3 GetInput () {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		return new Vector3 (h, 0, v);
	}

	private void Disable () {
		StartCoroutine(RespawnRoutine());
	}

	private IEnumerator RespawnRoutine () {
		transform.position = Vector3.zero;
		pMotor.Rb.velocity = Vector3.zero;
		yield return new WaitForSeconds (respawnTime);
		pHealth.Reset();
	}
}
