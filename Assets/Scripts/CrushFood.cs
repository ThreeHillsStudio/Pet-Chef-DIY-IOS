using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using DG.Tweening;

public class CrushFood : MonoBehaviour
{

    [SerializeField] private Transform foodBreaker;

    [SerializeField] private Transform pos1, pos2;

    private Vector3 startPos;

    [Header("Time for moving and rotation:")]
    [SerializeField] private float timeToMoveFromAndToStartPos = 0.5f;
    [SerializeField] private float timeToRotateZ = 0.2f;
    [SerializeField] private float timeToRotateY = 0.4f;
    [SerializeField] private float timeForDoubleHit = 0.2f;

    [Header("Loop time:")]
    [SerializeField] private int rotateYLoopRestart = 6;
    [SerializeField] private int hitsLoopYoyo = 4;

    [Header("Vectors for rotations:")]
    [SerializeField] Vector3 rotationZ = new Vector3(0, 0, 20);
    [SerializeField] Vector3 rotationY = new Vector3(0, 360, 20);
    [SerializeField] private Vector3 rotationZero;

    [SerializeField] private ParticleSystem mixingParticle;
    private void Start()
    {
        startPos = foodBreaker.position;
        //Rotate();
    }
    
    public void Rotate()
    {
        StartCoroutine( ActiveDeactiveParticle(true));
        foodBreaker.DOMove(pos1.position, timeToMoveFromAndToStartPos).SetEase(Ease.Linear);
        foodBreaker.DORotate(rotationZ, timeToRotateZ, RotateMode.FastBeyond360).SetDelay(timeToMoveFromAndToStartPos);
        foodBreaker.DORotate(rotationY, timeToRotateY, RotateMode.FastBeyond360)
            .SetLoops(rotateYLoopRestart, LoopType.Restart)
            .SetDelay(timeToMoveFromAndToStartPos + (timeToRotateZ * 0.5f))
            .SetRelative()
            .SetEase(Ease.Linear)
            .OnComplete(
            () =>
            {
               StartCoroutine( ActiveDeactiveParticle(false));
                DoubleHitAndBack();
            });
    }

    public void DoubleHitAndBack()
    {
        foodBreaker.DORotate(rotationZero, timeToRotateZ, RotateMode.FastBeyond360);
        foodBreaker.DOMove(pos2.position, timeForDoubleHit).SetLoops(hitsLoopYoyo, LoopType.Yoyo).OnComplete(() =>
        {
            foodBreaker.DOMove(startPos, timeToMoveFromAndToStartPos);
        });
    }

    public float GetTimeForAnimation()
    {
        return timeToMoveFromAndToStartPos + timeToRotateZ + timeToRotateY * rotateYLoopRestart +
               timeForDoubleHit * hitsLoopYoyo/2 + timeToMoveFromAndToStartPos;
    }

    IEnumerator ActiveDeactiveParticle(bool isActive)
    {
        if (isActive)
        {
            yield return new WaitForSeconds(timeToMoveFromAndToStartPos);
            mixingParticle.Play();
        }
        else
        {
            mixingParticle.Stop();
        }
    }
}
