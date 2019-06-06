using UnityEngine;

public class DelayToggle : MonoBehaviour {
	[Tooltip("How long the object stays on for.")]
	public float onDuration = 1f;
	[Tooltip("How long the object stays off for.")]
	public float offDuration = 1f;

	public float LastUpdate { get; private set; }
	public bool Active { get; private set; }

	void Start() {
		SetActive(true);
	}

	public void SetActive(bool active) {
		var ps = GetComponent<ParticleSystem>();
		if (active) {
			ps.Play(true);
		}
		else {
			ps.Stop(true);
		}

		LastUpdate = Time.time;
		Active = active;
	}
}
