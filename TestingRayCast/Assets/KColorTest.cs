using UnityEngine;
using System.Collections;

public class KColorTest : MonoBehaviour {

    public SpriteRenderer ColorTestSprite;
    public Color DestColor = new Color(1f, 1f, 1f, 0f);
    public float fAniSpeed = 1f;
    public bool bAniSwitch = true;
    public Vector4 CurrentColorVec4;
    public Vector4 DestColorVec4;
	
	// Update is called once per frame
	void Update () {
        ColorAnimation();
    }

    void ColorAnimation ()
    {
        if (bAniSwitch)
        {
            CurrentColorVec4.x = ColorTestSprite.color.r;
            DestColorVec4.x = DestColor.r;
            CurrentColorVec4.x = Mathf.MoveTowards(CurrentColorVec4.x, DestColorVec4.x, fAniSpeed * Time.deltaTime);

            CurrentColorVec4.y = ColorTestSprite.color.g;
            DestColorVec4.y = DestColor.g;
            CurrentColorVec4.y = Mathf.MoveTowards(CurrentColorVec4.y, DestColorVec4.y, fAniSpeed * Time.deltaTime);

            CurrentColorVec4.z = ColorTestSprite.color.b;
            DestColorVec4.z = DestColor.b;
            CurrentColorVec4.z = Mathf.MoveTowards(CurrentColorVec4.z, DestColorVec4.z, fAniSpeed * Time.deltaTime);

            CurrentColorVec4.w = ColorTestSprite.color.a;
            DestColorVec4.w = DestColor.a;
            CurrentColorVec4.w = Mathf.MoveTowards(CurrentColorVec4.w, DestColorVec4.w, fAniSpeed * Time.deltaTime);

            ColorTestSprite.color = CurrentColorVec4;

            if (CurrentColorVec4.Equals(DestColorVec4))
            {
                bAniSwitch = false;
                Debug.Log("End");
            }
        }
    }
}