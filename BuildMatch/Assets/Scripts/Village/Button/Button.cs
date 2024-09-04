using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    // TODO : 1. ���� ���� ¥�� ó���ϱ�
    private SoundManager _SoundManager = null;

    private void Awake()
    {
        _SoundManager = VillageGameManager.GetManagerClass<SoundManager>();

    }
    public void OnMapButtonClicked()
    {
        _SoundManager.PlayUIClickSound(Vector3.zero);
        SceneManager.LoadScene("PuzzleScene");
    } 
}
