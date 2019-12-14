using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image HlthBar;
    public Text txt;
    public Text txt_anim;
    public int Max;
    public int Min;
    private int mCurrent;
    private float mCurrentPer;
    int value = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetBar(50);
        txt_anim.gameObject.SetActive(false);
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
        StartAnim(value);
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
    public void StartAnim(int _value)
    {
        value = _value;
        StartCoroutine("PopulAnimation");
    }

    IEnumerator PopulAnimation()
    {
        if (value > 0)
        {
            txt_anim.color = new Color(0.0f, 156.0f, 15.0f);
            txt_anim.text = string.Format("+{0}$", value);
        }
        else
        {
            txt_anim.color = new Color(255.0f, 0.0f, 0.0f);
            txt_anim.text = string.Format("{0}$", value);
        }
        txt_anim.gameObject.SetActive(true);
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            yield return new WaitForSeconds(.1f);
        }
        txt_anim.gameObject.SetActive(false);
    }
}
