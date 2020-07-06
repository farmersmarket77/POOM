using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    private AudioMaster() { }
    public static AudioMaster instance = null;
    
    public List<AudioSource> listSFX = new List<AudioSource>();
    public AudioSource BGM;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        BGM.Play();
    }

    public void PlaySFX(AudioClip _clip)
    {
        for (int i = 0; i < listSFX.Count; i++)
        {
            if (listSFX[i].clip == null)
            {
                StartCoroutine(PlaySound(listSFX[i], _clip));
                break;
            }
        }
    }

    private IEnumerator PlaySound(AudioSource _source, AudioClip _clip)
    {
        float clipLenght = _clip.length;
        _source.clip = _clip;
        _source.Play();

        while (clipLenght >= 0f)
        {
            clipLenght -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _source.clip = null;
    }
}
