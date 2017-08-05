using UnityEngine;
using System.Collections;

public class HMenuSceneBtn : MonoBehaviour {

    public void OnClick()
    {
        HSoundMng.I.Play("Tap");
        Application.LoadLevel(1);
    }
}
