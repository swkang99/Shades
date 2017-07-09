using UnityEngine;
using System.Collections;

public class KQuitPopup : MonoBehaviour
{
    public UISprite BGSprite;
    public GameObject QuitPopup;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BGSprite.color = HMng.I.TileColors[HMng.I.nColorType, 1];

        if (Input.GetKeyUp(KeyCode.Escape))
            QuitPopup.SetActive(false);
    }

    public void OnQuitBtn()
    {
        Application.Quit();
    }

    public void OnNoBtn ()
    {
        QuitPopup.SetActive(false);
    }
}
