using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HelpTip2 : MonoBehaviour {

	private GUIText _GUIText;
	private GUIText GUIText {
		get {
			if (_GUIText == null) {
				_GUIText = HelpTip.GetComponent<GUIText> ();
			}
			return _GUIText;
		}
	}

	private Transform _background;
	private Transform Background {
		get {
			if(_background == null) {
				_background = HelpTip.transform.FindChild ("Background");
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
			Background.localScale = scale;
		}
	}

	public float TipDistance;
	public Vector3 Offset;
	public string ObjectTagToTip;
	public Camera Camera;
	public GameObject HelpTipPrefab;
	private GameObject HelpTip;
	private GameObject Target;
	private IList<GameObject> Targets = new List<GameObject>();
	
	void Start() {
		HelpTip = Instantiate (HelpTipPrefab) as GameObject;
	}
	
	void Update() {

		if (Target) {
			HelpTip.SetActive (true);
			HelpTip.transform.position = Camera.WorldToViewportPoint (Target.transform.position + Offset);
			Background.position = Target.transform.position + Offset;
		}
		else {
			HelpTip.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != ObjectTagToTip) {
			return;
		}
		Debug.Log ("Trigger ENTER: " + other.tag);
		if (Target) {
			Debug.Log ("Has Target!");
			var tDist = (Target.transform.position - gameObject.transform.position).sqrMagnitude;
			var oDist = (other.transform.position - gameObject.transform.position).sqrMagnitude;

			if(tDist < oDist) {
				Targets.Add(other.gameObject);
			}
			else {
				Targets.Add(Target);
				Target = other.gameObject;
			}
		}
		else {
			Target = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag != ObjectTagToTip) {
			return;
		}
		
		Debug.Log ("Trigger EXIT: " + other.tag);
		if (Targets.Contains (other.gameObject)) {
			Targets.Remove (other.gameObject);
			return;
		}

		if (Target == other.gameObject) {
			Debug.Log ("They're the same!");
			Target = Targets.OrderBy( o => (o.transform.position - gameObject.transform.position).sqrMagnitude).FirstOrDefault();
			if(Target) {
				Targets.Remove (Target);
			}
		}
		Debug.Log ("All Done");
	}
}
