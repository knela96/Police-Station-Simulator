using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBar : MonoBehaviour
{

    public Image HlthBar;
    public Text txt;
    public int Max;
    public int Min;
    private int mCurrent;
    private float mCurrentPer;

    public void SetBar(int health)
    {

        if (health != mCurrent)
        {

            if (Max - Min == 0 || health <= 0 )
            {

                mCurrent = 0;
                mCurrentPer = 0;
            }
            else
            {

                mCurrent = health;

                mCurrentPer = (float)mCurrent / (float)(Max - Min);


            }

            HlthBar.fillAmount = mCurrentPer;
            txt.text = string.Format("{0}", Mathf.RoundToInt(mCurrentPer*10000));
        }


    }

    public float CurrentPercent
    {

        get { return mCurrentPer; }

    }

    public float CurrentValue
    {

        get { return mCurrent; }

    }

    // Start is called before the first frame update
    void Start()
    {
        SetBar(50);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
