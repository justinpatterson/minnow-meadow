using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    bool _isOpen;
    public virtual void OpenPanel() 
    {
        _isOpen = true;
        gameObject.SetActive(_isOpen);
    }


    public virtual void ClosePanel() 
    {
        _isOpen = false;
        gameObject.SetActive(_isOpen);
    }
}
