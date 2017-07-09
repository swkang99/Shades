using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KSettingPopup : MonoBehaviour {

    public UISprite SFXBtnSprite;
    public bool bSFXOn = true; // true: 효과음 켜짐, false: 효과음 꺼짐

    public UISprite MusicBtnSprite;

    public UISprite RedBtnSprite;
    public bool bRedOn = true;

    public UISprite GreenBtnSprite;
    public bool bGreenOn = false;

    public UISprite BlueBtnSprite;
    public bool bBlueOn = false;

    public UISprite YellowBtnSprite;
    public bool bYellowOn = false;

    public UISprite PurpleBtnSprite;
    public bool bPurpleOn = false;

    public GameObject SettingPopup;

    // Use this for initialization
    void Start () {
        InitColorState();
	}

    public void InitColorState()
    {
        switch (HMng.I.nColorType)
        {
            // 빨간색
            case 0:
                bRedOn = true;
                bGreenOn = false;
                bBlueOn = false;
                bYellowOn = false;
                bPurpleOn = false;
                break;

            // 초록색
            case 1:
                bRedOn = false;
                bGreenOn = true;
                bBlueOn = false;
                bYellowOn = false;
                bPurpleOn = false;
                break;

            // 파란색
            case 2:
                bRedOn = false;
                bGreenOn = false;
                bBlueOn = true;
                bYellowOn = false;
                bPurpleOn = false;
                break;

            // 노란색
            case 3:
                bRedOn = false;
                bGreenOn = false;
                bBlueOn = false;
                bYellowOn = true;
                bPurpleOn = false;
                break;

            // 보라색
            case 4:
                bRedOn = false;
                bGreenOn = false;
                bBlueOn = false;
                bYellowOn = false;
                bPurpleOn = true;
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        ChangeSFXSprite();
        ChangeMusicSprite();
        ChangeColorSprite();

        if (Input.GetKeyUp(KeyCode.Escape))
            SettingPopup.SetActive(false);
    }

    public void OnSFXBtn ()
    {
        if (!bSFXOn)
            bSFXOn = true;
        else
            bSFXOn = false;
    }

    public void ChangeSFXSprite ()
    {
        if (bSFXOn)
            SFXBtnSprite.spriteName = "checkedbtn";
        else
            SFXBtnSprite.spriteName = "nopebtn";
    }

    public void OnMusicBtn ()
    {
        if (!HMng.I.bMusic)
        {
            HMng.I.bMusic = true;
            HMng.I.MusicAudio.Play();
        }
        else
        {
            HMng.I.bMusic = false;
            HMng.I.MusicAudio.Stop();
        }
    }

    public void ChangeMusicSprite()
    {
        if (HMng.I.bMusic)
            MusicBtnSprite.spriteName = "checkedbtn";
        else
            MusicBtnSprite.spriteName = "nopebtn";
    }

    public void OnTutorialBtn ()
    {
        SceneManager.LoadScene(4);
    }

    public void OnRedColorBtn ()
    {
        if (!bRedOn)
        {
            bRedOn = true;
            bGreenOn = false;
            bBlueOn = false;
            bYellowOn = false;
            bPurpleOn = false;
            HMng.I.nColorType = (int)E_COLORTYPE.E_RED;
        }
    }

    public void OnGreenColorBtn ()
    {
        if (!bGreenOn)
        {
            bRedOn = false;
            bGreenOn = true;
            bBlueOn = false;
            bYellowOn = false;
            bPurpleOn = false;
            HMng.I.nColorType = (int)E_COLORTYPE.E_GREEN;
        }
    }

    public void OnBlueColorBtn ()
    {
        if (!bBlueOn)
        {
            bRedOn = false;
            bGreenOn = false;
            bBlueOn = true;
            bYellowOn = false;
            bPurpleOn = false;
            HMng.I.nColorType = (int)E_COLORTYPE.E_BLUE;
        }
    }

    public void OnYellowColorBtn ()
    {
        if (!bYellowOn)
        {
            bRedOn = false;
            bGreenOn = false;
            bBlueOn = false;
            bYellowOn = true;
            bPurpleOn = false;
            HMng.I.nColorType = (int)E_COLORTYPE.E_YELLOW;
        }
    }

    public void OnPurpleColorBtn ()
    {
        if (!bPurpleOn)
        {
            bRedOn = false;
            bGreenOn = false;
            bBlueOn = false;
            bYellowOn = false;
            bPurpleOn = true;
            HMng.I.nColorType = (int)E_COLORTYPE.E_PURPLE;
        }
    }

    public void ChangeColorSprite ()
    {
        if (bRedOn)
            RedBtnSprite.spriteName = "redchecked";
        else
            RedBtnSprite.spriteName = "red";

        if (bGreenOn)
            GreenBtnSprite.spriteName = "greenchecked";
        else
            GreenBtnSprite.spriteName = "green";

        if (bBlueOn)
            BlueBtnSprite.spriteName = "bluechecked";
        else
            BlueBtnSprite.spriteName = "blue";

        if (bYellowOn)
            YellowBtnSprite.spriteName = "yellowchecked";
        else
            YellowBtnSprite.spriteName = "yellow";

        if (bPurpleOn)
            PurpleBtnSprite.spriteName = "purplechecked";
        else
            PurpleBtnSprite.spriteName = "purple";
    }
}
