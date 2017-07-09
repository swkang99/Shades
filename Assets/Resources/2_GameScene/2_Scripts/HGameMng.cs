using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MHomiLibrary;

public enum E_GTIME
{
    E_CREATE,
    E_MAX
}

// MonoBehaviour가 아닌 클래스에 대해 Inspector에 나타내기.
[System.Serializable]  
public class KTileIndexInfo
{
    public KTile Tile = null;
    public int nIndex = 0;
}

public class HGameMng : HSingleton<HGameMng> {

    protected HGameMng() { }

    public KTileGroup TileGroupSc;
    public KOffsetGroup OffsetGroupSc;
    public KTileIndexInfo[,] TileIndexInfoArray; // 타일의 정보를 담는 2차원 배열

    public int nWidth = 4; // 너비
    public int nHeight = 16; // 높이

    public UILabel DebugInfoLabel; // 모든 자리의 정보를 표시하는 라벨
	public UILabel DebugPosLabel; // 터치한 위치의 로컬 좌표를 표시하는 라벨

    public float[] fTimeArray = new float[(int)E_GTIME.E_MAX];
    public int[] nCntArray; // 행 단위로 모든 타일을 체크할 때 사용하는 배열

    public KRayCast KRay;
	public GameObject ColliderGam; // ...?
	public Vector3 TouchlocalVec3; // 터치한 위치의 로컬 좌표를 저장하는 변수
	public int nTouchYIndex; // 터치한 곳의 YIndex (열 번호, 타일은 횡이동)
    public int nTouchCount = 0; // 여러 번의 터치로 인해 실행이 여러 번 되는 것을 막는 변수

    public UILabel ScoreLabel; // 현재 점수를 표시하는 라벨
    public UISprite PauseBtnSprite; // 일시정지 버튼의 스프라이트

    public Vector3 SlideStartVec3; // 슬라이드 시작 지점
    public Vector3 SlideEndVec3; // 슬라이드 끝 지점 
    public bool bSlided = false; // 슬라이드 여부
    public float fSlideGap = 50f; // 슬라이드의 최소 길이
    public float fTileSpeedUp = 7f; // 슬라이드하면 타일의 속도가 (fTileSpeedUp)배만큼 증가

    public bool bColorAniSwitch = false;
    public float fTopColorAniSpeed = 1.63f; // 위 타일 애니메이션의 속도
    public float fBtmColorAniSpeed = 1f; // 아래 타일 애니메이션의 속도
    public int nColorAniTopTileXIndex; // 애니메이션에서 위 타일의 X인덱스
    public int nColorAniTileYIndex; // 애니메이션에서 타일의 Y인덱스
    public int nColorDepth; // 애니메이션 전 아래 타일의 색 깊이 값 및 애니메이션에서 아래 타일의 끝 색의 깊이 값
    public Vector4 TopCurrentColorVec4; // 위 타일의 현재 색 정보를 담는 변수
    public Vector4 TopDestColorVec4; // 위 타일의 끝 색 정보를 담는 변수
    public Vector4 BtmCurrentColorVec4; // 아래 타일의 현재 색 정보를 담는 변수
    public Vector4 BtmDestColorVec4; // 아래 타일의 끝 색 정보를 담는 변수

    public bool bLineAniSwitch = false;
    public float fLineAniSpeed = 1f;
    public Vector3 TileCurrentVec3; // 타일의 현재 위치를 담는 변수 (애니메이션을 실행할 때만 사용함)
    public Vector3 LineAniDestVec3; // 애니메이션의 목적지를 담는 변수
    public Vector4 CurrentColorVec4; // 타일의 현재 색 정보를 담는 변수
    public Vector4 DestColorVec4; // 타일의 끝 색 정보를 담는 변수
    public float fDestYPos = -1385f;
    public float fDestZPos = -100f;
    public int nLineAniXIndex; // 애니메이션을 실행할 행의 번호

    public UILabel TempScoreLabel; // 이펙트에 사용하는 라벨
    public Vector4 ScoreColorVec4; // 라벨의 색 정보를 담는 변수
    public float fScoreEftSpeed = 2.7f; // 이펙트의 속도
    public int nOnriginalFontSize; // 라벨 폰트의 원래 사이즈
    public int nPlusFont = 3; // 라벨의 사이즈 증가값
    public bool bScoreEftSwitch = false;

    public AudioSource DownTileAudio; // 타일을 내려놓을 때의 효과음
    
    void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        InitTileIndexInfo();
        InitArray();
        HMng.I.nCurrentScore = 0; // 현재 점수 초기화

        ScoreLabel.color = HMng.I.TileColors[HMng.I.nColorType, 1]; // 점수 라벨 색 변경
        TempScoreLabel.color = HMng.I.TileColors[HMng.I.nColorType, 1];
        PauseBtnSprite.color = HMng.I.TileColors[HMng.I.nColorType, 1]; // 일시정지 버튼 색 변경
        
        LineAniDestVec3.z = fDestZPos;

        nOnriginalFontSize = TempScoreLabel.fontSize;
    }
    
    void InitTileIndexInfo()
    {
        TileIndexInfoArray = new KTileIndexInfo[nHeight, nWidth]; // 높이=행, 너비=열

        for (int i = 0; i < nHeight; i++)
        {
            for (int j = 0; j < nWidth; j++)
            {
                TileIndexInfoArray[i, j] = new KTileIndexInfo();
                TileIndexInfoArray[i, j].nIndex = 0;
            }
        }
    }

    void InitArray()
    {
        for (int i = 0; i < fTimeArray.Length; i++)
            fTimeArray[i] = Time.time;

        nCntArray = new int[nHeight];
        for (int i = 0; i < nHeight; i++)
            nCntArray[i] = 0;
    }
    
    void Update()
    {
        DebugArray();
        CheckSameColorTile();
        TileColorAnimation();
        CheckSameTilesInLine();
        TileLineAnimation();
        CheckTheEndOfGame();

        for (int i = 0; i < TileGroupSc.KTileInfoList.Count; i++)
        {
            if (TileGroupSc.KTileInfoList[i].KTile.ETileState == E_TILE_STATE.E_DOWN && Time.timeScale !=0)
                TouchCtrl(TileGroupSc.KTileInfoList[i].KTile);
        }

        ScoreLabel.text = HMng.I.nCurrentScore.ToString();
        TempScoreLabel.text = HMng.I.nCurrentScore.ToString();
        PlusScoreEffect();
    }

    void DebugArray()
    {
        DebugInfoLabel.text = string.Empty;

        for (int i = 0; i < nHeight; i++)
        {
            for (int j = 0; j < nWidth; j++)
                DebugInfoLabel.text += TileIndexInfoArray[i, j].nIndex.ToString() + " ";

            DebugInfoLabel.text += "\n";
        }

        DebugPosLabel.text = TouchlocalVec3.ToString ();
    }

    void CheckSameTilesInLine() // 한 행의 타일들의 색이 모두 같은 지 체크
    {
        // 아래에서 위로 탐색
        for (int i = nHeight - 1; i >= 0; i--) 
        {
            for (int j = 0; j < nWidth - 1; j++)
            {
                if (TileIndexInfoArray[i, j].nIndex != 0 && TileIndexInfoArray[i, j + 1].nIndex != 0)
                {
                    if (TileIndexInfoArray[i, j].nIndex == TileIndexInfoArray[i, j + 1].nIndex) // 한 칸과 그 옆칸이 비어있지 않을 때
                    {
                        nCntArray[i]++;
                    }
                    if (TileIndexInfoArray[i, j].Tile.ETileState == E_TILE_STATE.E_STOP 
                        && TileIndexInfoArray[i, j + 1].Tile.ETileState == E_TILE_STATE.E_STOP) // 한 타일과 그 옆 타일이 움직이지 않을 때
                    {
                        nCntArray[i]++;
                    }
                }

                // 한 열의 타일이 모두 비어있지 않고, 움직이지 않을 때
                if (nCntArray[i] == (nWidth - 1) * 2) 
                {
                    nLineAniXIndex = i;
                    bLineAniSwitch = true;
                }
            }
            nCntArray[i] = 0;
        }
    }

    /// <summary>
    /// 타일 색이 모두 같은 행을 없애는 애니메이션
    /// </summary>
    void TileLineAnimation ()
    {
        if (bLineAniSwitch)
        {
            for (int i = 0; i < nWidth; i++)
            {
                CurrentColorVec4.x = TileIndexInfoArray[nLineAniXIndex, i].Tile.TileSprite.color.r;
                DestColorVec4.x = TileIndexInfoArray[nLineAniXIndex, i].Tile.TileSprite.color.r;
                CurrentColorVec4.x = Mathf.MoveTowards(CurrentColorVec4.x, DestColorVec4.x, fLineAniSpeed * Time.deltaTime);

                CurrentColorVec4.y = TileIndexInfoArray[nLineAniXIndex, i].Tile.TileSprite.color.g;
                DestColorVec4.y = TileIndexInfoArray[nLineAniXIndex, i].Tile.TileSprite.color.g;
                CurrentColorVec4.y = Mathf.MoveTowards(CurrentColorVec4.y, DestColorVec4.y, fLineAniSpeed * Time.deltaTime);

                CurrentColorVec4.z = TileIndexInfoArray[nLineAniXIndex, i].Tile.TileSprite.color.b;
                DestColorVec4.z = TileIndexInfoArray[nLineAniXIndex, i].Tile.TileSprite.color.b;
                CurrentColorVec4.z = Mathf.MoveTowards(CurrentColorVec4.z, DestColorVec4.z, fLineAniSpeed * Time.deltaTime);

                CurrentColorVec4.w = TileIndexInfoArray[nLineAniXIndex, i].Tile.TileSprite.color.a;
                DestColorVec4.w = 0f;
                CurrentColorVec4.w = Mathf.MoveTowards(CurrentColorVec4.w, DestColorVec4.w, fLineAniSpeed * Time.deltaTime);

                TileIndexInfoArray[nLineAniXIndex, i].Tile.TileSprite.color = CurrentColorVec4;

                if (CurrentColorVec4 == DestColorVec4)
                {
                    bLineAniSwitch = false;
                    RemoveSameTilesInLine(nLineAniXIndex);
                    DownTilesInTopLine(nLineAniXIndex);
                    break;
                }
            }
        }
    }

    void RemoveSameTilesInLine(int nXIndex)
    {
        for (int i = 0; i < nWidth; i++)
        { 
            TileIndexInfoArray[nXIndex, i].Tile.Reset();
            TileIndexInfoArray[nXIndex, i].nIndex = 0;
            TileIndexInfoArray[nXIndex, i].Tile = null;
        }

        HMng.I.nCurrentScore += 200;
        bScoreEftSwitch = true;
    }

    void DownTilesInTopLine(int nXIndex)
    {
        // 없앤 줄부터 시작해서 아래에서 위로 탐색, 맨 밑 줄은 탐색하지 않음
        for (int i = nXIndex; i >= 0; i--)
        {
            for (int j = 0; j < nWidth; j++)
            {
                if (TileIndexInfoArray[i, j].nIndex != 0)
                    TileIndexInfoArray[i, j].Tile.OneStepDown(i, j);
            }
        }
    }

    /// <summary>
    /// 한 타일과 그 아래 타일의 색이 같은 지 체크
    /// </summary>
    void CheckSameColorTile()
    {
        for (int i = 0; i < nHeight - 1; i++)
        {
            for (int j = 0; j < nWidth; j++)
            {
                if (TileIndexInfoArray[i, j].nIndex != 0) // 비어있는 자리가 아닐 때(타일이 있을 때)
                {
                    if (TileIndexInfoArray[i, j].nIndex == TileIndexInfoArray[i + 1, j].nIndex) // 색깔 인덱스가 같을 때
                    {
                        if (TileIndexInfoArray[i, j].Tile.ETileState == E_TILE_STATE.E_STOP
                            && TileIndexInfoArray[i + 1, j].Tile.ETileState == E_TILE_STATE.E_STOP) // 두 타일 다 멈추어 있을 때
                        {
                            if (TileIndexInfoArray[i + 1, j].Tile.nTileColorDepth < 5) // 아래 타일 색이 제일 진하지 않을 때  
                            {
                                // 색 깊이 값을 nColorDepth 변수에 복사
                                nColorDepth = TileIndexInfoArray[i + 1, j].Tile.nTileColorDepth;

                                // 애니메이션 실행 및 필요한 정보 복사
                                bColorAniSwitch = true;
                                nColorAniTopTileXIndex = i;
                                nColorAniTileYIndex = j;

                                // 주석처리된 코드: 색을 즉시 변경
                                //TileIndexInfoArray[i, j].Tile.TileSprite.color = Color.white;
                                //TileIndexInfoArray[i + 1, j].Tile.TileSprite.color = HMng.I.TileColors[HMng.I.nColorType, nColorDepth - 1];
                            }
                        }
                    }
                    if (TileIndexInfoArray[i + 1, j].nIndex == 0)
                        TileIndexInfoArray[i, j].Tile.OneStepDown(i, j);
                }
            }
        }
    }

    void TileColorAnimation ()
    {
        if (bColorAniSwitch)
        {
            // 위 타일: 점점 투명해짐
            TopCurrentColorVec4.x = TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].Tile.TileSprite.color.r;
            TopDestColorVec4.x = TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].Tile.TileSprite.color.r;
            //TopCurrentColorVec4.x = Mathf.MoveTowards(TopCurrentColorVec4.x, TopDestColorVec4.x, fTopColorAniSpeed * Time.deltaTime);

            TopCurrentColorVec4.y = TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].Tile.TileSprite.color.g;
            TopDestColorVec4.y = TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].Tile.TileSprite.color.g;
            //TopCurrentColorVec4.y = Mathf.MoveTowards(TopCurrentColorVec4.y, TopDestColorVec4.y, fTopColorAniSpeed * Time.deltaTime);

            TopCurrentColorVec4.z = TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].Tile.TileSprite.color.b;
            TopDestColorVec4.z = TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].Tile.TileSprite.color.b;
            //TopCurrentColorVec4.z = Mathf.MoveTowards(TopCurrentColorVec4.z, TopDestColorVec4.z, fTopColorAniSpeed * Time.deltaTime);

            TopCurrentColorVec4.w = TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].Tile.TileSprite.color.a;
            TopDestColorVec4.w = 0f;
            TopCurrentColorVec4.w = Mathf.MoveTowards(TopCurrentColorVec4.w, TopDestColorVec4.w, fTopColorAniSpeed * Time.deltaTime);

            TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].Tile.TileSprite.color = TopCurrentColorVec4;

            // 아래 타일: 점점 다음 단계 색으로 진해짐
            BtmCurrentColorVec4.x = TileIndexInfoArray[nColorAniTopTileXIndex + 1, nColorAniTileYIndex].Tile.TileSprite.color.r;
            BtmDestColorVec4.x = HMng.I.TileColors[HMng.I.nColorType, nColorDepth].r;
            BtmCurrentColorVec4.x = Mathf.MoveTowards(BtmCurrentColorVec4.x, BtmDestColorVec4.x, fBtmColorAniSpeed * Time.deltaTime);

            BtmCurrentColorVec4.y = TileIndexInfoArray[nColorAniTopTileXIndex + 1, nColorAniTileYIndex].Tile.TileSprite.color.g;
            BtmDestColorVec4.y = HMng.I.TileColors[HMng.I.nColorType, nColorDepth].g;
            BtmCurrentColorVec4.y = Mathf.MoveTowards(BtmCurrentColorVec4.y, BtmDestColorVec4.y, fBtmColorAniSpeed * Time.deltaTime);

            BtmCurrentColorVec4.z = TileIndexInfoArray[nColorAniTopTileXIndex + 1, nColorAniTileYIndex].Tile.TileSprite.color.b;
            BtmDestColorVec4.z = HMng.I.TileColors[HMng.I.nColorType, nColorDepth].b;
            BtmCurrentColorVec4.z = Mathf.MoveTowards(BtmCurrentColorVec4.z, BtmDestColorVec4.z, fBtmColorAniSpeed * Time.deltaTime);

            BtmCurrentColorVec4.w = TileIndexInfoArray[nColorAniTopTileXIndex + 1, nColorAniTileYIndex].Tile.TileSprite.color.a;
            BtmDestColorVec4.w = HMng.I.TileColors[HMng.I.nColorType, nColorDepth].a;
            BtmCurrentColorVec4.w = Mathf.MoveTowards(BtmCurrentColorVec4.w, BtmDestColorVec4.w, fBtmColorAniSpeed * Time.deltaTime);

            TileIndexInfoArray[nColorAniTopTileXIndex + 1, nColorAniTileYIndex].Tile.TileSprite.color = BtmCurrentColorVec4;

            // 두 타일 다 애니메이션이 끝났을 때
            if (TopCurrentColorVec4 == TopDestColorVec4 && BtmCurrentColorVec4 == BtmDestColorVec4) 
            {
                bColorAniSwitch = false;

                // 두 타일의 정보 합성
                TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].nIndex = 0;
                TileIndexInfoArray[nColorAniTopTileXIndex, nColorAniTileYIndex].Tile.Reset();
                TileIndexInfoArray[nColorAniTopTileXIndex + 1, nColorAniTileYIndex].nIndex++;
                TileIndexInfoArray[nColorAniTopTileXIndex + 1, nColorAniTileYIndex].Tile.nTileColorDepth = nColorDepth + 1;

                nColorDepth = 0;
                HMng.I.nCurrentScore += 4;
                bScoreEftSwitch = true;
            }
        }
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
    
    void TouchCtrl (KTile Tile)
    {
        if (Input.GetMouseButton(0)) // 터치
        {
            KRay.m_Hit = KRay.GetHitInfo();

            if (KRay.m_Hit.collider != null)
            {
                if (KRay.m_Hit.collider.tag == "KCollider")
                {
                    nTouchCount++;

                    if (!bSlided)
                    {
                        SlideStartVec3 = Input.mousePosition;
                        bSlided = true;
                    }

                    // 터치한 곳의 로컬 좌표 계산
                    Matrix4x4 TempMat = ColliderGam.transform.worldToLocalMatrix;
                    TouchlocalVec3 = TempMat * KRay.ClickPointVec3;

                    DebugPosLabel.text = "Fuck5";

                    // 터치한 곳의 YIndex 계산
                    nTouchYIndex = (int)(TouchlocalVec3.x / Tile.TileSprite.width);

                    Tile.ETileState = E_TILE_STATE.E_TOUCHMOVE;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) // 슬라이드
        {
            SlideEndVec3 = Input.mousePosition;
            bSlided = false;
            float fDeltaYPos = SlideStartVec3.y - SlideEndVec3.y;

            if (fDeltaYPos > fSlideGap)
                TileGroupSc.fCurrentSpeed *= fTileSpeedUp;
        }
    }

    void CheckTheEndOfGame ()
    {
        for (int i = 0; i < nWidth; i++)
        {
            if (TileIndexInfoArray[1, i].nIndex != 0 && TileIndexInfoArray[1, i].Tile.ETileState == E_TILE_STATE.E_STOP) // 1 = 화면 속 맨 위 행 번호
            {
                if (HMng.I.nCurrentScore > HMng.I.nHighScore)
                    HMng.I.nHighScore = HMng.I.nCurrentScore;

                SceneManager.LoadScene(3);
            }
        }
    }
    
    public int GetOffsetCount()
    {
        return OffsetGroupSc.OffsetTmList.Count;
    }

    /// <summary>
    /// 점수가 올라갈 때의 이펙트(시각 효과)
    /// </summary>
    public void PlusScoreEffect ()
    {
        if (bScoreEftSwitch)
        {
            TempScoreLabel.fontSize += nPlusFont;
            ScoreColorVec4.x = TempScoreLabel.color.r;
            ScoreColorVec4.y = TempScoreLabel.color.g;
            ScoreColorVec4.z = TempScoreLabel.color.b;
            ScoreColorVec4.w = TempScoreLabel.color.a;
            ScoreColorVec4.w = Mathf.MoveTowards(ScoreColorVec4.w, 0f, fScoreEftSpeed * Time.deltaTime);
            TempScoreLabel.color = ScoreColorVec4;

            if (ScoreColorVec4.w == 0f)
            {
                TempScoreLabel.fontSize = nOnriginalFontSize;
                TempScoreLabel.color = HMng.I.TileColors[HMng.I.nColorType, 1];
                bScoreEftSwitch = false;
            }
        }
    }
}