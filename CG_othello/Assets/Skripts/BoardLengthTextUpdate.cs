using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardLengthTextUpdate : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    public void TextUpdate(float value)
    {
        switch (value)
        {
            case 1:
                text.text = 6.ToString();
                PlayerPrefs.SetFloat("BoardLength", 6);
                break;
            
            case 2:
                text.text = 8.ToString();
                PlayerPrefs.SetFloat("BoardLength", 8);
                break;
            
            case 3:
                text.text = 10.ToString();
                PlayerPrefs.SetFloat("BoardLength", 10);
                break;
        }
    }
}
