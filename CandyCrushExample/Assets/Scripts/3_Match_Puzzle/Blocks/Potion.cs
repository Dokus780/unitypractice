using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

// Potion -> Block
// �̸� ���� �� �޸� ���� �߻�

// TODO : 1. Potion -> Block �����丵
public class Potion : MonoBehaviour
{
    public PotionType potionType;

    // ��ǥ�� �ǹ��ϴ°� �ƴ϶� ��°��
    public int xIndex;
    public int yIndex;

    public bool isMatched;

    // TODO : 1. �ش� ���� ���ҵǸ� ��Ī�� �Ǵ��� ���� (true�� �߱�, ���Ʒ��� ��¦ ������ ����)
    //public bool isMatchable;

    private Vector2 currentPos; // firstTouchPosition
    private Vector2 targetPos; // finalTouchPosition
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    public bool isMoving;

    // Ư���� üũ
    //public bool isBomb;
    //public bool isDrillVertical;
    //public bool isDrillVertical;
    //public bool isDrillVertical;  
    //public bool isDrillVertical;

    public Potion(int _x, int _y)
    {
        xIndex = _x;
        yIndex = _y;
        isMoving = false;
    }

    public void SetIndicies(int _x, int _y)
    {
        xIndex = _x;
        yIndex = _y;
    }

    public void OnMouseDown()
    {
        if (!isMoving)
        {
            currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void OnMouseUp()
    {
        if (!isMoving)
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        // swipeResist �̻��� �Է��� ���� ���� ����
        if (Mathf.Abs(targetPos.y - currentPos.y) > swipeResist || Mathf.Abs(targetPos.x - currentPos.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(targetPos.y - currentPos.y, targetPos.x - currentPos.x) * 180 / Mathf.PI;
        }
    }

    public void MoveToTarget(Vector2 _targetPos)
    {

        StartCoroutine(MoveCoroutine(_targetPos));
    }

    // TODO : 1. �� �ٲ�� �ð� �ڿ������� ����
    //        2. ���� �΋H���� �� �ӵ�, �Ÿ� ���� �ʿ� -> ���� �΋H���� �� ���� �޼��� ������ �ҵ�
    //           -> UI ���� �� ���� �Ÿ� �����ؼ� �۾� ����
    //        3. ���� �� �����Ǿ� �� �ڸ��� �������� �͵� ���⼭ ó���ϴµ� ���� �޼��� ������ �ҵ�
    private IEnumerator MoveCoroutine(Vector2 _targetPos)
    {
        isMoving = true;
        // ���� �ִϸ��̼� ���� �ð���
        float duration = 0.2f;

        Vector2 startPosition = transform.position;
        float elaspedTime = 0f;

        while (elaspedTime < duration)
        {
            float t = elaspedTime / duration;

            transform.position = Vector2.Lerp(startPosition, _targetPos, t);

            elaspedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = _targetPos;
        isMoving = false;
    }
}

// TODO : 1. Ư�� ��
public enum PotionType
{
    // �⺻ ��
    BlueBlock,  
    GreenBlock,
    OrangeBlock,
    PinkBlock,
    PurpleBlock,
    RedBlock,
    YellowBlock,
    // Ư�� ��
    Bomb, // ��ź
    DrillVertical, // �帱 ����
    DrillHorizontal, // �帱 ����
    Pick, // ���
    //PickLeft, // ��� ���밢(���� �����)
    //PickRight, // ��� �밢(������ �����)
    Prism, // ������
    
}