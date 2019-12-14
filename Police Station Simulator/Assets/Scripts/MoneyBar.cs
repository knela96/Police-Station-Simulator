using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBar : MonoBehaviour
{

    public Image HlthBar;
    public Text txt;
    public Text txt_anim;
    public int Max;
    public int Min;
    private int mCurrent = 0;
    int value = 0;
    public AudioSource asource;

    public AudioClip buy;
    public AudioClip sallaries;

    // Start is called before the first frame update
    void Start()
    {
        txt_anim.gameObject.SetActive(false);
        updateMoney(100);
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

    public void StartAnim(int _value,int audio)
    {
        if(audio == 0)
            asource.PlayOneShot(buy);
        else
            asource.PlayOneShot(sallaries);

        value = _value;
        StartCoroutine("MoneyAnimation");
    }

    IEnumerator MoneyAnimation()
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
