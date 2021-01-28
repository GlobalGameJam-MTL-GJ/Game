using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class PropsSpawn : MonoBehaviour
{
    private GameObject propsPrefab;
    [SerializeField] private Transform spawnTrajectoryHolder;
    [SerializeField] private Transform[] spawnThrowPoints;
    [Range(-180, 0)]
    [SerializeField] private int minYAngle = -45;
    [Range(0, 180)]
    [SerializeField] private int maxYAngle = 45;

    private LTSpline ltSpline;
    private Vector3[] points = new Vector3[7];
    private void Awake()
    {
        propsPrefab = Resources.Load<GameObject>("Prefabs/Props");
    }

    // Start is called before the first frame update
    void Start()
    {
        RotateSpawnHolderAndAssignPointsPosition();

        GameObject newProps = Instantiate(propsPrefab, transform.position + new Vector3(0, 0.25f, 0), transform.rotation);
        LeanTween.delayedCall(1f, () =>
        {
            LeanTween.moveSpline(newProps, ltSpline, UnityEngine.Random.Range(0.65f, 1.30f)).setEaseOutBounce().setOnComplete(() =>
            {
                LeanTween.delayedCall(0.5f, () => newProps.GetComponent<Props>().ActivateMovement());
            });;
        });
    }

    private void RotateSpawnHolderAndAssignPointsPosition()
    {
        float angle = UnityEngine.Random.Range(minYAngle, maxYAngle);
        Debug.Log(angle);
        spawnTrajectoryHolder.Rotate(Vector3.up, angle);
        for (int i = 0; i < points.Length; i++)
        {
            if (i < 2)
            {
                points[i] = spawnThrowPoints[0].position;
            }
            else if (i < points.Length - 1)
            {
                points[i] = spawnThrowPoints[i - 1].position;
            }
            else
            {
                points[i] = spawnThrowPoints[i - 2].position;
            }
        }
        ltSpline = new LTSpline(points);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
