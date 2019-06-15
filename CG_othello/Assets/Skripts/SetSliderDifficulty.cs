using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderDifficulty : MonoBehaviour
{

    Slider slider;
    


    // Start is called before the first frame update
    void Start()
    {
       
        GameObject go = GameObject.Find("Slider");
        slider = go.GetComponent<Slider>();


    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("Difficulty", slider.value);
       // Debug.Log(PlayerPrefs.GetFloat("Difficulty"));
        
    }
}
