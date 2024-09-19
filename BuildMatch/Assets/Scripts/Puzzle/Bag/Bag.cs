
using TMPro;
using UnityEngine;

// ��ǥ �ٱ��� ������Ʈ
// TDOD : 1. �ӽ÷� PuzzleManager���� Bag ������Ʈ ó����
//           ���߿� ���� Bag�� �ش� ��ũ��Ʈ �÷����� ó���ϵ��� ����
public class Bag : MonoBehaviour
{
    public GameObject _Bag;

    public SpriteRenderer _SpriteRenderer = null;

    public TMP_Text _Text = null;

    public PotionType _PotionType;

    public int CurrentCount;
    public int GoalCount;

    public Bag(int _CurrentCount, int _GoalCount)
    {
        CurrentCount = _CurrentCount;
        GoalCount = _GoalCount;
    }

    private void Awake()
    {
        _SpriteRenderer = _Bag.GetComponentInChildren<SpriteRenderer>();
        _Text = _Bag.GetComponentInChildren<TMP_Text>();;
    }

    private void Update()
    {
        if (CurrentCount >= GoalCount)
        {
            CurrentCount = GoalCount;
            // sprite üũ�� ����
            
            _Text.color = Color.red;
        }
    }

    public void UpdateCount()
    {
        if (CurrentCount < GoalCount)
        {
            CurrentCount++;
        }
    }
}
