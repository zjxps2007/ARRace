using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class InteractionModeDictionary : SerializableDictionaryBase<string, GameObject> { }

public class InteractionConteroller : Singleton<InteractionConteroller>
{
    [SerializeField] private InteractionModeDictionary interactionMode;

    private GameObject currentMode;

    void Awake()
    {
        base.Awake();
        ResetAllMode();
    }

    void ResetAllMode()
    {
        foreach (GameObject mode in interactionMode.Values)
        {
            mode.SetActive(false);
        }
    }

    public static void EnableMode(string name)
    {
        Instance?._EnableMode(name);
    }

    void _EnableMode(string name)
    {
        GameObject modeObject;
        if (interactionMode.TryGetValue(name, out modeObject))
        {
            StartCoroutine(ChangeMode(modeObject));
        }
        else
        {
            Debug.LogError("undefind mode named " + name);
        }
    }

    IEnumerator ChangeMode(GameObject mode)
    {
        if (mode == currentMode)
        {
            yield break;
        }

        if (currentMode)
        {
            currentMode.SetActive(false);
        }

        currentMode = mode;
        mode.SetActive(true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _EnableMode("Startup");
    }
    
}
