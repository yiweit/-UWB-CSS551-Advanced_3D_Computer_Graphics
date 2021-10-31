using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XFormControl : MonoBehaviour
{
    public Toggle T, R, S;
    public SliderWithEcho X, Y, Z;
    public Text ObjectName;

    private Transform mSelected;
    private Vector3 mPrevSliderVal = Vector3.zero;

    


    // Start is called before the first frame update
    void Start()
    {
        DefaultView();
    }

    public void DefaultView()
    {

        T.onValueChanged.AddListener(SetToTranslation);
        R.onValueChanged.AddListener(SetToRotation);
        // S.onValueChanged.AddListener(SetToScaling);

        X.SetSliderListener(XValueChanged);
        Y.SetSliderListener(YValueChanged);
        Z.SetSliderListener(ZValueChanged);

        T.isOn = true;
        R.isOn = false;
        S.isOn = false;
        SetToTranslation(true);
    }

    public void UnSelectCurrent()
    {
        ObjectName.text = "None";
        mSelected = null;
        UpdateUI();
    }

    // toggle actions
    void SetToTranslation(bool v)
    {
        //Debug.Log("Run SetToTranslation in XFormControl");
        Vector3 p = GetSelectedXFormParameter();
        mPrevSliderVal = p;
        // Init3SlidersRange(-24, 24, p);
        X.InitSliderRange(-17, 17, p.x);
        Y.InitSliderRange(-6, 17, p.y);
        Z.InitSliderRange(-5, 24, p.z);
        
    }

    void SetToRotation(bool v)
    {
        //Debug.Log("Run SetToRotation in XFormControl");
        Vector3 p = GetSelectedXFormParameter();
        mPrevSliderVal = p;
        Init3SlidersRange(-180, 180, p);
    }

    void SetToScaling(bool v)
    {
        //Debug.Log("Run SetToScaling in XFormControl");
        Vector3 p = GetSelectedXFormParameter();
        mPrevSliderVal = p;
        Init3SlidersRange(1f, 5f, p);
    }

    void Init3SlidersRange(float min, float max, Vector3 p)
    {
        X.InitSliderRange(min, max, p.x);
        Y.InitSliderRange(min, max, p.y);
        Z.InitSliderRange(min, max, p.z);
    }

    // respond to slide bars changes
    void XValueChanged(float v) { valueChanged(v, "x"); }

    void YValueChanged(float v) { valueChanged(v, "y"); }

    void ZValueChanged(float v) { valueChanged(v, "z"); }

    void valueChanged(float v, string label)
    {
        //Debug.Log("Run valueChanged on " +label+ " in XFormControl");
        Vector3 p = GetSelectedXFormParameter();
        float diff;
        Quaternion q;
        if (label == "x")
        {
            diff = v - mPrevSliderVal.x;
            mPrevSliderVal.x = v;
            q = Quaternion.AngleAxis(diff, Vector3.right);
            p.x = v;
        }
        else if (label == "y")
        {
            diff = v - mPrevSliderVal.y;
            mPrevSliderVal.y = v;
            q = Quaternion.AngleAxis(diff, Vector3.up);
            p.y = v;
        }
        else
        {
            diff = v - mPrevSliderVal.z;
            mPrevSliderVal.z = v;
            q = Quaternion.AngleAxis(diff, Vector3.forward);
            p.z = v;
        }

        SetSelectedXform(ref p, ref q);
    }


    // New object selected
    public void SetSelectedObject(Transform xform)
    {
        // Debug.Log("Run SetSelectedObject in XFormControl");
        mSelected = xform;
        mPrevSliderVal = Vector3.zero;
        if (xform != null)
        {
            ObjectName.text = xform.name;
        }
        else
        {
            ObjectName.text = "None";
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        //Debug.Log("Run UpdateUI in XFormControl");
        Vector3 p = GetSelectedXFormParameter();
        X.SetSliderValue(p.x);
        Y.SetSliderValue(p.y);
        Z.SetSliderValue(p.z);
    }

    private Vector3 GetSelectedXFormParameter()
    {
        //Debug.Log("Run GetSelectedXFormParameter in XFormControl");
        Vector3 p;
        if (T.isOn)
        {

            if (mSelected != null)
            {
                p = mSelected.localPosition;
            }
            else
            {
                p = Vector3.zero;
            }
        }
        else if (S.isOn)
        {

            if (mSelected != null)
            {
                p = mSelected.localScale;
            }
            else
            {
                p = Vector3.zero;
            }
        }
        else
        {

            p = Vector3.zero;

        }
        return p;
    }

    private void SetSelectedXform(ref Vector3 p, ref Quaternion q)
    {
        if (mSelected != null)
        {
            if (T.isOn)
            {
                mSelected.localPosition = p;
            }
            else if (S.isOn)
            {
                mSelected.localScale = p;
            }
            else
            {
                mSelected.localRotation *= q;
            }
        }

    }

}