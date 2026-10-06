using UnityEngine;
using FMODUnity;
using System;
using System.Runtime.InteropServices;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference music;

    private FMOD.Studio.EventInstance musicInstance;

    private void Awake()
    {
        if (!music.IsNull)
        {
            musicInstance = RuntimeManager.CreateInstance(music);
            musicInstance.start();

        }

    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();

    }

}
