using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KLogoAni : MonoBehaviour
{
    public UISprite LogoSprite;
    public float fAniDelay = 0.05f;
    public float fAniSwapTime = 1f;
    public float fLogoToMenuTime = 0.5f;
    public string[] SpriteNameStr = new string[7];
    public int nAniNum = 0;
    public bool bAniReverse = false;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < SpriteNameStr.Length; i++)
            SpriteNameStr[i] = "logo_" + i.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        LogoAnimation();
    }

    public void LogoAnimation()
    {
        LogoSprite.spriteName = SpriteNameStr[nAniNum];

        if (!bAniReverse) // 애니메이션 재생
        {
            if (HMng.I.TimeCtrl((int)E_KTIME.E_LOGOANISPEED, fAniDelay) && nAniNum < SpriteNameStr.Length - 1)
            {
                nAniNum++;

                if (nAniNum == 6)
                    HMng.I.fTimeArray[(int)E_KTIME.E_LOGOANISWAP] = Time.time;
            }

            if (HMng.I.TimeCtrl((int)E_KTIME.E_LOGOANISWAP, fAniSwapTime) && !bAniReverse)
                bAniReverse = true;
        }
        else // 애니메이션 역재생
        {
            if (HMng.I.TimeCtrl((int)E_KTIME.E_LOGOANISPEED, fAniDelay) && nAniNum > 0)
            {
                nAniNum--;
                HMng.I.fTimeArray[(int)E_KTIME.E_LOGOTOMENU] = Time.time;

            }
            if (nAniNum == 0 && HMng.I.TimeCtrl((int)E_KTIME.E_LOGOTOMENU, fLogoToMenuTime)) // 일정시간 뒤 씬전환
            {
                HMng.I.bMusic = true;
                SceneManager.LoadScene(1);
            }
        }
    }
}