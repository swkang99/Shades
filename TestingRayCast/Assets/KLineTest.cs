using UnityEngine;
using System.Collections;

public class KLineTest : MonoBehaviour {

    public SpriteRenderer TestSprite;
    public Vector3 TempVec3;
    public float fLineAniSpeed = 1f;
    public bool bLineAniSwitch = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        LineAni();
	}

    void LineAni ()
    {
        if (bLineAniSwitch)
        {
            TestSprite.transform.position = Vector3.MoveTowards(TestSprite.transform.position, TempVec3, fLineAniSpeed * Time.deltaTime);

            if (TestSprite.transform.position == TempVec3)
            {
                TempVec3.y = -5f;
                TestSprite.transform.position = Vector3.MoveTowards(TestSprite.transform.position, TempVec3, fLineAniSpeed * Time.deltaTime);

                if (TestSprite.transform.position == TempVec3)
                {
                    bLineAniSwitch = false;
                }
            }
        }
    }
}
