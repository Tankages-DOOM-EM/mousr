using UnityEngine;
using System.Collections;

public class HelpTip : MonoBehaviour {
	
	private GUIText _GUIText;
	private GUIText GUIText {
		get {
			if (_GUIText == null) {
				_GUIText = gameObject.GetComponent<GUIText> ();
			}
			return _GUIText;
		}
	}
	private Transform _background;
	private Transform Background {
		get {
			if(_background == null) {
				_background = transform.FindChild ("Background");
			}
			return _background;
		}

	}
	
	public string Text {
		get {
			return GUIText.text;
		}
		set {
			GUIText.text = value;
		}
	}

	public float Size {
		get {
			return Background.localScale.x; 
		}
		set {
			var scale = Background.localScale;
			scale.x = value;
			scale.y = 0.45f;
			Background.localScale = scale;
		}
	}
	private GameObject _target;
	public GameObject Target {
		get { return _target; }
		set {
			_target = value;
			transform.parent = _target.transform;
			Background.parent = _target.transform;
			Background.localPosition = new Vector3(0, 1.0f, -8);
		}
	}
	public Vector3 Offset;
	
	private Camera Camera;

	void Start() {
		Camera = GameObject.Find ("Camera").GetComponent<Camera>();
	}

	void Update() {
		transform.position = Camera.WorldToViewportPoint (Target.transform.position + Offset);
	}

}
