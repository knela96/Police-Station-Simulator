using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image HlthBar;
    public Text txt;
    public int Max;
    public int Min;
    private int mCurrent;
    private float mCurrentPer;
    
    // Start is called before the first frame update
    void Start()
    {
        SetBar(50);
    }

    public void SetBar(int health) {
        
        if (Max - Min == 0)
        {
            mCurrent = 0;
            mCurrentPer = 0;
        }
        else {
            mCurrent = health;
            mCurrentPer = (float)mCurrent / (float)(Max - Min);
        }
        HlthBar.fillAmount = mCurrentPer;
    }

    public void updatePopul(int value)
    {
        mCurrent = mCurrent + value;
        if (mCurrent > Max)
            mCurrent = Max;
        if (mCurrent < Min)
            mCurrent = Min;
        SetBar(mCurrent);
    }

    public float CurrentPercent
    {
        get { return mCurrentPer; }
    }

    public float CurrentValue
    {
        get { return mCurrent; }

    }
}
