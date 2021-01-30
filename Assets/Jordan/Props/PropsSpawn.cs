using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = System.Random;

public class PropsSpawn : MonoBehaviour
{
    [SerializeField] private Transform spawnTrajectoryHolder;
    [SerializeField] private Transform[] spawnThrowPoints;
    [Range(-180, 0)]
    [SerializeField] private int minYAngle = -45;
    [Range(0, 180)]
    [SerializeField] private int maxYAngle = 45;

    private GameObject spawnTracePrefab;
    private LTSpline ltSpline;
    private Vector3[] points = new Vector3[7];

    private void Awake()
    {
        spawnTracePrefab = Resources.Load<GameObject>("Prefabs/SpawnTrace");

    }

    public void SpawnTween(GameObject newProps)
    {
        RotateSpawnHolderAndAssignPointsPosition();
        LeanTween.delayedCall(0.25f, () =>
        {
            LeanTween.moveSpline(newProps, ltSpline, UnityEngine.Random.Range(0.65f, 1.30f)).setEaseOutBounce()
                .setOnComplete(() =>
                {
                    LeanTween.delayedCall(0.5f, () => newProps.GetComponent<Props>().ActivateMovement());
                });
            ;
        });
    }

    private void RotateSpawnHolderAndAssignPointsPosition()
    {
        bool validate = false;
        int attemptCount = 0;
        while (!validate && attemptCount < 50)
        {
            attemptCount++;
            float angle = UnityEngine.Random.Range(minYAngle, maxYAngle);
            spawnTrajectoryHolder.Rotate(Vector3.up, angle);
            Collider[] hits = Physics.OverlapSphere(spawnThrowPoints[spawnThrowPoints.Length - 1].position, 1.25f, 1 << 23, QueryTriggerInteraction.Collide);
            if (hits.Length != 0) continue;
            Instantiate(spawnTracePrefab, spawnThrowPoints[spawnThrowPoints.Length - 1].position,
                Quaternion.identity);
            validate = true;
        }
        
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(spawnThrowPoints[spawnThrowPoints.Length - 1].position, 0.25f);
    }
}
