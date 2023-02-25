using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class ShowIntructions : MonoBehaviour
{
    [Header("Videos - Instructions")]
    [SerializeField] TextMeshProUGUI text_instruction; 
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] string[] instruction_texts;
    
    [Header("Canvas")]
    [SerializeField] GameObject instructionList;
    [SerializeField] GameObject videoCanvas;
    [SerializeField] GameObject videoCanvasBG;
    [SerializeField] VideoPlayer videoPlayer;

    bool isActivated;


    public void ActivateVideoCanvas()
    {
        videoCanvas.SetActive(!videoCanvas.activeSelf);
        videoCanvasBG.SetActive(!videoCanvasBG.activeSelf);
        OpenInstructionList();
        isActivated = !isActivated;

        if (isActivated)
        {
            ChangeInstruction(0);
        }

    }


    public void ChangeInstruction(int index)
    {
        ChangeVideo(index);
        ChangeText(index);
    }

    public void ChangeVideo(int indexVideo)
    {
        videoPlayer.clip = videoClips[indexVideo];
    }


    public void OpenInstructionList()
    {
        instructionList.SetActive(!instructionList.activeSelf);
    }

    public void ChangeText(int indexText)
    {
        text_instruction.text = instruction_texts[indexText];
    }

}
