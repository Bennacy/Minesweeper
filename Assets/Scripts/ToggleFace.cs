using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFace : MonoBehaviour
{
    private SavedInfo savedInfo;
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        savedInfo = GameObject.FindGameObjectWithTag("SavedInfo").GetComponent<SavedInfo>();
        toggle.onValueChanged.AddListener(delegate{savedInfo.ToggleFace(toggle.isOn);});
        toggle.isOn = savedInfo.animateFace;
    }
}
