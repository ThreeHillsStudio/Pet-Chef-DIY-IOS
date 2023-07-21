using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Poison : MonoBehaviour, ILiquid
{
    [SerializeField] private Transform bottlePos, bottleExitPos;

    [SerializeField] private ParticleSystem liquidParticle;

    public void BottleAnimation()
    {
        this.GetComponent<ObjectFollowTouch>().enabled = false;
        this.transform.DOMove(bottlePos.position, .3f);
        StartCoroutine( PlayStopParticle(true, 0.5f));
        this.transform.DOLocalRotate(new Vector3(18, -98, 220), 1).SetDelay(.3f)
            .OnComplete(() =>
            {
                this.transform.DOMove(bottleExitPos.position, 2f).SetDelay(0.7f);
                StartCoroutine(PlayStopParticle(false, 0.5f));
            });
    }

    IEnumerator PlayStopParticle(bool play, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (play)
        {
            liquidParticle.Play();
        }
        else
        {
            liquidParticle.Stop();
        }
    }
}