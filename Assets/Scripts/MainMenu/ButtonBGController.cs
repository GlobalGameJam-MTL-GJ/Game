using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBGController : MonoBehaviour
{
    [SerializeField] private Image _buttonBG;

    public void OnButtonEnter()
    {
        _buttonBG.enabled = true;
    }

    public void OnButtonExit()
    {
        _buttonBG.enabled = false;
    }
}
