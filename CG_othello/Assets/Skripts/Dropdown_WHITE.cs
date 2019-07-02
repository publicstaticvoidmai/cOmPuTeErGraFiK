using UnityEngine;
using UnityEngine.UI;

public class Dropdown_WHITE : MonoBehaviour
{
    public Dropdown m_Dropdown;
    public GameObject Difficulty_AIWHITE;
    public Dropdown Difficulty_AI_W;
    
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
                PlayerPrefs.SetString("White", "HUMAN");
                Difficulty_AIWHITE.SetActive(false);
                
                break;
            case 1: 
                PlayerPrefs.SetString("White", "AI");
                Difficulty_AIWHITE.SetActive(true);
                PlayerPrefs.SetInt("AI_BLACK", Difficulty_AI_W.value == 0 ? 2 : 3);
                break;
        }
    }
}
