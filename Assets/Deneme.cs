using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
public class Deneme : MonoBehaviour
{
    [SerializeField] VideoClip videoClip;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] string videosPath;

    public void CalisKopek()
    {
        // videoPlayer.url = System.IO.Path.Combine(videosPath, "Drag_Nodes.mp4");
        videoPlayer.url = "https://drive.google.com/file/d/1nnx3gKoN8G10GBq3ioVB2E_jbmBM64cx/view?usp=share_link";
        videoPlayer.Play();

    }
}
