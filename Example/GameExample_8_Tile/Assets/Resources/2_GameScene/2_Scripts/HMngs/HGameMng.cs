using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MHomiLibrary;


[System.Serializable]  //  MonoBehaviour가 아닌 클래스에 대해 Inspector에 나타내기.
public class HTileIndexInfo
{
	public HTileInfo TileInfoSc = null;
	public int nIndex = 0;
}
	
public class HGameMng : HSingleton<HGameMng> {

    protected HGameMng() { }
    void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
            Destroy(gameObject);
    }

	public Camera HGameCam = null;
	public List<string> TileSpriteNameList  = null;
	public int nWidth = 6;
	public int nHeight = 15;
	public HTileIndexInfo[,] TileIndexs = null;
	public HTileOffsetGroup TOGroupSc = null;
	public HTileGroup TGroupSc = null;
	public int nTilesCnt = 0;					//타일 개수
	public UILabel DebugLab = null;

    void Start()
    {
		TileIndexsIdentity ();
    }

    void Update()
    {
		if (Input.GetKeyUp (KeyCode.Space)) {
			TOGroupSc.CreateTile ();
		}
			
		DebugLogLabel ();
		CheckBottomLineTiles ();
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy()/HGameMng.cs");
    }

	void DebugLogLabel()
	{
		DebugLab.text = string.Empty;

		for (int i = 0; i < nHeight; i++) {
			for (int j = 0; j < nWidth; j++) {

				DebugLab.text += TileIndexs [i, j].nIndex.ToString () + " ";
			}
			DebugLab.text += "\n";
		}
	}

	// init
	void TileIndexsIdentity()
	{
		TileIndexs = new HTileIndexInfo[nHeight, nWidth];
		
		for (int i = 0; i < nHeight; i++) {
			for (int j = 0; j < nWidth; j++) {
				TileIndexs[i, j] = new HTileIndexInfo();
				TileIndexs[i,j].nIndex = 0;
			}
		}
	}

	// 바 닥 체 크 
	void CheckBottomLineTiles()
	{
		// 바 닥 이 전 부 1 이 고
		// 타 일 이 전 부 E_STOP일 경 우
		int nCnt = 0;
		int nYIndex = nHeight - 1;

		for (int i = 0; i < nWidth; i++) 
		{
			if (TileIndexs [nYIndex, i].nIndex != 0) {
				nCnt++;
			} else {
				return;
			}
				
			if (TileIndexs [nYIndex, i].TileInfoSc.E_TSTATE == E_TILE_STATE.E_STOP) 
				nCnt++;
		}

		// 바닥이모두 1 이고, 모든 타일이 도착했다면 
		if (nCnt == (nWidth*2)) 
		{
			//삭 제 해 도 되 는 조 건 발 동 ^^
			for (int i = 0; i < nWidth; i++) 
			{
				TileIndexs [nYIndex, i].TileInfoSc.Reset ();
				SetTileIndex (nYIndex, i, 0, null);
			}

			// 한단계 아래로 이동시키라는  메시지전달
			SendMessageDownTile ();
		}
	}

	public int GetRandomIndex()
	{
		int nIndex = Random.Range (0, TileSpriteNameList.Count);
		return nIndex;
	}

	public int GetOffsetRandomIndex()
	{
		int nIndex = Random.Range (0, 6);
		return nIndex;
	}

	public string GetSpriteName(int nIndex)
	{
		return TileSpriteNameList [nIndex];
	}

	public void SetTileIndex(int nXIndex, int nYIndex, int nValue, HTileInfo HTInfo)
	{
		TileIndexs [nXIndex, nYIndex].nIndex = nValue;
		TileIndexs [nXIndex, nYIndex].TileInfoSc = HTInfo;
	}
		
	// 바닥체크 후 한 칸씩 내리기
	public void SendMessageDownTile()
	{
		int nYIndex = nHeight - 1;

		//바닥부터 
		for (int i = nYIndex; i >= 0; i--) 
		{
			for (int j = 0; j < nWidth; j++) 
			{
				if (TileIndexs [i, j].nIndex != 0) 
					TileIndexs [i, j].TileInfoSc.OneStepDown (i,j);
			}
		}
	}
}
