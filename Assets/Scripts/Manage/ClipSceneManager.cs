using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipSceneManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject light;
    public StagePlay stagePlay;
    public int sceneIndex;
    private GameObject clipChild;

    // Start is called before the first frame update
    void Start()
    {
        clipChild = this.transform.GetChild(0).gameObject;        
        //stagePlay = FindObjectOfType<StagePlay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NextButtonEvent()
    {        
        clipChild.SetActive(false);
        //stagePlay.forwardDown();
    }

    public void StartAnimation()
    {
        if (1 == sceneIndex)
        {
            mainCamera.transform.localPosition = new Vector3(0f, 2.34f, -9.97f);
            mainCamera.transform.eulerAngles = new Vector3(-3.735f, 21.282f, 0.0f);
        }
        else if (2 == sceneIndex)
        {
            mainCamera.transform.localPosition = new Vector3(38.0f, 1.0f, 292.0f);
            mainCamera.transform.eulerAngles = new Vector3(0.0f, 205.0f, 0.0f);
        }
        
        light.SetActive(false);
        clipChild.SetActive(true);
    }

    public void SkipAnimation()
    {
        mainCamera.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        mainCamera.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        //mainCamera.SetActive(true);
        light.SetActive(true);
        clipChild.SetActive(false);
    }
}
