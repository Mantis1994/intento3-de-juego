using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public List<AudioEntry> audioClips; // Asigna los audios y nombres desde el inspector
    private Dictionary<string, AudioClip> audioDict;

    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }


        // Inicializar el diccionario
        audioDict = new Dictionary<string, AudioClip>();
        foreach (var entry in audioClips)
        {
            if (!audioDict.ContainsKey(entry.name))
                audioDict.Add(entry.name, entry.clip);
            else
                Debug.LogWarning("AudioManager: Nombre duplicado en audioClips: " + entry.name);
        }
    }

    public void PlaySFX(string name)
    {
        if (audioDict.ContainsKey(name) && sfxSource != null)
            sfxSource.PlayOneShot(audioDict[name]);
    }



}











[System.Serializable]
public class AudioEntry
{
    public string name;
    public AudioClip clip;
        
    public AudioEntry(string name, AudioClip clip)
    {
        this.name = name;
        this.clip = clip;       
    }
}

