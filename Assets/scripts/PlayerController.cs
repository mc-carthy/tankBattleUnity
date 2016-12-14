using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerSetup))]
[RequireComponent(typeof(PlayerShoot))]
public class PlayerController : MonoBehaviour {

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
		Vector3 inputDirection = GetInput();
		if (inputDirection.sqrMagnitude > 0.25f) {
			pMotor.RotateChassis(inputDirection);
		}
	}

	private void FixedUpdate () {
		Vector3 inputDirection = GetInput();
		pMotor.MovePlayer(inputDirection);
	}

	private Vector3 GetInput () {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		return new Vector3 (h, 0, v);
	}

}
