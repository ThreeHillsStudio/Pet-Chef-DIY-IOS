using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ObjectFollowTouch : MonoBehaviour
{
    Touch touch;
    Vector3 pos;
    private bool isEnded = false;

    private ChangeUI changeUI;

    [SerializeField] private int ID = 0;
    
    private void Start()
    {
        changeUI = FindObjectOfType<ChangeUI>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            isEnded = false;
            touch = Input.GetTouch(0);
            
            pos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.75f/*1.65f*/));
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 100, Color.red);
            this.transform.position = pos;
        }

        if (touch.phase is TouchPhase.Ended && !isEnded)
        {
            CheckIsItTouchingBowl();
        }
    }

    public void CheckIsItTouchingBowl()
    {
        
        Debug.Log("TOUCH ENDED");
        Debug.Log(Camera.main.transform.position);
        isEnded = true;
        if (IsRaycastHittingBowl())
        {
            ILiquid liquid = this.gameObject.GetComponent<ILiquid>();

            if (liquid != null)
            {
                liquid.BottleAnimation();
                changeUI.DisableEnableScroll(true);
                return;
            }
            
            this.gameObject.AddComponent<Rigidbody>();
            
        }
        else
        {
            Destroy(this.gameObject);
            changeUI.EnabeleButton();
        }
        changeUI.DisableEnableScroll(true);
        ChangeLayerAndDisableScript();
    }

    bool IsRaycastHittingBowl()
    {
        LayerMask layerMask = LayerMask.GetMask("Bowl");
  
        if (Physics.Raycast(this.transform.position, Vector3.down,  100f, layerMask))
        {
            Debug.Log("Hitting Bowl!");
            return true;
        }
        Debug.Log("NOT Hitting Bowl!");
        return false;
    }
    
    public void ChangeLayerAndDisableScript()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        ChangeChildrenLayer();
        this.enabled = false;
    }

    public void ChangeChildrenLayer()
    {
        Transform[] ts = GetComponentsInChildren<Transform>();

        if (ts != null)
        {
            foreach (Transform child in ts)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    public int GetID()
    {
        return ID;
    }
}