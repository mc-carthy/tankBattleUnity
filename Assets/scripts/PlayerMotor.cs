using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Transform chassis;
	[SerializeField]
	private Transform turret;

	private Rigidbody rb;
	private float moveSpeed = 100f;
	private float chassisRotateSpeed = 1f;
	private float turretRotateSpeed = 3f;

	private void Awake () {
		rb = GetComponent<Rigidbody>();
	}

	public void RotateChassis (Vector3 dir) {
		FaceDirection(chassis, dir, chassisRotateSpeed);
	}

	public void RotateTurret (Vector3 dir) {
		FaceDirection(turret, dir, turretRotateSpeed);
	}

	public void MovePlayer (Vector3 dir) {
		Vector3 moveDirection = dir * moveSpeed * Time.deltaTime;
		rb.velocity = moveDirection;
	}

	private void FaceDirection (Transform xform, Vector3 dir, float rotSpeed) {
		if (dir != Vector3.zero && xform != null) {
			Quaternion desiredRot = Quaternion.LookRotation(dir);

			xform.rotation = Quaternion.Slerp(xform.rotation, desiredRot, rotSpeed * Time.deltaTime);
		}
	}
}