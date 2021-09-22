using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    AudioSource source;
    public AudioMixerGroup audioMixerGroup;
    public string clipName;
    public AudioClip audioClip;

    public float volume;
    public float pitch;

    public bool loop = false;
    public bool playOnAwake = false;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = audioClip;
        source.pitch = pitch;
        source.loop = loop;
        source.volume = volume;
        source.playOnAwake = playOnAwake;
        source.outputAudioMixerGroup = audioMixerGroup;

    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public IEnumerator AbbassaVolumeCO(string _name, float vol)
    {
        float currentVol = vol;
        float currentTime = 0;

        float duration = 1.5f;

        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(1, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);

            source.volume = Mathf.Log10(newVol) * 20;

            yield return null;

        }


        yield return null;
    }

    public IEnumerator AlzaVolumeCo(string _name, float vol)
    {


        float currentVol = vol;
        Debug.Log("SOURCE VOL " + source.volume);
        float currentTime = 0;

        float duration = 1.5f;

        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(1, 0.0001f, 1);


        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(targetValue, currentVol, currentTime / duration);

            source.volume = Mathf.Log10(newVol) * 20;

            yield return null;

        }

        Debug.Log("clipname " + source.clip.name);

        yield return null;

    }


}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    Sound[] sound;

    [SerializeField] EnvironmentManager environmentManager;

    Dictionary<int, string> musiche = new Dictionary<int, string>();

    private void Awake()
    {
        for (int i = 0; i < sound.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sound[i].clipName);
            _go.transform.SetParent(this.transform);
            sound[i].SetSource(_go.AddComponent<AudioSource>());
        }

        musiche.Add(0, "Livello_0");
        musiche.Add(1, "Livello_1");     

    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].clipName == _name)
            {
                sound[i].Play();

                Debug.Log("VOLUME " + sound[i].volume);

                return;
            }
        }
    }
    public void StopSound(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].clipName == _name)
            {
                sound[i].Stop();
                return;
            }
        }
    }

    public void AbbassaVolume(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].clipName == _name)
            {
                StartCoroutine(sound[i].AbbassaVolumeCO(sound[i].clipName, sound[i].volume));

                return;
            }
        }

    }

    public void AlzaVolume(string _name)
    {

        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].clipName == _name)
            {
                StartCoroutine(sound[i].AlzaVolumeCo(sound[i].clipName, sound[i].volume));

                return;
            }
        }

    }

    public void SwapMusicLevel(int currentScore, int targetScore)
    {

        Debug.Log("musica cur" + musiche[currentScore]);

        Debug.Log("musica tar" + musiche[targetScore]);

        AbbassaVolume(musiche[currentScore]);
        //StopSound(musiche[currentScene]);

        PlaySound(musiche[targetScore]);

        AlzaVolume(musiche[targetScore]);

    }

    private void Start()
    {
         PlaySound(musiche[0]);
      
    }

    private void Update()
    {
      

                
    }


}
