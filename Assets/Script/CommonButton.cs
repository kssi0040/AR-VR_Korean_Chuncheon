using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommonButton : MonoBehaviour
{
    public StagePlay stagePlay;
    public InputField signupID;
    public InputField signupPassword;

    // Start is called before the first frame update
    void Start()
    {
        stagePlay = FindObjectOfType<StagePlay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonDown()
    {
        if(Quiz_XML_Reader.Instance.readCompleted==true&&XML_Reader.Instance.readCompleted==true)
        {
            // 아래는 테스트용
            SceneManager.LoadScene("Prologue");

            // 아래는 릴리즈용...
            //LogInButtonEvent();
        }
    }

    public void Stage1ButtonDown()
    {
        if(Quiz_XML_Reader.Instance.readCompleted==true&&XML_Reader.Instance.readCompleted==true)
        {
            SceneManager.LoadScene("Stage1");
            //SceneManager.LoadScene("TestScene1");
            //SceneManager.LoadScene("TestScene");
        }
    }
    public void Stage2ButtonDown()
    {
        if (Quiz_XML_Reader.Instance.readCompleted == true && XML_Reader.Instance.readCompleted == true)
        {
            //SceneManager.LoadScene("TestScene2");
            SceneManager.LoadScene("Stage2");            
        }
    }
    public void Stage3ButtonDown()
    {
        if (Quiz_XML_Reader.Instance.readCompleted == true && XML_Reader.Instance.readCompleted == true)
        {
            //SceneManager.LoadScene("TestScene2");
            SceneManager.LoadScene("GPS_Scene");
        }
    }

    //=============================================== LOGIN =================================================//    
    public void LogInButtonEvent()
    {
        string tmpId = signupID.text;
        string tmpPw = signupPassword.text;

        if ("" == signupID.text || "" == signupPassword.text)
            return;

        DatabaseManager.Instance.LogInSystemEvent(tmpId, tmpPw);
    }
    // DataBaseManager 에서 이벤트 받음
    public void LoginEventCallBack(string _strMsg)
    {
        Debug.Log("login: " + _strMsg);
        if (signupID.text == _strMsg)
        {
            // 로그인 성공
            SceneManager.LoadScene("Prologue");
        }
        else
        {
            // 에러는 몇 가지로 나뉜다...

        }
    }

    public void JoinMemeberButtonEvent()
    {
        SceneManager.LoadScene("JoinMember");
    }




    public void SelectMapButtonDown()
    {
        SceneManager.LoadScene("SelectMap");
    }

    public void SignUpButtonEvent()
    {
        Application.OpenURL("http://eduarvr.dlinkddns.com/pages/login_admission/admission.php");
    }
}
