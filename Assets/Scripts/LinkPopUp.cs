using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPopUp : MonoBehaviour
{

    public string UrlAddress = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LinkButtonEvent()
    {
        Application.OpenURL(UrlAddress);
    }
}
