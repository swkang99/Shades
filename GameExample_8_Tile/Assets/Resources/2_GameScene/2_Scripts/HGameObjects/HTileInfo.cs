using UnityEngine;
using System.Collections;

public enum E_TILE_STATE
{
	E_DOWN,
	E_STOP
}

public class HTileInfo : MonoBehaviour {
	
	public UISprite UserSprite = null;

	public bool bUse = false;
	public int nTiletype = -1;
	public E_TILE_STATE E_TSTATE;
	public Transform Tm = null;

	public Vector3 DestVec3 = Vector3.zero;

	void Start () 
	{
		Init ();
	}
		
	void Update () 
	{
		if (bUse) {
			switch (E_TSTATE) {
			case E_TILE_STATE.E_DOWN:
				TileDown ();
				break;

			case E_TILE_STATE.E_STOP:
				break;
			}
		}
	}

	void Init()
	{
		E_TSTATE = E_TILE_STATE.E_STOP;
		UserSprite = transform.GetComponent<UISprite> ();
	}


	void TileDown()
	{
		transform.localPosition = Vector3.MoveTowards (transform.localPosition, DestVec3, Time.deltaTime * HGameMng.I.TGroupSc.fTileMoveSpeed);

		if (transform.localPosition == DestVec3) {
			E_TSTATE = E_TILE_STATE.E_STOP;
		}
	}

	//초 기 
	public void Reset()
	{
		bUse = false;
		nTiletype = -1;
		E_TSTATE = E_TILE_STATE.E_STOP;
		Tm.localPosition = Vector3.zero;
		DestVec3 = Vector3.zero;
	}

	// 한칸씩 내려가 
	public void OneStepDown(int nYIndex, int nXIndex)
	{
		Tm = this.transform;

		Vector3 nTempVec3 = Vector3.zero;

		HGameMng.I.SetTileIndex (nYIndex, nXIndex, 0, null);
		HGameMng.I.SetTileIndex (nYIndex+1, nXIndex, 1, (HTileInfo)this);

		nTempVec3.x = nXIndex * 40f;
		nTempVec3.y = -((nYIndex+1) * 40f);
		nTempVec3.z = 0f;

		DestVec3 = nTempVec3;

		E_TSTATE = E_TILE_STATE.E_DOWN;
	}
		
	// 처음생성시목적지계산
	public void CalcDestPos()
	{
		Tm = this.transform;

		Vector3 nTempVec3 = Vector3.zero;

		int nXIndex = (int)(transform.localPosition.x / 40f);
		int nYIndex = (int)(transform.localPosition.y / 40f);

		for (int y = 0; y < HGameMng.I.nHeight; y++) 
		{
			if (HGameMng.I.TileIndexs [y, nXIndex].nIndex != 0) 
			{
				HGameMng.I.SetTileIndex (y-1, nXIndex, 1, (HTileInfo)this);

				nTempVec3.x = nXIndex * 40f;
				nTempVec3.y = -((y-1) * 40f);
				nTempVec3.z = 0f;

				DestVec3 = nTempVec3;
				return;
			} 
		}

		//바닥 좌표 목적지
		HGameMng.I.SetTileIndex ((HGameMng.I.nHeight-1), nXIndex, 1, (HTileInfo)this);

		nTempVec3.x = nXIndex * 40f;
		nTempVec3.y = -((HGameMng.I.nHeight-1) * 40f);
		nTempVec3.z = 0f;

		DestVec3 = nTempVec3;
	}
}
