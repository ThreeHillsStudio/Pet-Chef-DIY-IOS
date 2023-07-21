using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class Items : MonoBehaviour
{
    [SerializeField] private Material matBowl;
    [SerializeField] private Material matBottle;
    [SerializeField] private Material catMat;

    [SerializeField] private ParticleSystem heartParticle;

    [SerializeField] private ParticleSystem wingLeft;
    [SerializeField] private ParticleSystem wingright;
    [SerializeField] private ParticleSystem hornParticle;

    [SerializeField] private ParticleSystem magicParticle;

    [SerializeField] private CinemachineVirtualCamera lastCam;
    [SerializeField] private Animator animatorCat;
    [SerializeField] private GameObject winUI;
 
    private void Start()
    {
        SetMaterialVal();
    }

    void SetMaterialVal()
    {
        matBowl.SetFloat("_Fill", -0.1f);
        matBottle.SetFloat("_Fill", 0.1f);
        catMat.SetFloat("_amount", 0);
    }

    public void ShowItem(GameObject item)
    {
        item.SetActive(true);
        wingLeft.Play();
        wingright.Play();
    }
    
    public void ShowItem(Transform itemToShow, Transform itemFinalPosition)
    {
        itemToShow.gameObject.SetActive(true);

        itemToShow.DOMove(itemFinalPosition.position, 1).SetEase(Ease.OutSine);
        itemToShow.DOScale(itemFinalPosition.localScale, 1).SetEase(Ease.OutSine);

        StartCoroutine(HornParticle());
    }

    private IEnumerator HornParticle()
    {
        yield return new WaitForSeconds(1);
        hornParticle.Play();
    }

    public void ChangeColor()
    {
        StartCoroutine(ColorTransition());
    }

    IEnumerator ColorTransition()
    {
        float amount = catMat.GetFloat("_amount");

        while (amount < 1)
        {
            amount += 0.1f;
            catMat.SetFloat("_amount", amount);
            yield return new WaitForSeconds(0.1f);
        }
        heartParticle.Play();
    }

    public void Magic()
    {
        magicParticle.Play();
        lastCam.Priority = 12;
        StartCoroutine(CatAnimationLeg());
    }

    IEnumerator CatAnimationLeg()
    {
        yield return new WaitForSeconds(2);
        animatorCat.SetBool("canRaiseLeg", true);

        yield return new WaitForSeconds(1.5f);
        winUI.SetActive(true);
    }
}
