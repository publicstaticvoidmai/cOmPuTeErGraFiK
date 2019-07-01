using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_BLACK : MonoBehaviour
{
    public Dropdown m_Dropdown;

    
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
                PlayerPrefs.SetString("BLACK", "AI");
                break;
            
            case 1: 
                PlayerPrefs.SetString("BLACK", "HUMAN");
                break;
        }
        
    }
}
