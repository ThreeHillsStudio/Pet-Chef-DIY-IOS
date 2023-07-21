using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeUI : MonoBehaviour
{
    [FormerlySerializedAs("Canvas")]
    [Header("UI")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private List<RectTransform> contentsBtns;
    [SerializeField] private Button currentPressedButton;
    [SerializeField] private List<GameObject> UIBtns;
    private int indexOfUIBtns = 0;

    [Header("Items")]
    [SerializeField] private GameObject rog;
    [SerializeField] private GameObject rogEndPos;

    [Header("Scripts")]
    [SerializeField] private Items itemsScript;
    [SerializeField] private CrushFood crushFoodScript;
    [FormerlySerializedAs("itemsFromUI")] [SerializeField] private ItemsFromUI itemsFromUIScript;

    [Header("Cameras")] 
    [SerializeField] private CinemachineVirtualCamera bowlCam;
    [SerializeField] private CinemachineVirtualCamera startCam;
    [SerializeField] private CinemachineBrain brain;
    
    [Header("Animator")]
    [SerializeField] private Animator modelAnimator;

    [Header("Food")]
    [SerializeField] private Transform parent;

    private int checkMarkNumPressed = 0;
    private void Start()
    {
        StartCoroutine(ChangeCameras());
        StartCoroutine(ShowCanvas());
    }

    IEnumerator ShowCanvas()
    {
        yield return new WaitForSeconds(1.8f);
        canvas.SetActive(true);
    }

    public void ChangeButtons(GameObject buttonsToRemove, GameObject buttonsToShow)
    {
        buttonsToRemove.SetActive(false);
        buttonsToShow.SetActive(true);
    }

    public void CheckMark()
    {
        checkMarkNumPressed++;
        canvas.SetActive(false);
        EatingCam();
    }

    void ContentIndex()
    {
        if (indexOfUIBtns < UIBtns.Count - 1)
        {
            ChangeButtons(UIBtns[indexOfUIBtns], UIBtns[indexOfUIBtns+1]);
            ChangeScrollContent(contentsBtns[indexOfUIBtns]);
            indexOfUIBtns++;
        }
        else
        {
            canvas.SetActive(false);
        }
    }

    public void ButtonPressed(Button button)
    {
        ButtonColorWhenPressed(button);
        DisableEnableScroll(false);
    }

    public void ItemInstantiate(GameObject itemToEat)
    {
        InstantiateItemFromButton(itemToEat);
    }

    public void ButtonColorWhenPressed(Button button)
    {
        button.interactable = false;
        currentPressedButton = button;
    }
    
    
    public void DisableEnableScroll(bool isDisable)
    {
        scrollRect.horizontal = isDisable;
    }
    
  
    void InstantiateItemFromButton(GameObject itemForEating)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.75f));
        GameObject instantiatedPrefab = Instantiate(itemForEating, pos, itemForEating.transform.localRotation, parent);
    }

    public void EnabeleButton()
    {
        currentPressedButton.interactable = true;
    }


    IEnumerator ChangeCameras()
    {
        bowlCam.Priority = 9;
        yield return new WaitForSeconds(.1f);
        brain.m_DefaultBlend.m_Time = 2;
        bowlCam.Priority = 11;
    }

    void EatingCam()
    {
        bowlCam.Priority = 9;
        StartCoroutine(AnimationOfEating());
    }

    IEnumerator AnimationOfEating()
    {
        if (checkMarkNumPressed < 3)
        {
            yield return new WaitForSeconds(1.8f);
            
            float wait = crushFoodScript.GetTimeForAnimation();
            crushFoodScript.Rotate();
            StartCoroutine(DestroyFood());
            yield return new WaitForSeconds(wait);
        }
        else
        {
            StartCoroutine(DestroyFood());
        }
        Debug.Log("IZMELJA LI GA");
        modelAnimator.SetBool("IsHappy", false);
        modelAnimator.SetBool("CanEat", true);
        
        yield return new WaitForSeconds(4f);
       
        modelAnimator.SetBool("CanEat", false);
        itemsFromUIScript.AnimateItem();
        yield return new WaitForSeconds(1f);
        modelAnimator.SetBool("IsHappy", true);
        //funckija za izraslinu
        
        yield return new WaitForSeconds(2);
        bowlCam.Priority = 11;
        yield return new WaitForSeconds(1.8f);
        canvas.SetActive(true);
        ContentIndex();
    }

    public void ChangeScrollContent(RectTransform newScrollUi)
    {
        scrollRect.content = newScrollUi;
    }

    IEnumerator DestroyFood()
    {
        yield return new WaitForSeconds(0.4f); // vrijeme koje treba food breaker-u da se spusti u zdjelu

        for (int i = 0; i < parent.childCount; i++)
        {
            int num = parent.GetChild(i).GetComponent<ObjectFollowTouch>().GetID();
            Debug.Log(num);
            itemsFromUIScript.AddID(num);
            yield return new WaitForSeconds(0.1f);
            //parent.GetChild(i).gameObject.SetActive(false);
            Destroy(parent.GetChild(0).gameObject);
        }
    }
}
