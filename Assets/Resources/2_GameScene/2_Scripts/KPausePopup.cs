using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KPausePopup : MonoBehaviour {

    public GameObject PausePopup;
    public UISprite BGSprite;
    public UISprite MusicBtnSprite;
    public UISprite SFXBtnSprite;
    public bool bSFXOn = true; // true: 효과음 켜짐, false: 효과음 꺼짐
    public bool bMusicOn = true; // true: 배경음 켜짐, false: 배경음 꺼짐

    // Use this for initialization
    void Start () {
        BGSprite.color = HMng.I.TileColors[HMng.I.nColorType, 1];
    }
	
	// Update is called once per frame
	void Update () {
        ChangeSFXSprite();
        ChangeMusicSprite();
    }

    public void OnPauseBtn ()
    {
        PausePopup.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnResumeBtn ()
    {
        PausePopup.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnGoMenuBtn ()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void OnSFXBtn()
    {
        if (!bSFXOn)
            bSFXOn = true;
        else
            bSFXOn = false;
    }

    public void ChangeSFXSprite()
    {
        if (bSFXOn)
            SFXBtnSprite.spriteName = "checkedbtn";
        else
            SFXBtnSprite.spriteName = "nopebtn";
    }

    public void OnMusicBtn()
    {
        if (!bMusicOn)
            bMusicOn = true;
        else
            bMusicOn = false;
    }

    public void ChangeMusicSprite()
    {
        if (bMusicOn)
            MusicBtnSprite.spriteName = "checkedbtn";
        else
            MusicBtnSprite.spriteName = "nopebtn";
    }
}
