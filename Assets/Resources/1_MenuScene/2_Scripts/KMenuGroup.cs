using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KMenuGroup : MonoBehaviour {

    public UILabel BestScoreLabel;
    public GameObject QuitPopup;
    public GameObject CreditPopup;
    public GameObject SettingPopup;

    // Use this for initialization
    void Start () {
        BestScoreLabel.text += HMng.I.nHighScore.ToString();
        HMng.I.PlayMusic(HMng.I.bMusic);
    }
	
	// Update is called once per frame
	void Update () {
        if (!CreditPopup.activeSelf && !SettingPopup.activeSelf) // 두 팝업 모두 안 떠있을 때
        {
            if (Input.GetKeyUp(KeyCode.Escape))
                QuitPopup.SetActive(true);
        }

        if (CreditPopup.activeSelf && Input.GetKeyUp(KeyCode.Escape))
            CreditPopup.SetActive(false);
    }

    public void OnGameStart ()
    {
        SceneManager.LoadScene(2);
    }

    public void OnSettingPopupBtn()
    {
        SettingPopup.SetActive(true);
    }

    public void OnCreditPopupBtn ()
    {
        CreditPopup.SetActive(true);
    }

    public void OnCancelBtn ()
    {
        if (CreditPopup.activeSelf)
            CreditPopup.SetActive(false);
        else if (SettingPopup.activeSelf)
            SettingPopup.SetActive(false);
    }
}
