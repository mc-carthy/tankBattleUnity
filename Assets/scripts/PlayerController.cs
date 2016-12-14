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

	private Vector2 GetInput () {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		return new Vector3 (h, 0, v);
	}

}
