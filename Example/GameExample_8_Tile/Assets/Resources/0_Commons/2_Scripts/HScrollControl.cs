using UnityEngine;
using System.Collections;


//[System.Serializable]
/// <summary>
/// 내용 : 스크롤 제어 클래스입니다
/// 위치 : 스크롤할 Object
/// </summary>
public class HScrollControl : MonoBehaviour {

    public UITexture ScrollSpirte = null;
    public bool bScrollFlag = false;
    public Vector2 fScrollSpeedVec2 = new Vector3(1f, 1f);

 //   [HideInInspector]
    Vector2 fMngScrollSpeedVec2 = new Vector3(1.0f, 1.0f);

    public string textureName = "_MainTex";

    Vector2 uvOffset = Vector2.zero;

    public Material Mat = null;
    // Use this for initialization
    void Start()
    {
        ScrollSpirte = transform.GetComponent<UITexture>();
    }
	
    void LateUpdate()
    {
        Vector2 MulVec = Vector2.zero;
        
        //if (ScrollSpirte.material)
        //{
            MulVec.x = fMngScrollSpeedVec2.x * fScrollSpeedVec2.x;
            MulVec.y = fMngScrollSpeedVec2.y * fScrollSpeedVec2.y;

            uvOffset += ((MulVec) * Time.deltaTime);

            ScrollSpirte.uvRect = new Rect(uvOffset.x , uvOffset.y, ScrollSpirte.uvRect.width, ScrollSpirte.uvRect.height);


            //ScrollSpirte.material.SetTextureOffset(textureName, uvOffset);
        //}
    }
}
