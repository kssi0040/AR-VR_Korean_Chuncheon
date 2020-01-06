using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipSceneButton : MonoBehaviour
{
    public StagePlay stagePlay;
    public ClipSceneManager clipSceneMgr;

    // Start is called before the first frame update
    void Start()
    {
        clipSceneMgr = FindObjectOfType<ClipSceneManager>();
        stagePlay = FindObjectOfType<StagePlay>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonEvent()
    {        
        if(PlayerInfo.Instance.isComplite)
        {
            stagePlay.forwardDown();
            clipSceneMgr.StartAnimation();
        }        
    }

    public void SkipButtonEvent()
    {
        clipSceneMgr.SkipAnimation();
        stagePlay.forwardDown();        
    }





}
