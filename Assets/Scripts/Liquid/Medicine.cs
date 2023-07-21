using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Medicine : MonoBehaviour, ILiquid
{
    [SerializeField] private Transform plug;
    [SerializeField] private Transform bottlePos, bottleExitPos;
    [SerializeField] private ParticleSystem liquidParticle;

    [SerializeField] private Material matBwol;
    [SerializeField] private Material matBottle;
    [SerializeField] private ParticleSystem aura;
    private ParticleSystem ps;
    public void BottleAnimation()
    {
        this.GetComponent<ObjectFollowTouch>().enabled = false;
        this.transform.DOMove(bottlePos.position, .3f);
        plug.DOLocalMove(plug.position + new Vector3(0, 10, 0), 0.3f).SetEase(Ease.Linear).SetDelay(0.3f).OnComplete(() =>
        {
            plug.gameObject.SetActive(false);
            StartCoroutine( PlayStopParticle(true, 0.6f));

            this.transform.DOLocalRotate(new Vector3(0, 0, 74), 1);
            StartCoroutine(PlayStopParticle(false, 1.8f));
            this.transform.DOMove(bottleExitPos.position, 2f).SetDelay(2.3f);
        });
    }
    IEnumerator PlayStopParticle(bool play, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (play)
        {
            liquidParticle.Play();
            StartPouring();
        }
        else
        {
            liquidParticle.Stop();
            ps.Stop();
        }
    }

    private void StartPouring()
    {
        StartCoroutine(Pour());
        StartCoroutine(Aura());
    }

    IEnumerator Pour()
    {
        yield return new WaitForSeconds(0.1f);
        float matVal = -0.03f;
        float matValBottle = 0.03f;
        while (matBwol.GetFloat("_Fill") < 0.04f)
        {
            matVal += 0.001f;
            matValBottle -= 0.00075f;
            
            matBwol.SetFloat("_Fill", matVal);
            matBottle.SetFloat("_Fill", matValBottle);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Aura()
    {
        yield return new WaitForSeconds(0.5f);
        ps = Instantiate(aura, aura.transform.position, aura.transform.localRotation);
        ps.Play();
    }
}
