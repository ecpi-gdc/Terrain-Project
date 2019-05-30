using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCycle : MonoBehaviour {

#if UNITY_EDITOR
	[SerializeField]
	public UnityEditor.SceneAsset[] scenes;
#endif

	[HideInInspector]
	public string[] scenePaths;

	[Tooltip("Time a scene is active, in seconds.")]
	public float sceneDuration = 300f;

	private int currentIndex;
	private static LevelCycle instance;

	void Awake() {
		if (instance != null)
			Destroy(this);
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		currentIndex = -1;
		StartCoroutine(NextScene());
	}

	void OnValidate() {
#if UNITY_EDITOR
		scenePaths = scenes != null ? scenes.Select(UnityEditor.AssetDatabase.GetAssetPath).ToArray() : null;
#endif
	}
	
	private IEnumerator NextScene() {
		currentIndex = (currentIndex + 1) % scenePaths.Length;
		yield return SceneManager.LoadSceneAsync(scenePaths[ currentIndex ]);
		yield return new WaitForSecondsRealtime(sceneDuration);
		if (enabled)
			StartCoroutine(NextScene());
	}
}
