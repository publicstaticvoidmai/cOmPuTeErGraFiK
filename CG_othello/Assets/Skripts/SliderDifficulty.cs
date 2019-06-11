using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderDifficulty : MonoBehaviour
{

    Slider slider;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("Difficulty", slider.value);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
