using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// MonoBehaviour가 아닌 클래스에 대해 Inspector에 나타내기.
[System.Serializable]  
public class KTileInfo
{
    public Transform KTileTm = null;
    public KTile KTile = null;
}

public class KTileGroup : MonoBehaviour {

    public List<KTileInfo> KTileInfoList = new List<KTileInfo>();
    public float fCurrentSpeed; // 현재 속도
    public float fOriginalSpeed = 150f; // 원래 속도
    public float fSpeedUp = 10f; // 원래 속도의 증가폭
    public float fTileDownSpeed;
    public int nSpeedUpScore = 50; // 점수가 nSpeedUpsScore점만큼 속도 증가
    public int nSpeedUpCount = 0; // 속도가 증가한 횟수
    public float fCreateDelay = 0.1f;
    int nOffestIndex;
    public int nCurrentColorDepth;
    public int nNextColorDepth;
    public UISprite ColorNoticeSprite; // 다음 타일의 색을 알려주는 스프라이트

    // Use this for initialization
    void Start () {
        InitList();
        nNextColorDepth = Random.Range(0, HMng.I.TileColors.GetLength(1) - 1);
        ColorNoticeSprite.color = HMng.I.TileColors[HMng.I.nColorType, nNextColorDepth];
        nCurrentColorDepth = nNextColorDepth;
        fCurrentSpeed = fOriginalSpeed;
        fTileDownSpeed = fOriginalSpeed;
    }

    void InitList()
    {
        int nCount = transform.childCount;
        for (int i = 0; i < nCount; i++)
        {
            KTileInfo TempInfo = new KTileInfo();
            TempInfo.KTileTm = transform.FindChild("KTile (" + i.ToString() + ")");
            TempInfo.KTile = TempInfo.KTileTm.GetComponent<KTile>();
            KTileInfoList.Add(TempInfo);
        }
    }

    // Update is called once per frame
    void Update () {
        if (HGameMng.I.TimeCtrl((int)E_GTIME.E_CREATE, fCreateDelay))
        {
            if (CheckForCreate())
                CreateTile();
        }
    }
    
    public void CreateTile ()
    {
        nOffestIndex = Random.Range(0, HGameMng.I.GetOffsetCount());
        nNextColorDepth = Random.Range(0, HMng.I.TileColors.GetLength(1) - 1); // 0부터 4까지
        for (int i = 0; i < KTileInfoList.Count; i++)
        {
            if (!HGameMng.I.bColorAniSwitch && !HGameMng.I.bLineAniSwitch) // 두 타일을 합치는 애니메이션과 색이 같은 줄이 사라지는 애니메이션이 끝났을 때
            {
                if (!KTileInfoList[i].KTile.bUse)
                {
                    KTileInfoList[i].KTileTm.position = HGameMng.I.OffsetGroupSc.OffsetTmList[nOffestIndex].position;
                    KTileInfoList[i].KTile.bMoved = false;

                    KTileInfoList[i].KTile.TileSprite.color = HMng.I.TileColors[HMng.I.nColorType, nCurrentColorDepth]; // 0 : 색깔 종류 번호 (0 ~ 4)
                    KTileInfoList[i].KTile.nTileColorDepth = nCurrentColorDepth + 1;

                    KTileInfoList[i].KTile.bUse = true;

                    int nYIndex = KTileInfoList[i].KTile.GetTileYIndex();
                    bool bMoved = KTileInfoList[i].KTile.bMoved;
                    KTileInfoList[i].KTile.CalcDestPos(nYIndex, bMoved);

                    KTileInfoList[i].KTile.ETileState = E_TILE_STATE.E_DOWN;

                    // 다음 색깔 예고
                    ColorNoticeSprite.color = HMng.I.TileColors[HMng.I.nColorType, nNextColorDepth];
                    nCurrentColorDepth = nNextColorDepth;
                    break;
                }
            }
        }
    }

    bool CheckForCreate ()
    {
        int nTileCnt = 0; // 현재 내려와 있는 타일의 개수
        int nStopTile = 0; // 현재 멈춰 있는 타일의 개수

        for (int i = 0; i < HGameMng.I.nHeight; i++)
        {
            for (int j = 0; j < HGameMng.I.nWidth; j++)
            {
                if (HGameMng.I.TileIndexInfoArray[i, j].nIndex != 0)
                {
                    nTileCnt++; // 내려와 있는 타일(비어있지 않은 자리)의 개수를 센다

                    if (HGameMng.I.TileIndexInfoArray[i, j].Tile.ETileState == E_TILE_STATE.E_STOP)
                        nStopTile++; // 멈춰있는 타일의 개수를 센다
                }
            }
        }

        if (nTileCnt == nStopTile) // 내려 온 타일들이 모두 멈춰있을 때
            return true;
        else
            return false;
    }
}