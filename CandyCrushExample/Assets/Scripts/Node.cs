using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //to determine whether the space can be filled with potions or not.
    public bool isUsable;

    // TODO : 1. �ش� ��尡 ���ҵǸ� ��Ī�� �Ǵ��� ���� (true�� �߱�, ���Ʒ��� ��¦ ������ ����)
    //public bool isMatchable;

    public GameObject potion;

    public Node(bool _isUsable, GameObject _potion)
    {
        isUsable = _isUsable;
        potion = _potion;
    }
}
