using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_BLACK : MonoBehaviour
{
    public Dropdown m_Dropdown;
    public GameObject Difficulty_AIBLACK;
    public Dropdown Difficulty_AI_B;
    

    
    // Start is called before the first frame update
    void Start()
    {
        
        m_Dropdown = GetComponent<Dropdown>();
        
    }

    
    void Update()
    {
        switch (m_Dropdown.value)
        {
            case 0:
                PlayerPrefs.SetString("Black", "AI");
                Difficulty_AIBLACK.SetActive(true);
                PlayerPrefs.SetInt("Black_AI", Difficulty_AI_B.value == 0 ? 3 : 2);
                break;
            
            case 1: 
                PlayerPrefs.SetString("Black", "HUMAN");
                Difficulty_AIBLACK.SetActive(false);
                break;
        }
        
    }
}
