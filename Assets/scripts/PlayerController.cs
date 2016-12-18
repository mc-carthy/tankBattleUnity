using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerSetup))]
[RequireComponent(typeof(PlayerShoot))]
public class PlayerController : NetworkBehaviour {

	private PlayerHealth pHealth;
	private PlayerMotor pMotor;
	private PlayerSetup pSetup;
	private PlayerShoot pShoot;

	private void Awake () {
		pHealth = GetComponent<PlayerHealth>();
		pMotor = GetComponent<PlayerMotor>();
		pSetup = GetComponent<PlayerSetup>();
		pShoot = GetComponent<PlayerShoot>();
	}

	private void Update () {
		if (!isLocalPlayer) {
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
		if (!isLocalPlayer) {
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

}
