using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using System;

public class GPS_System : MonoBehaviour
{
    [Header("UI")]
    //public Text gpsNotice;    
    public GameObject mainCanvas;
    public GameObject ImageBundle;
    public Text describeText;

    //public GameObject CityButtons;
    public GameObject BackButton;
    //public GameObject Ansan;
    //public GameObject Buyeo;
    //public GameObject Chang_neong;
    //public GameObject Chun_chen;
    public string AppID = "";
    public string strEdu_type = null;
    public string strSpot1 = null;
    public string strSpot2 = null;
    public string strSpot3 = null;
    public string strSpot4 = null;

    private Text[] aAnsanSpotsDistance = new Text[4];
    private double[] aDistance = new double[4];
    
    private float latitude;
    private float longitude;
    private int iCount = 0;
    private string gpsText = "";    
    private int iStampCount = 0;
    private int iGPS_PosCount = 0;

    private double[] aSpotLatitude = new double[4];
    private double[] aSpotLongitude  = new double[4];
    private GameObject[] aStampImg = new GameObject[4];
    private GameObject[] aButtonImg = new GameObject[4];
    private GameObject[] aImageBundle = new GameObject[4];
    private int iTestCount = 0;
    private int iButtonIndex = -1;
    private int iArriveIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < aStampImg.Length; ++i)
        {
            aStampImg[i] = mainCanvas.transform.GetChild(i + 1).gameObject.transform.GetChild(0).gameObject;
            aButtonImg[i] = mainCanvas.transform.GetChild(i + 1).gameObject.transform.GetChild(1).gameObject;
            aAnsanSpotsDistance[i] = mainCanvas.transform.GetChild(i + 1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
            aImageBundle[i] = ImageBundle.transform.GetChild(i).gameObject;
            aImageBundle[i].SetActive(false);
        }

        //모바일 location 정보에 대한 권한 설정...
        if (Application.platform == RuntimePlatform.Android)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }            
        }
        ReStartGPS();
        //RequestGPS_Pos();
        //StampStatusUpdate();
    }

    // Update is called once per frame
    void Update()    {      }

    //================================================= GPS 정보 받기(경도, 위도) ==================================================//
    // GPS 정보 받기...    
    public void RequestGPS_Pos()
    {
        string structure = "";
        if (0 == iGPS_PosCount)
        {
            structure = strSpot1;
        }
        else if (1 == iGPS_PosCount)
        {
            structure = strSpot2;
        }
        else if (2 == iGPS_PosCount)
        {
            structure = strSpot3;
        }
        else if (3 == iGPS_PosCount)
        {
            structure = strSpot4;
        }
        else if (4 == iGPS_PosCount)
        {            
            StampCheck();            
            return;
        }

        //Debug.Log("check");
        DatabaseManager.Instance.GetGPS_Info(structure);
    }
    public void GetGPS_Pos(string _strAnswer)
    {
        //Debug.Log("answer: " + _strAnswer);
        string strAnswer = _strAnswer;
        if (strAnswer != "")
        {
            int location = strAnswer.IndexOf(",");
            int length = strAnswer.Length;
            int minusLength = length - location;
            string strLatitude = strAnswer.Substring(0, location);
            string strLongitute = strAnswer.Substring(location + 1, minusLength - 1);
            aSpotLatitude[iGPS_PosCount] = double.Parse(strLatitude);
            aSpotLongitude[iGPS_PosCount] = double.Parse(strLongitute);            
            
            // 계속 반복한다.
            Debug.Log("lati: " + aSpotLatitude[iGPS_PosCount]);
            Debug.Log("long: " + aSpotLongitude[iGPS_PosCount]);

            iGPS_PosCount++;
            RequestGPS_Pos();
        }
        else
        {
            // 에러....
        }
    }

    //====================================================================================================================//
    // 스탬프 획득 여부...
    void StampCheck()
    {
        //string appID = PlayerInfo.Instance.GetAppID();
        string appID = AppID;
        string userID = PlayerInfo.Instance.GetUserID();
        //string kind = "social_sticker_first";
        string kind = "";
        string get_flag = "";
        string edu_type = strEdu_type + "_live";
        string game_type = "AR";
        
        if (0 == iStampCount)
        {
            kind = strEdu_type + "_sticker_first";
            DatabaseManager.Instance.GetStamp(appID, userID, kind, edu_type);
        }
        else if(1 == iStampCount)
        {
            kind = strEdu_type + "_sticker_second";
            DatabaseManager.Instance.GetStamp(appID, userID, kind, edu_type);
        }
        else if (2 == iStampCount)
        {
            kind = strEdu_type + "_sticker_third";
            DatabaseManager.Instance.GetStamp(appID, userID, kind, edu_type);
        }
        else if (3 == iStampCount)
        {
            kind = strEdu_type + "_sticker_fourth";
            DatabaseManager.Instance.GetStamp(appID, userID, kind, edu_type);
        }
        else if (4 == iStampCount)
        {
            StartCoroutine(GPS_KeepUpdate());
            return;
        }        
    }



    public void CatchStampInfo(string _strAnswer)
    {
        Debug.Log("answer: " + _strAnswer);
        string strAnswer = _strAnswer;
        if ("Y" == strAnswer)
        {
            // 이미 획득한 Stamp..            
            aStampImg[iStampCount].SetActive(true);
            aButtonImg[iStampCount].SetActive(false);
        }
        else
        {            
            // 획득 안한 Stamp...
            aStampImg[iStampCount].SetActive(false);
            aButtonImg[iStampCount].SetActive(true);
        }        
        iStampCount++;
        StampCheck();
    }
    






    //======================================================= Stamp 획득 ======================================================//  
    // 획득시가 아닌,,,, 
    public void StampStatusUpdate()
    {
        //string appID = PlayerInfo.Instance.GetAppID();
        string appID = AppID;
        string userID = PlayerInfo.Instance.GetUserID();
        //string kind = "social_sticker_first";
        string kind = "";
        string get_flag = "";                
        string edu_type = strEdu_type + "_live";
        string game_type = "AR";
        int timeInfo = 0;

        if (0 == iStampCount)
        {
            // 수정 예정...
            get_flag = "Y";
            kind = strEdu_type + "_sticker_first";
            DatabaseManager.Instance.GpsInfoUpdate(appID, userID, kind, get_flag, edu_type, game_type, timeInfo);
        }
        else if (1 == iStampCount)
        {
            get_flag = "Y";
            kind = strEdu_type + "_sticker_second";
            DatabaseManager.Instance.GpsInfoUpdate(appID, userID, kind, get_flag, edu_type, game_type, timeInfo);
        }
        else if (2 == iStampCount)
        {
            get_flag = "Y";
            kind = strEdu_type + "_sticker_third";
            DatabaseManager.Instance.GpsInfoUpdate(appID, userID, kind, get_flag, edu_type, game_type, timeInfo);
        }
        else if (3 == iStampCount)
        {
            get_flag = "Y";
            kind = strEdu_type + "_sticker_fourth";
            DatabaseManager.Instance.GpsInfoUpdate(appID, userID, kind, get_flag, edu_type, game_type, timeInfo);
        }
        else if (4 == iStampCount)
        {
            ReStartGPS();
        }        
    }

    //==================================================== GPS INITIALIZE =====================================================//   
    void ReStartGPS()
    {

#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        PcFuncStart();
#elif UNITY_ANDROID
        StartCoroutine(GpsStart());        
#endif
        //StartCoroutine(GpsStart());
    }

    // 안드로이드 용..
    IEnumerator GpsStart()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            gpsText = "User has not enabled GPS";            
            describeText.text = gpsText;
            Invoke("ReStartGPS", 4f);
            yield break;
        }
        Input.location.Start(5, 10);
        int maxWait = 20;

        gpsText = Input.location.status.ToString();
        describeText.text = gpsText;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            //Input.location.status
            gpsText = Input.location.status.ToString();
            describeText.text = gpsText;
            maxWait--;
        }

        RequestGPS_Pos();

        while (true)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;            
            iCount++;            
            yield return new WaitForSeconds(0.5f);
        }
    }

    // pc용...
    void PcFuncStart()
    {        
        //latitude = (float)37.274092;
        //longitude = (float)126.839966;
        latitude = (float)37.484222;
        longitude = (float)126.899064;
        RequestGPS_Pos();
    }


    // 아래는 삭제 예정...
    // 아래는 삭제 예정...
    
    //================================================= GPS ===================================================//    

    IEnumerator GPS_KeepUpdate()
    {
        int iTmpCount = 0;

        while(true)
        {
            if (4 == iTmpCount)
                iTmpCount = 0;

            //Debug.Log("lati " + latitude);
            //Debug.Log("long " + longitude);

            if (false == aStampImg[iTmpCount].activeSelf)
            {                                
                // 모바일용...
                aDistance[iTmpCount] = GetDistance(latitude, longitude, aSpotLatitude[iTmpCount], aSpotLongitude[iTmpCount]);

                if (1000 <= aDistance[iTmpCount])                                                                            // 1000 미터 이상, 즉 kilo 로 표시...
                {
                    double dDist22 = aDistance[iTmpCount] / 1000;
                    string strDistance = dDist22.ToString("N1");
                    strDistance += "kilo";
                    aAnsanSpotsDistance[iTmpCount].text = strDistance;
                }
                else
                {
                    if (50 > aDistance[iTmpCount])                                                                              // 100 미터 이하일 경우 도착으로 표시...
                    {
                        // arrived...                
                        aAnsanSpotsDistance[iTmpCount].text = "";
                        // effect 가 있어야 할지도 있음....
                        aStampImg[iTmpCount].SetActive(true);
                        aButtonImg[iTmpCount].SetActive(false);

                        RequestCheckArrive(iTmpCount, latitude, longitude);
                    }
                    else
                    {
                        string strDistance = aDistance[iTmpCount].ToString("N0");
                        strDistance += "m";
                        aAnsanSpotsDistance[iTmpCount].text = strDistance;
                    }
                }
            }
            
            if (iButtonIndex == iTmpCount)
            {
                if (false == aStampImg[iTmpCount].activeSelf)
                {
                    describeText.text = aAnsanSpotsDistance[iTmpCount].text;
                }                
            }
                

            iTmpCount++;            
            yield return new WaitForSeconds(0.5f);
        }        
    }

    // 유저 id, 현재 gps, appID, 장소...
    void RequestCheckArrive(int _iTmeCount, double _latitude, double _longitute)
    {
        int iTmpCount = _iTmeCount;
        iArriveIndex = _iTmeCount;
        string dCurrentLatitude = _latitude.ToString();
        string dCurrentLongitude = _longitute.ToString();
        string dUserGPS = dCurrentLatitude + "," + dCurrentLongitude;

        //string appID = PlayerInfo.Instance.GetAppID();
        string appID = AppID;
        string userID = PlayerInfo.Instance.GetUserID();

        string structure = "";
        if (0 == iTmpCount)
            structure = strSpot1;
        else if (1 == iTmpCount)
            structure = strSpot2;
        else if (2 == iTmpCount)
            structure = strSpot3;
        else if (3 == iTmpCount)
            structure = strSpot4;
        else if (4 <= iTmpCount)
            return;

        DatabaseManager.Instance.GPS_ArriveUpdate(appID, userID, structure, dUserGPS);
    }

    //DatabaseManager.Instance.GpsInfoUpdate(appID, userID, kind, get_flag, edu_type, game_type, timeInfo);
    public void StampStickerUpdate()
    {
        //string appID = PlayerInfo.Instance.GetAppID();
        string appID = AppID;
        string userID = PlayerInfo.Instance.GetUserID();
        //string kind = "social_sticker_first";
        string kind = "";
        string get_flag = "";
        string edu_type = strEdu_type + "_live";
        string game_type = "AR";
        int timeInfo = 0;
        
        if (0 == iArriveIndex)
            kind = strEdu_type + "_sticker_first";
        else if (1 == iArriveIndex)
            kind = strEdu_type + "_sticker_second";
        else if (2 == iArriveIndex)
            kind = strEdu_type + "_sticker_third";
        else if (3 == iArriveIndex)
            kind = strEdu_type + "_sticker_fourth";
        else if (4 == iArriveIndex)
            return;

        get_flag = "Y";        
        // test 1 update...        
        DatabaseManager.Instance.GpsInfoUpdate(appID, userID, kind, get_flag, edu_type, game_type, timeInfo);
    }

    public void CatchGpsUpdate(string _strAnswer)
    {
        Debug.Log("state: " + _strAnswer);
    }

    //==========================================================================================================//
    //================================================= BUTTON ===================================================//
    //========================================================================================================//
    public void SpotButtonEvent1()
    {
        for(int i = 0; i < aImageBundle.Length; ++i)
        {
            aImageBundle[i].SetActive(false);
        }
        aImageBundle[0].SetActive(true);

        if (false == aStampImg[0].activeSelf)
        {
            describeText.text = aAnsanSpotsDistance[0].text;
        }
        iButtonIndex = 0;
    }
    public void SpotButtonEvent2()
    {
        for (int i = 0; i < aImageBundle.Length; ++i)
        {
            aImageBundle[i].SetActive(false);
        }
        aImageBundle[1].SetActive(true);

        if (false == aStampImg[1].activeSelf)
        {
            describeText.text = aAnsanSpotsDistance[1].text;
        }
        iButtonIndex = 1;
    }
    public void SpotButtonEvent3()
    {
        for (int i = 0; i < aImageBundle.Length; ++i)
        {
            aImageBundle[i].SetActive(false);
        }
        aImageBundle[2].SetActive(true);

        if (false == aStampImg[2].activeSelf)
        {
            describeText.text = aAnsanSpotsDistance[2].text;
        }
        iButtonIndex = 2;
    }
    public void SpotButtonEvent4()
    {
        for (int i = 0; i < aImageBundle.Length; ++i)
        {
            aImageBundle[i].SetActive(false);
        }
        aImageBundle[3].SetActive(true);

        if (false == aStampImg[3].activeSelf)
        {
            describeText.text = aAnsanSpotsDistance[3].text;
        }
        iButtonIndex = 3;
    }
    //========================================================================================================================//
    public void StampButtonEvent1()
    {
        for (int i = 0; i < aImageBundle.Length; ++i)
        {
            aImageBundle[i].SetActive(false);
        }
        aImageBundle[0].SetActive(true);
        iButtonIndex = 0;

        describeText.text = "소양강댐 물 문화관은 소양강 댐 정상에서 청평사로 이동할 수 있는 뱃나루에 위치하며, 물에 대한 이해를 돕는 자료가 있다. 물의 힘을 이용한 수력발전이나, 각 나라별 물에 대해 갖는 이미지, 물의 소중함을 소개해주는 코너가 준비되어 있다. 근방에 소양강 처녀 동상이 있어, 소양강 처녀 노래 가삿말에 등장하는 ‘노란 동백꽃’을 확인할 수 있다. *소양강댐 물 문화관 블로그 주소: https://blog.naver.com/kwatersyk";
    }
    public void StampButtonEvent2()
    {
        for (int i = 0; i < aImageBundle.Length; ++i)
        {
            aImageBundle[i].SetActive(false);
        }
        aImageBundle[1].SetActive(true);
        iButtonIndex = 1;

        describeText.text = "김유정역에서 출발하는 레일바이크는, 추억과 낭만이 있는 강촌까지의 옛 기차구간을 이용한 레일바이크를 통해 즐길 수 있다. 서정적이면서 톡톡 튀는 분위기를 연출하는 김유정역 북월 광장과, 각기각색의 테마가 있는 터널, 그리고 아름다운 북한강 절경을 즐길 수 있는 국내 최대 규모의 레일바이크 코스이다. * 예약 및 관련 안내: http://www.railpark.co.kr/rail/rail1_01.php";
    }
    public void StampButtonEvent3()
    {
        for (int i = 0; i < aImageBundle.Length; ++i)
        {
            aImageBundle[i].SetActive(false);
        }
        aImageBundle[2].SetActive(true);
        iButtonIndex = 2;

        describeText.text = "금병산에 둘러싸인 마을의 모습이 ‘떡시루’의 모양과 같다고 해서 지어진 ‘실레(증리)’는 김유정의 고향이자, 마을 곳곳에 김유정의 소설 에 등장하는 인물들과 관련된 곳이다. 이를 바탕으로 만들어진 금병산 자락의 실레 이야기길은 < 금병산 아기장수 전설길>, < 점순이가 ‘나’를 꼬시던 동백숲길 >, < 덕돌이가 장가가던 신바람길>, < 장인 입에서 할아버지 소리 나오던 데릴사위길 > < 김유정이 코다리찌개 먹던 주막길 > 등 재미난 이야기 열여섯 마당과 만날 수 있도록 1시간에서 1시간 30분 정도의 코스로 짜여 있다.";
     }
    public void StampButtonEvent4()
    {
        for (int i = 0; i < aImageBundle.Length; ++i)
        {
            aImageBundle[i].SetActive(false);
        }
        aImageBundle[3].SetActive(true);
        iButtonIndex = 3;

        describeText.text = "성호기념관은 성호 이익(1681~1763)의 생애를 기리고 그 학문적 업적을 계승, 발전시키고자 안산시가 건립한 기념관이다. 성호기념관에는 성호 이익 선생의 학문 및 실학사상을 소개하는 상설전시실과 어린이의 눈높이에 맞춘 체험전시실, 선생의 일대기를 상영하는 영상관 및 실학관련 주요 정보를 얻을 수 있는 실학정보실, 그리고 교육 공간인 성호학당 등으로 이루어져 있다. 성호기념관은 다양한 교육프로그램을 마련하여 시민들의 문화사랑방으로 이용되고 있으며, 실학의 고장 안산을 알리고 경기실학을 대표하는 곳으로 자리잡고 있다.";        
        /*
        if ("art" == strEdu_type)
            describeText.text = "성호기념관은 성호 이익(1681~1763)의 생애를 기리고 그 학문적 업적을 계승, 발전시키고자 안산시가 건립한 기념관이다. 성호기념관에는 성호 이익 선생의 학문 및 실학사상을 소개하는 상설전시실과 어린이의 눈높이에 맞춘 체험전시실, 선생의 일대기를 상영하는 영상관 및 실학관련 주요 정보를 얻을 수 있는 실학정보실, 그리고 교육 공간인 성호학당 등으로 이루어져 있다. 성호기념관은 다양한 교육프로그램을 마련하여 시민들의 문화사랑방으로 이용되고 있으며, 실학의 고장 안산을 알리고 경기실학을 대표하는 곳으로 자리잡고 있다.";
        else if ("sci" == strEdu_type)
            describeText.text = "국제습지조약 보존습지로 지정되어 국제적인 습지가 된 우포늪은 우포늪(1.3㎢), 목포늪(53만㎡), 사지포(36만㎡), 쪽지벌(14만㎡) 4개 늪으로 이루어져 있다. 1997년 342종의 동·식물이 이곳에 서식하는 것으로 조사·보고되었다. 식물은 가시연꽃·생이가래·부들·줄·골풀·창포·마름·자라풀 등 168종, 조류는 쇠물닭·논병아리·노랑부리저어새(천연기념물 205)·청둥오리·큰고니(천연기념물 201) 등 62종, 어류는 뱀장어·붕어·잉어·가물치·피라미 등 28종, 수서곤충은 연못하루살이·왕잠자리·장구애비·소금쟁이 등 55종, 패각류는 우렁이·물달팽이·말조개 등 5종, 포유류는 두더지·족제비· 너구리 등 12종, 파충류는 남생이·자라·줄장지뱀·유혈목이 등 7종, 양서류는 무당개구리·두꺼비·청개구리·참개구리·황소개구리 등 5종이 서식하고 있다. [네이버 지식백과] 우포늪[牛浦─](두산백과)";
        */
    }
    public void BackButtonEvent()
    {
        SceneManager.LoadScene("SelectMap");
    }
    public void TestButtonEvent()
    {
        if (4 <= iTestCount)
            return;
        aStampImg[iTestCount].SetActive(true);
        aButtonImg[iTestCount].SetActive(false);
        iTestCount++;
    }

    public void MapButtonEvent()
    {
        //https://www.google.com/maps/dir/37.483782,126.9003409/37.5081441,126.8385434        

        //iButtonIndex
        //aSpotLatitude , aSpotLongitude        
        if (-1 == iButtonIndex)
            return;

        //string strUrl = "https://www.google.com/maps/dir/" + latitude + "," + longitude + "/";
        //string strUrl = "https://www.google.com/maps/dir/" + latitude + "," + longitude + "/" + 37.94849 + "," + 127.8147;
        string strUrl = "https://www.google.com/maps/dir/" + latitude + "," + longitude + "/" + aSpotLatitude[iButtonIndex] + "," + aSpotLongitude[iButtonIndex];
        Application.OpenURL(strUrl);
    }


    public void TestButtonEvent2()
    {
        //37.66771 128.7053
        latitude = (float)37.66771;
        longitude = (float)128.7053;
    }


    // 지도 열기...
    // 아래와 같은 양식으로 한다.
    //https://www.google.com/maps/dir/37.483782,126.9003409/37.5081441,126.8385434



    //=================================================================================================================//
    //===================================================== EXTRA FUNCTION ===============================================//
    //=================================================================================================================//    
    // 거리 계산 Function...
    double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double theta = lon1 - lon2;
        double dLat1 = deg2rad(lat1);
        double dLat2 = deg2rad(lat2);
        double dTheta = deg2rad(theta);

        double dist = Math.Sin(dLat1) * Math.Sin(dLat2) + Math.Cos(dLat1) * Math.Cos(dLat2) * Math.Cos(dTheta);
        dist = Math.Acos(dist);
        double dDistResult = rad2deg(dist);

        dDistResult = dDistResult * 60 * 1.1515;        
        dDistResult = dDistResult * 1.6093344;
        dDistResult = dDistResult * 1000.0;
        return dDistResult;
    }
    // 방향 각도 구하기....
    public short BearingP1toP2(double P1_latitude, double P1_longitude, double P2_latitude, double P2_longitude)
    {
        // 현재 위치 : 위도나 경도는 지구 중심을 기반으로 하는 각도이기 때문에 라디안 각도로 변환한다.
        double cur_Lat_radian = P1_latitude * (3.141592 / 180);
        double cur_Lon_radian = P1_longitude * (3.141592 / 180);
        // 목표 위치 : 위도나 경도는 지구 중심을 기반으로 하는 각도이기 때문에 라디안 각도로 변환한다.
        double Dest_Lat_radian = P2_latitude * (3.141592 / 180);
        double Dest_Lon_radian = P2_longitude * (3.141592 / 180);
        
        // radian distance               
        //radian_distance = Mathf.Acos(Mathf.Sin(DoubleToFloat(cur_Lat_radian)) * Mathf.Sin(DoubleToFloat(Dest_Lat_radian)) + Mathf.Cos(DoubleToFloat(cur_Lat_radian)) * Mathf.Cos(DoubleToFloat(Dest_Lat_radian)) * Mathf.Cos(DoubleToFloat(cur_Lon_radian - Dest_Lon_radian)));
        double radian_distance = Math.Acos(Math.Sin(cur_Lat_radian)) * Math.Sin(Dest_Lat_radian) + Math.Cos(cur_Lat_radian) * Math.Cos(Dest_Lat_radian) * Math.Cos(cur_Lon_radian - Dest_Lon_radian);

        // 목적지 이동 방향을 구한다.(현재 좌표에서 다음 좌표로 이동하기 위해서는 방향을 설정해야 한다. 라디안값이다.
        //double radian_bearing = Mathf.Acos((Mathf.Sin(DoubleToFloat(Dest_Lat_radian)) - Mathf.Sin(DoubleToFloat(cur_Lat_radian)) * Mathf.Cos(DoubleToFloat(radian_distance))) / (Mathf.Cos(DoubleToFloat(cur_Lat_radian)) * Mathf.Sin(DoubleToFloat(radian_distance))));
        double radian_bearing = Math.Acos(Math.Sin(Dest_Lat_radian)) - Math.Sin(cur_Lat_radian) * Math.Cos(radian_distance) / Math.Cos(cur_Lat_radian) * Math.Sin(radian_distance);

        // acos의 인수로 주어지는 x는 360분법의 각도가 아닌 radian(호도)값이다.       
        double true_bearing = 0;
        if (Math.Sin(Dest_Lon_radian - cur_Lon_radian) < 0)
        {
            true_bearing = radian_bearing * (180 / 3.141592);
            true_bearing = 360 - true_bearing;
        }
        else
        {
            true_bearing = radian_bearing * (180 / 3.141592);
        }
        return (short)true_bearing;
    }
    static double deg2rad(double _deg)
    {
        return (_deg * Mathf.PI / 180d);
    }
    static double rad2deg(double _rad)
    {
        return (_rad * 180d / Mathf.PI);
    }


    //===================================================== USELESS ===============================================//


    void AnsanCalculateDistance(int _iCount)
    {
        int iTempCount = _iCount;

        // test 용...        
        //double dMyLatitude = 37.274092;
        //double dMyLogitude = 126.839966;
        //pc 테스트용...        
        //aDistance[iTempCount] = GetDistance(dMyLatitude, dMyLogitude, aSpotLatitude[iTempCount], aSpotLongitude[iTempCount]);
        // 모바일용...
        aDistance[iTempCount] = GetDistance(latitude, longitude, aSpotLatitude[iTempCount], aSpotLongitude[iTempCount]);

        if (1000 <= aDistance[iTempCount])                                                                            // 1000 미터 이상, 즉 kilo 로 표시...
        {
            if (true == aStampImg[iTempCount].activeSelf)
                return;

            double dDist22 = aDistance[iTempCount] / 1000;
            string strDistance = dDist22.ToString("N1");
            strDistance += "kilo";

            aAnsanSpotsDistance[iTempCount].text = strDistance;
        }
        else
        {
            if (true == aStampImg[iTempCount].activeSelf)
                return;

            if (50 > aDistance[iTempCount])                                                                              // 100 미터 이하일 경우 도착으로 표시...
            {
                // arrived...                
                aAnsanSpotsDistance[iTempCount].text = "";
                //aAnsanSpotsStampBg[iTempCount].SetActive(true);
                //aAnsanSpotsStamp[iTempCount].SetActive(true);

                // 그리고 db 에 send 한다....
                // 도착...
                //RequestCheckArrive(iTempCount, dMyLatitude, dMyLogitude);
                RequestCheckArrive(iTempCount, latitude, longitude);
            }
            else
            {
                string strDistance = aDistance[iTempCount].ToString("N0");
                strDistance += "m";
                aAnsanSpotsDistance[iTempCount].text = strDistance;
            }
        }
    }


    // 안산시 버튼을 클리시에....
    public void AnsanButtonEvent()
    {
        // 총 4개를 받아온다.
        // 여기서 시작해서,,,
        // 아래처럼.....

        //string appID = PlayerInfo.Instance.GetAppID();
        string appID = AppID;
        string userID = PlayerInfo.Instance.GetUserID();
        //string kind = "social_sticker_first";
        string kind = "";
        string get_flag = "";
        string edu_type = "social_live";
        string game_type = "AR";
        int timeInfo = 0;

        /*
        if (0 == iStampCount)
            kind = "social_sticker_first";
        else if (1 == iStampCount)
            kind = "social_sticker_second";
        else if (2 == iStampCount)
            kind = "social_sticker_third";
        else if (3 == iStampCount)
            kind = "social_sticker_fourth";
        else if (4 == iStampCount)
            return;
        */

        get_flag = "Y";
        //kind = "social_sticker_first";
        kind = "social_sticker_second";


        Debug.Log("app ID: " + appID);
        Debug.Log("user ID: " + userID);

        // 여기서 먼저 체크를 해야 한다.
        // first ~ fourth 까지 각각 받아온다.
        // 총 4개의 코루틴 함수를 받아온다.

        // db 에서 stamp 결과 받아오기....
        // 성공...
        //DatabaseManager.Instance.GetStamp(appID, userID, kind, edu_type);

        // test 1 update...
        // 성공...
        DatabaseManager.Instance.GpsInfoUpdate(appID, userID, kind, get_flag, edu_type, game_type, timeInfo);

        /*
        CityButtons.SetActive(false);        
        Buyeo.SetActive(false);
        Chang_neong.SetActive(false);
        Chun_chen.SetActive(false);
        Ansan.SetActive(true);
        BackButton.SetActive(true); 
        
        eCity = GPS_CITY.ANSAN;
        */
    }

    /*
    public void CatchGpsUpdate(string _strAnswer)
    {
        Debug.Log("answer: " + _strAnswer);
        string strAnswer = _strAnswer;
        if ("Y" == strAnswer)
        {
            // 이미 획득한 Stamp..
            Debug.Log("already have");
            aStampImg[iStampCount].SetActive(true);
            aButtonImg[iStampCount].SetActive(false);
        }
        else
        {
            // 획득 안한 Stamp...
            aStampImg[iStampCount].SetActive(false);
            aButtonImg[iStampCount].SetActive(true);
        }

        // 여기서 call....
        iStampCount++;
        Invoke("StampStatusUpdate", 0.2f);
    }
    */
}
