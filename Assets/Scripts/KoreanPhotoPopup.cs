using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KoreanPhotoPopup : MonoBehaviour
{
    public StagePlay m_StagePlay;
    private Button GlassesButton;
    private Button NextButton;
    private GameObject textBg;
    private GameObject textDescribe;    


    // Start is called before the first frame update
    void Start()
    {
        m_StagePlay = FindObjectOfType<StagePlay>();
        GlassesButton = this.transform.GetChild(1).gameObject.GetComponent<Button>();
        NextButton = this.transform.GetChild(4).gameObject.GetComponent<Button>();
        textBg = this.transform.GetChild(2).gameObject;
        textDescribe = this.transform.GetChild(3).gameObject;

        GlassesButton.onClick.AddListener(delegate { GlassesButtonEvent(); });
        NextButton.onClick.AddListener(delegate { m_StagePlay.forwardDown(); });
    }

    // Update is called once per frame
    void Update()    { }

    void GlassesButtonEvent()
    {
        textBg.SetActive(true);
        textDescribe.SetActive(true);
        NextButton.gameObject.SetActive(true);
        //Debug.Log("test~!!!!");
    }







}
