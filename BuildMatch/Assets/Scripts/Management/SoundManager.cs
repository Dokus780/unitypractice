using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour, IManager
{
    public GameManager gameManager
    {
        get
        {
            return GameManager.gameManager;
        }
    }

    //public static SoundManager instance;
    public AudioClip[] audioClips;

    public AudioSource _BGMAudioSource = null;


    // BGM ���ӵǰ� �� ��� �ش� BGM ������ҽ� ã�Ƽ� DontDestroyOnLoad
    // : ���񿡼� �ٽ� ����¡ �� �� �ٽ� ���� �� ���� ������ �ؼ� ������
    //private void Awake()
    //{
    //    if (_BGMAudioSource == null)
    //    {
    //        _BGMAudioSource = GameObject.Find("BGMAudioSource").GetComponent<AudioSource>();
            
    //    }
    //    DontDestroyOnLoad(_BGMAudioSource);
    //}

    public void PlayBackgroundSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClips[0], position);
    }

    public void PlayUIClickSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClips[1], position);
    }

    public void PlayCharacterWalkSound(Vector3 position)
    {
        if (GameObject.FindObjectsOfType<AudioSource>().Length == 1)
        {
            AudioSource.PlayClipAtPoint(audioClips[2], position, 0.7f);
        }
    }
}
