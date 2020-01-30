using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KoreanStampPopUp : MonoBehaviour
{
    public StagePlay m_StagePlay;
    public GameObject[] aStampImage = new GameObject[4];
    private Button nextButton;

    private int iCount = -2;

    // Start is called before the first frame update
    void Awake()
    {
        m_StagePlay = FindObjectOfType<StagePlay>();
        for(int i = 0; i < aStampImage.Length; ++i)
        {
            aStampImage[i] = this.transform.GetChild(i + 1).gameObject.transform.GetChild(1).gameObject;
            aStampImage[i].SetActive(false);
        }
        nextButton = this.transform.GetChild(5).gameObject.transform.GetComponent<Button>();
        nextButton.onClick.AddListener(delegate { m_StagePlay.forwardDown(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnEnable()
    {                
        if (0 > iCount)
        {
            iCount++;
            return;
        }

        Debug.Log("count: " + iCount);
        aStampImage[iCount].SetActive(true);

        iCount++;
    }
}
