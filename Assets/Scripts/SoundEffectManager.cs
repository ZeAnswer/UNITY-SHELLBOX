using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] public AudioClip shellXshell;
    [SerializeField] public  AudioClip shellXsurface;
    [SerializeField] public  AudioClip[] shellXsword = new AudioClip[3];
    [SerializeField] public  AudioClip swordXsurface;
    private static SoundEffectManager instance;
    private float MIN_VOLUME = 0.1f;
    private const float MAX_FORCE = 15000f;
    private const float MIN_FORCE = 20f;
    private float vol_modifier = 1f;
    private int mute = 1;
    
    void Awake(){
        instance = this;
    }


    public static SoundEffectManager GetInstance(){
        return instance;
    }

    private void play_shellXshell(Vector3 pos, float mag){
       playVarClipAt(shellXshell, pos, mag);
    }

    private void play_shellXsurface(Vector3 pos, float mag){
       playVarClipAt(shellXsurface, pos, mag);
    }

    private void play_shellXsword(Vector3 pos, float mag){
        int variant = Random.Range(0, 3);
        playVarClipAt(shellXsword[variant], pos, mag);
    }

    private void play_swordXsurface(Vector3 pos, float mag){
        playVarClipAt(swordXsurface, pos, mag);
    }

    private AudioSource playVarClipAt(AudioClip clip, Vector3 pos, float mag){
        float pitch = 1f + Random.Range(-0.25f, 0.25f);
        return playClipAt(clip, pos, mag, pitch);
    }

    private AudioSource playClipAt(AudioClip clip, Vector3 pos, float mag, float pitch){
        GameObject tempobj = new GameObject("TempAudio");
        tempobj.transform.position = pos;
        AudioSource tempas = tempobj.AddComponent<AudioSource>();
        tempas.spatialBlend = 0.3f;
        tempas.clip = clip;
        tempas.volume = mag * vol_modifier * mute;
        tempas.pitch = pitch;
        tempas.Play();
        //Debug.Log("play clip with vol " + mag.ToString() + "\n");
        Destroy(tempobj, clip.length);
        return tempas;
    }
    //Contact types (will be enums later) 1:shellXshell, 2:shellXsurface, 3:shellXsword, 4:swordXsurface/sword
    public void PlaySoundEffect(Vector3 pos, float mag, int contactType){
        float vol = Mathf.Max(MIN_VOLUME, Mathf.Min((mag - MIN_FORCE)/(MAX_FORCE - MIN_FORCE), 1f));
        switch (contactType)
        {
            case 1:
                play_shellXshell(pos, vol);
                break;
            case 2:
                play_shellXsurface(pos, vol);
                break;
            case 3:
                play_shellXsword(pos, vol);
                break;
            default:
                play_swordXsurface(pos, vol);
                break;
        }
    }

    public void changeVol(float modifier){
        vol_modifier = modifier;
    }

    public void muteVol(bool mut){
        mute = mut ? 0 : 1;
    }

}