using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

	[SyncVarAttribute]
	private bool isDead;
	public bool IsDead {
		get {
			return isDead;
		}
	}

	[SyncVarAttribute(hook="UpdateHealthBar")]
	private float currentHealth;

	[SerializeField]
	private GameObject deathPrefab;
	[SerializeField]
	private RectTransform healthBar;

	private PlayerManager lastAttacker;
	private float maxHealth = 3f;
	private float initialRectWidth;

	private void Start () {
		initialRectWidth = healthBar.rect.width;
		Reset();
		UpdateHealthBar(currentHealth);
	}

	public void Damage (float damage, PlayerManager pc = null) {

		if (!isServer) {
			return;
		}

		if (pc != null && pc != this.GetComponent<PlayerManager>()) {
			lastAttacker = pc;
		}

		currentHealth -= damage;
		if (currentHealth <= 0 && !isDead) {
			if (lastAttacker != null) {
				lastAttacker.Score++;
				lastAttacker = null;
			}
			GameManager.Instance.UpdateScoreboard();
			isDead = true;
			RpcDie();
		}
	}

	public void Reset () {
		currentHealth = maxHealth;
		SetActiveState(true);
		isDead = false;
	}

	[ClientRpc]
	private void RpcDie () {
		if (deathPrefab != null) {
			GameObject deathFx = Instantiate(deathPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
			Destroy(deathFx.gameObject, 3f);
		}

		SetActiveState(false);

		gameObject.SendMessage("Disable");
	}

	private void SetActiveState (bool state) {
		foreach (Collider c in GetComponentsInChildren<Collider>()) {
			c.enabled = state;
		}

		foreach (Canvas c in GetComponentsInChildren<Canvas>()) {
			c.enabled = state;
		}

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = state;
		}
	}

	private void UpdateHealthBar (float value) {
		if (healthBar != null) {
			healthBar.sizeDelta = new Vector2(value / maxHealth * initialRectWidth, healthBar.sizeDelta.y);
		}
	}
}
