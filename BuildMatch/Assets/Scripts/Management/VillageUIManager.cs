using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageUIManager : MonoBehaviour
{
    public GameObject _ControllerUI = null;
    public GameObject _QuestUI = null;

    private void Awake()
    {
        _ControllerUI = GameObject.Find("Canvas");
        // TODO : 1. UI ������, ���� ���� ���� ���� ¥��
        //_QuestUI = GameObject.Find("CanvasNPC");
        //_QuestUI.SetActive(false);
    }
}
