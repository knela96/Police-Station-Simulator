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
    private int mCurrent = 0;

    // Start is called before the first frame update
    void Start()
    {
        updateMoney(1500);
    }

    public void updateMoney(int value)
    {
        mCurrent = mCurrent + value;
        SetBar((int)mCurrent);
    }

    public void SetBar(int value)
    {
        txt.text = string.Format("{0}$", mCurrent);
    }

    public float CurrentValue
    {
        get { return mCurrent; }
    }
}
