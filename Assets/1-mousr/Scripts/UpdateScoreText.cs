using UnityEngine;
using System.Collections;

public class UpdateScoreText : MonoBehaviour {
	public string ScoreLabelText = "Score";
	private ScoreManager PlayerScoreManager;

	// Use this for initialization
	void Start () {
		PlayerScoreManager = GameObject.Find ("Player").GetComponent<ScoreManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.guiText.text = string.Format ("{0}: {1}", ScoreLabelText, PlayerScoreManager.PlayerScore.ToString("D3"));
	}
}
