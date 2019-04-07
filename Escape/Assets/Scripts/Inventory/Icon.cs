using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{

    [SerializeField] private Sprite attimage, defimage;
    [SerializeField] private Text level, value;
    [SerializeField] private Image iconimage;
    public int reference, lvlval, val = 0;
    public int status = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        setIcon();
    }


    public void setIcon()
    {
        if (status == 0)
        {
            iconimage.GetComponent<Image>().sprite = attimage;
        }
        else
        {
            iconimage.GetComponent<Image>().sprite = defimage;
        }

        level.GetComponent<Text>().text = lvlval + "";
        value.GetComponent<Text>().text = val + "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
