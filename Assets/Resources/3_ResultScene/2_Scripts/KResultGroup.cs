using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KResultGroup : MonoBehaviour {

    public UILabel CurrentScoreLabel;
    public UILabel BestScoreLabel;
    public UISprite BGSprite;

    // Use this for initialization
    void Start () {
        CurrentScoreLabel.text += HMng.I.nCurrentScore.ToString();
        BestScoreLabel.text += HMng.I.nHighScore.ToString();
        BGSprite.color = HMng.I.TileColors[HMng.I.nColorType, 1];
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPlayAgainBtn ()
    {
        SceneManager.LoadScene(2);
    }

    public void OnBackToMenuBtn ()
    {
        SceneManager.LoadScene(1);
    }
}
