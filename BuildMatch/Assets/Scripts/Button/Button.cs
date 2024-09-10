using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    private PlayerInstance _PlayerInstance = null;

    // TODO : 1. ���� ���� ¥�� ó���ϱ�
    private SoundManager _SoundManager = null;

    private StageManager _StageManager = null;

    private void Awake()
    {
        _PlayerInstance = GameManager.GetManagerClass<CharacterManager>().player;
        _SoundManager = GameManager.GetManagerClass<SoundManager>();
        _StageManager = GameManager.GetManagerClass<StageManager>();
    }

    #region VillageScene
    public void OnMapButtonClicked()
    {
        _SoundManager.PlayUIClickSound(Vector3.zero);
        //SceneManager.LoadScene("StageScene");
        SceneManager.LoadScene("PuzzleScene");

    }

    public void OnHomeButtonClicked()
    {
        _SoundManager.PlayUIClickSound(Vector3.zero);
        SceneManager.LoadScene("VillageScene");
    }

    // �ӽ� ��ư
    public void ReturnOriginCamera()
    {
        _PlayerInstance.isQuestEnd = true;
    }
    #endregion

    #region StageScene
    public void OnStage1ButtonClicked()
    {
        _SoundManager.PlayBackgroundSound(Vector3.zero);
        SceneManager.LoadScene("PuzzleScene");
    }

    public void OnStage2ButtonClicked()
    {
        // TODO : 1. �������� �Ŵ������� ����
        _SoundManager.PlayBackgroundSound(Vector3.zero);

    }
    #endregion
}
