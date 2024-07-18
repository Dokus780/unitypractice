using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

// Potion -> Block
// 이름 변경 시 메모리 부족 발생

// TODO : 1. Potion -> Block 리팩토링
public class Potion : MonoBehaviour
{
    public PotionType potionType;

    // 좌표를 의미하는게 아니라 번째임
    public int xIndex;
    public int yIndex;

    public bool isMatched;

    // TODO : 1. 해당 블럭이 스왑되면 매칭이 되는지 여부 (true면 발광, 위아래로 살짝 움직임 적용)
    //public bool isMatchable;

    private Vector2 currentPos; // firstTouchPosition
    private Vector2 targetPos; // finalTouchPosition
    public float swipeAngle = 0;

    public bool isMoving;

    public Potion(int _x, int _y)
    {
        xIndex = _x;
        yIndex = _y;
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
        swipeAngle = Mathf.Atan2(targetPos.y - currentPos.y, targetPos.x - currentPos.x) * 180 / Mathf.PI;
    }

    public void MoveToTarget(Vector2 _targetPos)
    {

        StartCoroutine(MoveCoroutine(_targetPos));
    }

    // TODO : 1. 블럭 바뀌는 시간 자연스럽게 조절
    //        2. 벽에 부딫혔을 때 속도, 거리 조절 필요 -> 벽에 부딫혔을 때 따로 메서드 만들어야 할듯
    //           -> UI 적용 후 벽과 거리 측정해서 작업 예정
    //        3. 새로 블럭 생성되어 빈 자리에 떨어지는 것도 여기서 처리하는데 따로 메서드 만들어야 할듯
    private IEnumerator MoveCoroutine(Vector2 _targetPos)
    {
        isMoving = true;
        // 스왑 애니메이션 지속 시간임
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

// TODO : 1. 특수 블럭
public enum PotionType
{
    BlueBlock,
    PurpleBlock,
    GreenBlock,
    OrangeBlock,
    PinkBlock,
    RedBlock,
    YellowBlock,
}

// 바구니 제작중...
public struct Bag
{
    public PotionType potionType;

}