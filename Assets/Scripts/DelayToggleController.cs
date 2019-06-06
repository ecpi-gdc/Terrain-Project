using UnityEngine;

public class DelayToggleController : MonoBehaviour {

	private static DelayToggleController instance;

	[RuntimeInitializeOnLoadMethod]
	private static void Init() {
		if (instance == null) {
			var toggles = FindObjectsOfType<DelayToggle>();
			if (toggles.Length > 0) {
				instance = new GameObject(nameof(DelayToggleController)).AddComponent<DelayToggleController>();
				instance.toggles = toggles;
			}
		}
	}

	private DelayToggle[] toggles;

	void Update() {
		float t = Time.time;
		foreach (var obj in toggles) {
			if (obj.gameObject.activeSelf && t >= obj.lastUpdate + obj.onDuration) {
				obj.gameObject.SetActive(false);
				obj.lastUpdate = t;
			}
			else if (!obj.gameObject.activeSelf && t >= obj.lastUpdate + obj.offDuration) {
				obj.gameObject.SetActive(true);
				obj.lastUpdate = t;
			}
		}
	}


}
