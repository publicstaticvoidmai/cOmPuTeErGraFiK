using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyTextUpdate : MonoBehaviour
{

    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    public void TextUpdate(float value)
    {
        PlayerPrefs.SetFloat("Difficulty", value);
        text.text = value.ToString();
    }
}
