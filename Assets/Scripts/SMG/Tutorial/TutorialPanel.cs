using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [System.Serializable]
    public class KeyInfo
    {
        public string name;
        public KeyCode code;
        public bool isUse;
        public bool canCapture;
        public bool isHold;
        public float holdTime;
    }

    public KeyInfo[] tutorialKeys1P;
    public KeyInfo[] tutorialKeys2P;

    float[] holdDeltaTimes1P;
    float[] holdDeltaTimes2P;

    public Action completed;
    public bool isActive = false;
    bool isComplete = false;
    //bool isSubComplete
    public Func<bool> ExtraIF1P;
    public Func<bool> ExtraIF2P;

    private void Awake()
    {
        if (tutorialKeys1P.Length > 0)
        {
            holdDeltaTimes1P = new float[tutorialKeys1P.Length];
        }
        if (tutorialKeys2P.Length > 0)
        {
            holdDeltaTimes2P = new float[tutorialKeys2P.Length];
        }
        ResetPanel();
    }

    private void Update()
    {
        if (!isActive)
            return;

        bool check1P = CheckTutorial(tutorialKeys1P, holdDeltaTimes1P, ExtraIF1P);
        bool check2P = CheckTutorial(tutorialKeys2P, holdDeltaTimes2P, ExtraIF2P);

        if ((check1P || check2P) && !isComplete)
        {
            isComplete = true;
            CompleteMission(true);
        }
    }

    bool CheckTutorial(KeyInfo[] tutorialKeys, float[] holdDeltaTimes, Func<bool> extraIF = null)
    {
        KeyInfo key;
        bool isAllUse = true;
        
        for (int i = 0; i < tutorialKeys.Length; i++)
        {
            key = tutorialKeys[i];
            if (key.isHold)
            {
                if (Input.GetKey(key.code))
                {
                    holdDeltaTimes[i] += Time.deltaTime;
                    if (holdDeltaTimes[i] >= key.holdTime)
                    {
                        key.isUse = true;
                        UsedKey(key.name, true);
                    }
                }
                else
                {
                    if(!key.canCapture)
                    {
                        UsedKey(key.name, false);
                    }
                    holdDeltaTimes[i] = 0f;
                }
            }
            else
            {
                if (Input.GetKeyDown(key.code))
                {
                    key.isUse = true;
                    UsedKey(key.name, true);
                }
                else if((!key.canCapture) && Input.GetKeyUp(key.code))
                {
                    UsedKey(key.name, false);
                }
                    
                
            }

            if (extraIF != null && extraIF() == false)
            {
                key.isUse = false;
            }

            if (!key.isUse)
            {
                isAllUse = false;
            }
                
        }
        return isAllUse;
    }

    void ResetPanel()
    {
        isActive = false;
        isComplete = false;
        for(int i = 0; i < holdDeltaTimes1P.Length; i++)
        {
            holdDeltaTimes1P[i] = 0f;
        }
        for (int i = 0; i < holdDeltaTimes2P.Length; i++)
        {
            holdDeltaTimes2P[i] = 0f;
        }
        for (int i = 0; i < tutorialKeys1P.Length; i++)
        {
            tutorialKeys1P[i].isUse = false;
        }
        for (int i = 0; i < tutorialKeys2P.Length; i++)
        {
            tutorialKeys2P[i].isUse = false;
        }
        ExtraIF1P = null;
        ExtraIF2P = null;
    }
    public void ShowPanel(bool show)
    {
        Image[] images = GetComponentsInChildren<Image>();
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();

        for(int i = 0; i < images.Length; i++)
        {
            Color _color = images[i].color;
            _color.a = (show) ? 0.6f : 0f;
            images[i].color = _color;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            Color _color = texts[i].color;
            _color.a = (show) ? 1f : 0f;
            texts[i].color = _color;
        }
    }

    public void CompleteMission(bool completed)
    {
        Image background = transform.GetChild(0)?.GetComponent<Image>();
        if (background == null)
            return;

        Color _color = background.color;
        _color.g = (completed) ? 1f : 0f;
        background.color = _color;

        StartCoroutine(CompleteMissionCoroutine(3f));
    }

    IEnumerator CompleteMissionCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        completed?.Invoke();
    }

    public void UsedKey(string key, bool used)
    {
        Image background = transform.GetChild(1)?.Find(key)?.GetComponent<Image>();
        if (background == null)
            return;

        Color _color = background.color;
        _color.r = (used) ? 0f : 1f;
        _color.b = (used) ? 0f : 1f;
        background.color = _color;
    }


    [ContextMenu("ResetUsedKeys()")]
    void ResetUsedKeys()
    {
        Transform keyboard = transform.GetChild(1)?.GetChild(0);
        if (keyboard != null)
        {
            for (int i = 0; i < keyboard.childCount; i++)
            {
                UsedKey(keyboard.name + "/" + keyboard.GetChild(i).name, false);
            }
        }
    }

    //#region Test Function

    //[Header("Context Menu")]
    //public string keyName;

    //[ContextMenu("ShowPanel(true)")]
    //void TextShowPanelTrue()
    //{
    //    ShowPanel(true);
    //}

    //[ContextMenu("ShowPanel(false)")]
    //void TextShowPanelFalse()
    //{
    //    ShowPanel(false);
    //}

    //[ContextMenu("CompleteMission(false)")]
    //void CompleteMissionFalse()
    //{
    //    CompleteMission(false);
    //}

    //[ContextMenu("CompleteMission(true)")]
    //void CompleteMissionTrue()
    //{
    //    CompleteMission(true);
    //}

    //[ContextMenu("UsedKey(keyName, true)")]
    //void UsedKeyTrue()
    //{
    //    UsedKey(keyName, true);
    //}

    //[ContextMenu("UsedKey(keyName, false)")]
    //void UsedKeyFalse()
    //{
    //    UsedKey(keyName, false);
    //}
    //#endregion
}
