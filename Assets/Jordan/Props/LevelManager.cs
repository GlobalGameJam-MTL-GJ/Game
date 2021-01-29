using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private Renderer propsBoundsRenderer;
    [SerializeField] private LevelConfigSO levelConfigSO;
    [SerializeField] private GameObject tempPlayer;
    private Bounds propsBounds;
    public Bounds PropsBounds => propsBounds;

    public LevelConfigSO LevelConfigSo => levelConfigSO;
    public GameObject TempPlayer => tempPlayer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        propsBounds = propsBoundsRenderer.bounds;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
