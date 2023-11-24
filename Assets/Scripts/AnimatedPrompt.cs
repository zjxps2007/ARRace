using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPrompt : MonoBehaviour
{
    public enum InstructionUI
    {
        CrossPlatformFindAPlane,
        FindAFace,
        FindABody,
        FindAnImage,
        FindAnObject,
        ARKitCoachingOverlay,
        TapToPlace,
        None
    };

    [SerializeField] private InstructionUI instruction;
    [SerializeField] private ARUXAnimationManager animationManager;

    private bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        ShowInstructions();
        isStarted = true;
    }

    private void OnEnable()
    {
        if (isStarted)
        {
            ShowInstructions();
        }
    }

    private void OnDisable()
    {
        animationManager.FadeOffCurrentUI();
    }

    void ShowInstructions()
    {
        switch (instruction)
        {
            case InstructionUI.CrossPlatformFindAPlane:
                animationManager.ShowCrossPlatformFindAPlane();
                break;
            case InstructionUI.FindAFace:
                animationManager.ShowFindFace();
                break;
            case InstructionUI.FindABody:
                animationManager.ShowFindBody();
                break;
            case InstructionUI.FindAnImage:
                animationManager.ShowFindImage();
                break;
            case InstructionUI.FindAnObject:
                animationManager.ShowFindObject();
                break;
            case InstructionUI.TapToPlace:
                animationManager.ShowTapToPlace();
                break;
            default:
                Debug.LogError("instruction switch missing, please edit Animated Prompt.cs" + instruction);
                break;
        }
    }
}