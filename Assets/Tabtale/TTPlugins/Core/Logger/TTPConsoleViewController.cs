﻿using UnityEngine;
using UnityEngine.UI;

namespace Tabtale.TTPlugins
{
    public class TTPConsoleViewController : MonoBehaviour
    {
        GameObject panelConsole;
        GameObject buttonShow;
        GameObject buttonHide;
        GameObject buttonShare;
        GameObject buttonTestAb;
            
        void Start()
        {
            panelConsole = gameObject.transform.Find("Panel").gameObject;
            buttonShow = gameObject.transform.Find("ShowButton").gameObject;
            buttonShow.GetComponent<Button>().onClick.AddListener(OnShowConsoleClicked);
            buttonHide = gameObject.transform.Find("Panel/MinimazeButton").gameObject;
            buttonHide.GetComponent<Button>().onClick.AddListener(OnMinimizeConsoleClicked);
            buttonShare = gameObject.transform.Find("Panel/ShareButton").gameObject;
            buttonShare.GetComponent<Button>().onClick.AddListener(OnShareClicked);
            buttonTestAb = gameObject.transform.Find("Panel/TestAbButton").gameObject;
            buttonTestAb.GetComponent<Button>().onClick.AddListener(OnTestAbClicked);
            gameObject.SetActive(true);
            TTPTestAB.OnHideConsole = () =>
            {
                panelConsole.SetActive(true);
            };
        }

        void OnTestAbClicked()
        {
            panelConsole.SetActive(false);
            TTPTestAB.Show();
        }
        
        void OnShowConsoleClicked()
        {
            panelConsole.SetActive(true);
            buttonShow.SetActive(false);
            Debug.Log("TTPLog::OnShowConsoleClicked");
        }

        void OnMinimizeConsoleClicked()
        {
            panelConsole.SetActive(false);
            buttonShow.SetActive(true);
            Debug.Log("TTPLog::OnMinimizeConsoleClicked");
        }

        void OnShareClicked()
        {
            Debug.Log("TTPLog::OnShareClicked");
#if !UNITY_EDITOR && TTP_DEV_MODE
            TTPLogger.FlushBuffer();
            string path = TTPLogger.GetFilePath();
            if (path.Length != 0)
            {
                NativeShare.Share("", path, "", "logfile");
            }
#endif
        }
    }
}