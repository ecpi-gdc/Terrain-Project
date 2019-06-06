using UnityEngine;

public class DelayToggle : MonoBehaviour {
	[Tooltip("How long the object stays on for.")]
	public float onDuration = 1f;
	[Tooltip("How long the object stays off for.")]
	public float offDuration = 1f;

	internal float lastUpdate;

	void Start() {
		lastUpdate = Time.time;
	}
}
