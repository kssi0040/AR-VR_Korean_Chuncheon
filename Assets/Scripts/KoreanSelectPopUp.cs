using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KoreanSelectPopUp : MonoBehaviour
{
    public StagePlay m_StagePlay;
    private Button[] aButtons = new Button[4];
    
    private int iCount = -1;

    // Start is called before the first frame update
    void Awake()
    {
        m_StagePlay = FindObjectOfType<StagePlay>();
        for (int i = 0; i < aButtons.Length; ++i)
        {
            aButtons[i] = this.transform.GetChild(i + 1).gameObject.GetComponent<Button>();
            aButtons[i].onClick.AddListener(delegate { m_StagePlay.forwardDown(); });
        }        
    }

    // Update is called once per frame
    void Update()    {    }



    private void OnEnable()
    {
        if(-1 == iCount)
        {
            iCount++;
            return;
        }

        for (int i = 0; i < aButtons.Length; ++i)
        {            
            aButtons[i].interactable = false;
        }
        aButtons[iCount].interactable = true;        
        iCount++;        
    }

}
