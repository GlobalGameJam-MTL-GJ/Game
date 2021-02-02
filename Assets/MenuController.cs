using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _text1;
    [SerializeField] private GameObject _text2;
    [SerializeField] private GameObject _text3;
    [SerializeField] private GameObject _text4;
    [SerializeField] private GameObject _text5;
    [SerializeField] private GameObject _text6;

    // Start is called before the first frame update
    void Start()
    {
        var seq = LeanTween.sequence();
        seq.append(LeanTween.scale(_text1, new Vector3(1, 1, 1), 1f).setEaseInOutElastic());
        seq.append(LeanTween.scale(_text2, new Vector3(1, 1, 1), 1f).setEaseInOutElastic());
        seq.append(LeanTween.scale(_text3, new Vector3(1, 1, 1), 1f).setEaseInOutElastic());
        seq.append(LeanTween.scale(_text4, new Vector3(1, 1, 1), 1f).setEaseInOutElastic());
        seq.append(LeanTween.scale(_text5, new Vector3(1, 1, 1), 1f).setEaseInOutElastic());
        seq.append(LeanTween.scale(_text6, new Vector3(1, 1, 1), 1f).setEaseInOutElastic());
    }
}
