using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropTest : MonoBehaviour
{
    public GameObject dragImage;
    public BoxCollider2D coillider1;
    public StagePlay m_StagePlay;

    private bool bTouch = false;
    private bool bSettle = false;

    private Camera mainCamera;

    private GameObject target = null;
    private bool bDrag = false;
    private bool bEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_StagePlay = FindObjectOfType<StagePlay>();

    }

    // Update is called once per frame
    void Update()
    {
        if (true == bEnd)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            bool bCheck = Physics.Raycast(ray, out hitInfo, 150f);
            if (true == bCheck)
            {
                target = hitInfo.collider.gameObject;
                bDrag = true;
            }
        }
        if (true == bDrag && null != target)
        {
            target.transform.localPosition = new Vector3(Input.mousePosition.x - 1130f, Input.mousePosition.y - 720f, 0f);
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (false == PlayerInfo.Instance.isComplite || true == m_StagePlay.Narration.isPlaying)
            {
                if (null != target)
                {
                    target.transform.localPosition = new Vector3(-390f, 0f, 0f);
                    bDrag = false;
                    target = null;
                }                
                return;
            }

            bDrag = false;            
            if(280f < target.transform.localPosition.x && 620f > target.transform.localPosition.x && 200f > target.transform.localPosition.y && -200f < target.transform.localPosition.y)
            {
                // 안에 들어감....
                target.transform.localPosition = new Vector3(460f, 45f, 0f);                
                bEnd = true;
                Invoke("ToNextScene", 1f);
            }
            else
            {
                target.transform.localPosition = new Vector3(-390f, 0f, 0f);
            }
        }

        /*
        if(Input.touchCount > 0)
        {
            Debug.Log("event");
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("touch began");
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (true == bTouch)
                {
                    // hmm... // image
                    dragImage.transform.localPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.0f);
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                bTouch = false;                
                if (true == bSettle)
                {                    
                    dragImage.transform.localPosition = new Vector3(480f, 0f, 0f);
                }
                else if (false == bSettle)
                {                    
                    dragImage.transform.localPosition = new Vector3(-420, 0f, 0f);
                }
            }
        }
        */
    }
    void ToNextScene()
    {
        m_StagePlay.forwardDown();
    }
}
