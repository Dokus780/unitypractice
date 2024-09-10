using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBlockEffect : MonoBehaviour, IRecyclableGameObject
{
    // ���� ������Ʈ Ȱ��ȭ ������Ƽ
    public bool isActive { get; set; } = true;

    private void OnExplotionAnimEnded()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        isActive = true;
    }

    private void OnDisable()
    {
        isActive = false;
    }
}
