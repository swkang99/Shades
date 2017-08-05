using UnityEngine;
using System.Collections;
using MHomiLibrary;

public class HMng : HSingleton<HMng>
{
    protected HMng() { }

    public UIScrollBar HUserRedBar = null;
    public UIScrollBar HUserGreenBar = null;
    public UIScrollBar HUserBlueBar = null;
    public UIScrollBar HUserAlphaBar = null;

    public UISpriteAnimation HSAnimation = null;

    public UISprite HHeroSprite = null;

    void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        //HHeroSprite.color = new Color(HUserRedBar.value, HUserGreenBar.value, HUserBlueBar.value, HUserAlphaBar.value);
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy()/HMng.cs");
    }
}
