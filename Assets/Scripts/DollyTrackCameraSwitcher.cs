using System;
using Cinemachine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[DisallowMultipleComponent]
public class DollyTrackCameraSwitcher : MonoBehaviour {

	[Serializable]
	public class SwitchDefinition {
		public CinemachineVirtualCameraBase targetCam;
		[Range(0f, 1f)]
		public float switchPercent;

		[NonSerialized]
		public float switchDistance;
		[NonSerialized]
		public bool hit = false;
	}

	[SerializeField]
	private CinemachineDollyCart dollyCart = null;
	[SerializeField]
	private CinemachineVirtualCameraBase initialActive = null;
	[SerializeField]
	private SwitchDefinition[] definitions;

	private float trackLength;
	private CinemachineVirtualCameraBase activeCam;

	void Awake() {
		// disable all cameras at start
		foreach (SwitchDefinition def in definitions)
			def.targetCam.enabled = false;

		if (initialActive != null)
			SetActive(initialActive);
	}

	private void SetActive(CinemachineVirtualCameraBase vcam) {
		if (activeCam != null)
			activeCam.enabled = false;

		vcam.enabled = true;
		activeCam = vcam;
	}

	void Start() {
		trackLength = dollyCart.m_Path.PathLength;

		foreach (SwitchDefinition def in definitions)
			def.switchDistance = def.switchPercent * trackLength;
		ResetHit();
	}

	void Update() {
		foreach (SwitchDefinition def in definitions) {
			if (HitTrigger(def)) {
				SetActive(def.targetCam);
				def.hit = true;
				return;
			}
		}
	}

	private void ResetHit() {
		foreach (SwitchDefinition def in definitions)
			def.hit = false;
	}

	private bool HitTrigger(SwitchDefinition def) {
		if (def.hit) return false;
		bool forward = dollyCart.m_Speed > 0;
		float d = dollyCart.m_Position;
		return forward ? d > def.switchDistance : d < def.switchDistance;
	}
/*
#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;
		var path = dollyCart.m_Path;
		for (int i = 0; i < definitions.Length; i++) {
			Vector3 pos = path.EvaluatePosition(definitions[ i ].switchPercent * path.MaxPos);
			Gizmos.DrawSphere(pos, 0.5f);
		}
	}

	[CustomPropertyDrawer(typeof(SwitchDefinition))]
	public class DefinitionDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var vcamProp = property.FindPropertyRelative("targetCam");
			var percentProp = property.FindPropertyRelative("switchPercent");
			var path = (property.serializedObject.targetObject as DollyTrackCameraSwitcher)?.dollyCart.m_Path;

			Rect r = new Rect(position) { height = EditorGUIUtility.singleLineHeight };

			EditorGUI.LabelField(r, label);
			r.y += r.height + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.indentLevel++;
			EditorGUI.PropertyField(r, vcamProp);
			r.y += r.height + EditorGUIUtility.standardVerticalSpacing;

			using (var check = new EditorGUI.ChangeCheckScope()) {
				EditorGUI.PropertyField(r, percentProp);
				if (check.changed && path != null) {
					SceneView.lastActiveSceneView.pivot = path.EvaluatePosition(percentProp.floatValue * path.MaxPos);
					SceneView.lastActiveSceneView.size = 5f;
					SceneView.lastActiveSceneView.Repaint();
				}
			}

			EditorGUI.indentLevel--;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return 3 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
		}
	}

#endif
*/

}