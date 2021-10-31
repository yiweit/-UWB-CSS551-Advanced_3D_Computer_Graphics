using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithEcho : MonoBehaviour
{
    public Slider TheSlider = null;
    public Text TheEcho = null;
    public Text TheLabel = null;

    public delegate void SliderCallbackDelegate(float v);      // defined a new data type
    private SliderCallbackDelegate mCallBack = null;           // private instance of the data type


    // Use this for initialization
    void Start()
    {

        //Debug.Log("start the Slider with Echo");
        TheSlider.onValueChanged.AddListener(SliderValueChange);
    }

    public void SetSliderListener(SliderCallbackDelegate listener)
    {
        //Debug.Log("Run SetSliderListener at " + listener.ToString() + " on SliderwEcho");
        mCallBack = listener;



    }

    // GUI element changes the object
    void SliderValueChange(float v)
    {
        //Debug.Log("Run SliderValueChange at " + v.ToString() + " on SliderwEcho");

        TheEcho.text = v.ToString("0.000");
        //Debug.Log("SliderValueChange: " + v);
        //Debug.Log(Time.deltaTime);
        if (mCallBack != null)
        {
            //Debug.Log("mCallback below");
            //Debug.Log(mCallBack.ToString());
            mCallBack(v);

        }

    }

    public float GetSliderValue() { return TheSlider.value; }
    public void SetSliderLabel(string l) { TheLabel.text = l; }
    public void SetSliderValue(float v)
    {
        //Debug.Log("Set " + TheLabel.text + " to " + v.ToString());
        TheSlider.value = v;
        SliderValueChange(v);
    }
    public void InitSliderRange(float min, float max, float v)
    {
        TheSlider.minValue = min;
        TheSlider.maxValue = max;
        SetSliderValue(v);
    }
}