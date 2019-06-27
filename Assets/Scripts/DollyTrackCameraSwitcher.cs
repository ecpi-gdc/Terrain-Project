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
		public bool hit = false;
	}

	[SerializeField]
	private CinemachineDollyCart dollyCart = null;
	[SerializeField]
	private CinemachineVirtualCameraBase initialActive = null;
	[SerializeField]
	private SwitchDefinition[] definitions;
	
	#if UNITY_EDITOR
	[SerializeField]
	private bool followMarkerGizmo = true;
#endif

	private CinemachineVirtualCameraBase activeCam;

	void Awake() {
		// disable all cameras at start
		foreach (SwitchDefinition def in definitions)
			def.targetCam.enabled = false;

		if (initialActive != null)
			SetActive(initialActive);
	}

	void Start() {
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

	private void SetActive(CinemachineVirtualCameraBase vcam) {
		if (activeCam != null)
			activeCam.enabled = false;

		vcam.enabled = true;
		activeCam = vcam;
	}

	private void ResetHit() {
		foreach (SwitchDefinition def in definitions)
			def.hit = false;
	}

	private bool HitTrigger(SwitchDefinition def) {
		if (def.hit) return false;
		bool forward = dollyCart.m_Speed > 0;
		float pos = dollyCart.m_Position;
		float dist = def.switchPercent * dollyCart.m_Path.PathLength;
		return forward ? pos > dist : pos < dist;
	}

	private Vector3 GetPosition(float percent) {
		var path = dollyCart.m_Path;
		return path.EvaluatePosition(path.ToNativePathUnits(percent * path.PathLength,
			CinemachinePathBase.PositionUnits.Distance));
	}

//*
#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;

		foreach (SwitchDefinition def in definitions) {
			Vector3 pos = GetPosition(def.switchPercent);
			Gizmos.DrawSphere(pos, 0.5f);
		}
	}

	[CustomPropertyDrawer(typeof(SwitchDefinition))]
	public class DefinitionDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var vcamProp = property.FindPropertyRelative("targetCam");
			var percentProp = property.FindPropertyRelative("switchPercent");
			var comp = property.serializedObject.targetObject as DollyTrackCameraSwitcher;

			Rect r = new Rect(position) { height = EditorGUIUtility.singleLineHeight };

			EditorGUI.LabelField(r, label);
			r.y += r.height + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.indentLevel++;
			EditorGUI.PropertyField(r, vcamProp);
			r.y += r.height + EditorGUIUtility.standardVerticalSpacing;

			using (var check = new EditorGUI.ChangeCheckScope()) {
				EditorGUI.PropertyField(r, percentProp);
				if (comp != null && comp.followMarkerGizmo && check.changed) {
					SceneView.lastActiveSceneView.pivot = comp.GetPosition(percentProp.floatValue);
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
//*/

}