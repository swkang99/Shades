using UnityEngine;
using System.Collections;
using MHomiLibrary;

public enum E_KTIME
{
    E_LOGOANISPEED,
    E_LOGOANISWAP,
    E_LOGOTOMENU,
    E_MAX
}

public enum E_COLORTYPE
{
    E_RED,
    E_GREEN,
    E_BLUE,
    E_YELLOW,
    E_PURPLE
}

public class HMng : HSingleton<HMng>
{

    protected HMng() { }

    public float[] fTimeArray = new float[(int)E_KTIME.E_MAX];
    public int nCurrentScore = 0; // 현재 점수
    public int nHighScore = 0; // 최고 점수
    public int nColorType; // 색깔 종류
    public Color[,] TileColors = new Color[5, 5]; // 색 배열
    public AudioSource MusicAudio;
    public bool bMusic = false;

    void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
            Destroy(gameObject);

        nHighScore = PlayerPrefs.GetInt("nHighScore"); // 최고 점수 불러오기
        nColorType = PlayerPrefs.GetInt("nColorType"); // 색깔 종류 불러오기
    }

    void Start()
    {
        DontDestroyOnLoad(this);

        for (int i = 0; i < fTimeArray.Length; i++)
            fTimeArray[i] = Time.time;
       
        InitColor();

        Screen.SetResolution(480, 800, false);

        PlayMusic(bMusic);
    }

    /// <summary>
    /// 모든 블록의 색의 값을 세팅
    /// </summary>
    void InitColor()
    {
        // Red: 0(연함) ~ 4(진함)
        TileColors[0, 0] = new Color(1f, 190f / 255f, 190f / 255f);
        TileColors[0, 1] = new Color(1f, 118f / 255f, 121f / 255f);
        TileColors[0, 2] = new Color(172f / 255f, 0f, 0f);
        TileColors[0, 3] = new Color(101f / 255f, 0f, 0f);
        TileColors[0, 4] = new Color(60f / 255f, 0f, 0f);

        // Green: 0(연함) ~ 4(진함)
        TileColors[1, 0] = new Color(241f / 255f, 1f, 199f / 255f);
        TileColors[1, 1] = new Color(161f / 255f, 232f / 255f, 119f / 255f);
        TileColors[1, 2] = new Color(0f, 172f / 255f, 0f);
        TileColors[1, 3] = new Color(0f, 101f / 255f, 0f);
        TileColors[1, 4] = new Color(0f, 60f / 255f, 0f);

        // Blue: 0(연함) ~ 4(진함)
        TileColors[2, 0] = new Color(139f / 255f, 245f / 255f, 1f);
        TileColors[2, 1] = new Color(70f / 255f, 172 / 255f, 224f / 255f);
        TileColors[2, 2] = new Color(0f, 0f, 172f / 255f);
        TileColors[2, 3] = new Color(0f, 0f, 101f / 255f);
        TileColors[2, 4] = new Color(0f, 0f, 60f / 255f);

        // Yellow: 0(연함) ~ 4(진함)
        TileColors[3, 0] = new Color(1f, 1f, 220f / 255f);
        TileColors[3, 1] = new Color(1f, 240f / 255f, 136f / 255f);
        TileColors[3, 2] = new Color(1f, 147f / 255f, 0f);
        TileColors[3, 3] = new Color(198f / 255f, 66f / 255f, 0f);
        TileColors[3, 4] = new Color(115f / 255f, 10f / 255f, 0f);

        // Purple: 0(연함) ~ 4(진함)
        TileColors[4, 0] = new Color(1f, 234f / 255f, 252f / 255f);
        TileColors[4, 1] = new Color(237f / 255f, 173f / 255f, 233f / 255f);
        TileColors[4, 2] = new Color(188f / 255f, 103f / 255f, 180f / 255f);
        TileColors[4, 3] = new Color(122f / 255f, 55f / 255f, 113f / 255f);
        TileColors[4, 4] = new Color(73f / 255f, 24f / 255f, 59f / 255f);
    }

    public bool TimeCtrl (int nTimeIndex, float fDelay)
    {
        if (fTimeArray[nTimeIndex] + fDelay <= Time.time)
        {
            fTimeArray[nTimeIndex] = Time.time;
            return true;
        }
        else
            return false;
    }

    void Update()
    {
      
    }

    void OnApplicationQuit() // 앱이 종료되면 실행되는 함수
    {
        PlayerPrefs.SetInt("nHighScore", nHighScore); // 최고 점수 저장하기
        PlayerPrefs.SetInt("nColorType", nColorType); // 색깔 종류 저장하기
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy()/HMng.cs");
    }

    public void PlayMusic (bool bMusic)
    {
        if (bMusic)
            MusicAudio.Play();
    }
}
