using UnityEngine;
using System.Collections;

public enum E_TILE_STATE
{
    E_STOP,
    E_DOWN,
    E_TOUCHMOVE
}

public class KTile : MonoBehaviour {
    
    public int nTileColorDepth = 0; // 색 깊이 값 번호
    public bool bUse = false;
    public E_TILE_STATE ETileState = E_TILE_STATE.E_STOP;
    public Vector3 DestVec3;
    public UISprite TileSprite;
    public int nPrevXIndex; // 이전 자리의 X인덱스
    public int nPrevYIndex; // 이전 자리의 Y인덱스
    public int nCurrentXIndex; // 현재 자리의 X인덱스
    public int nCurrentYIndex; // 현재 자리의 Y인덱스
    public bool bMoved = false; // true: 이동한 타일, false: 이동한 적 없는 타일

    // Use this for initialization
    void Start() {
        TileSprite = GetComponent<UISprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bUse)
        {
            switch (ETileState)
            {
                case E_TILE_STATE.E_DOWN:
                    DownTiles();
                    break;

                case E_TILE_STATE.E_STOP:
                    break;

                case E_TILE_STATE.E_TOUCHMOVE:
                    TouchMovingTiles();
                    DownTiles();
                    break;
            }
        }
    }

    public void Reset()
    {
        TileSprite.color = Color.white;

        transform.localPosition = Vector3.zero;

        nTileColorDepth = 0;
        bUse = false;
        ETileState = E_TILE_STATE.E_STOP;
        DestVec3 = Vector3.zero;
        bMoved = false;
    }

    void DownTiles()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, DestVec3, HGameMng.I.TileGroupSc.fCurrentSpeed * Time.deltaTime);

        if (transform.localPosition == DestVec3)
        {
            ETileState = E_TILE_STATE.E_STOP;
            HGameMng.I.fTimeArray[(int)E_GTIME.E_CREATE] = Time.time;

            HGameMng.I.bScoreEftSwitch = true;
            HMng.I.nCurrentScore += 2;

            HGameMng.I.DownTileAudio.Play();

            if (HMng.I.nCurrentScore >= HGameMng.I.TileGroupSc.nSpeedUpScore * (HGameMng.I.TileGroupSc.nSpeedUpCount + 1)) // 점수가 nSpeedUpScore만큼 증가했는지 검사
            {
                HGameMng.I.TileGroupSc.nSpeedUpCount++;
                HGameMng.I.TileGroupSc.fOriginalSpeed += HGameMng.I.TileGroupSc.fSpeedUp;
            }
            HGameMng.I.TileGroupSc.fCurrentSpeed = HGameMng.I.TileGroupSc.fOriginalSpeed;
        }
    }

    void TouchMovingTiles ()
    {
        if (HGameMng.I.nTouchCount == 1) // 1번째 터치일 때
        {
            int nDestYIndex = HGameMng.I.nTouchYIndex;
            int nTouchXIndex = GetTileXIndex(); // 터치한 시점에서 타일의 X인덱스
            int nTouchYIndex = GetTileYIndex(); // 터치한 시점에서 타일의 Y인덱스

            if (nDestYIndex != nTouchYIndex) // 지금 있는 열과 터치한 열이 다를 때
            {
                if (HGameMng.I.TileIndexInfoArray[nTouchXIndex + 1, nDestYIndex].nIndex == 0) // 터치한 열에 타일의 현재 위치보다 높이 있는 타일이 없을 때
                {
                    bMoved = true;

                    Vector3 TempVec3 = transform.position;
                    TempVec3.x = HGameMng.I.OffsetGroupSc.OffsetTmList[nDestYIndex].position.x;
                    transform.position = TempVec3;

                    CalcDestPos(nDestYIndex, bMoved);

                    HGameMng.I.TileIndexInfoArray[nPrevXIndex, nPrevYIndex].Tile = null;
                    HGameMng.I.TileIndexInfoArray[nPrevXIndex, nPrevYIndex].nIndex = 0;
                }
            }
        }
        ETileState = E_TILE_STATE.E_DOWN;
        HGameMng.I.nTouchCount = 0;
    }

    /// <summary>
    /// 타일의 목적지 계산, 정보 삽입
    /// </summary>
    /// <param name="nYIndex">실행할 열</param>
    /// <param name="bMoved">터치로 이동한 타일인가?</param>
    public void CalcDestPos(int nYIndex, bool bMoved)
    {
        for (int i = 0; i < HGameMng.I.nHeight; i++)
        {
            // 쌓인 타일이 있을 때
            if (HGameMng.I.TileIndexInfoArray[i, nYIndex].nIndex != 0) 
            {
                HGameMng.I.TileIndexInfoArray[i - 1, nYIndex].Tile = this;
                HGameMng.I.TileIndexInfoArray[i - 1, nYIndex].nIndex = nTileColorDepth;

                DestVec3.x = transform.localPosition.x;
                DestVec3.y = -((i - 1) * TileSprite.height);
                DestVec3.z = 0f;

                if (!bMoved)
                {
                    nCurrentXIndex = i - 1;
                    nCurrentYIndex = nYIndex;
                }
                else
                {
                    nPrevXIndex = nCurrentXIndex;
                    nPrevYIndex = nCurrentYIndex;

                    nCurrentXIndex = i - 1;
                    nCurrentYIndex = nYIndex;
                }

                return;
            }
        }
        
        // 맨 밑 바닥으로 가야할 때
        HGameMng.I.TileIndexInfoArray[HGameMng.I.nHeight - 1, nYIndex].Tile = (KTile)this;
        HGameMng.I.TileIndexInfoArray[HGameMng.I.nHeight - 1, nYIndex].nIndex = nTileColorDepth;

        DestVec3.x = transform.localPosition.x;
        DestVec3.y = -((HGameMng.I.nHeight - 1) * TileSprite.height);
        DestVec3.z = 0f;

        if (!bMoved)
        {
            nCurrentXIndex = HGameMng.I.nHeight - 1;
            nCurrentYIndex = nYIndex;
        }
        else
        {
            nPrevXIndex = nCurrentXIndex;
            nPrevYIndex = nCurrentYIndex;

            nCurrentXIndex = HGameMng.I.nHeight - 1;
            nCurrentYIndex = nYIndex;
        }
    }

    public void OneStepDown (int nXIndex, int nYIndex)
    {
        // 한 칸 아래에 정보 복사
        HGameMng.I.TileIndexInfoArray[nXIndex + 1, nYIndex].Tile = this;
        HGameMng.I.TileIndexInfoArray[nXIndex + 1, nYIndex].nIndex = HGameMng.I.TileIndexInfoArray[nXIndex, nYIndex].nIndex;
        
        // 지금 칸 정보 비움
        HGameMng.I.TileIndexInfoArray[nXIndex, nYIndex].Tile = null;
        HGameMng.I.TileIndexInfoArray[nXIndex, nYIndex].nIndex = 0;

        // 한 칸 아래로 목적지 계산
        DestVec3.x = transform.localPosition.x;
        DestVec3.y = -((nXIndex + 1)) * TileSprite.height;
        DestVec3.z = 0f;

        ETileState = E_TILE_STATE.E_DOWN;
        HGameMng.I.TileGroupSc.fCurrentSpeed = HGameMng.I.TileGroupSc.fTileDownSpeed;
    }
    
    public int GetTileXIndex ()
    {
        int nXIndex = (int)((transform.localPosition.y * -1) / TileSprite.height);
        return nXIndex;
    }

    public int GetTileYIndex ()
    {
        int nYIndex = (int)(transform.localPosition.x / TileSprite.width);
        return nYIndex;
    }
}