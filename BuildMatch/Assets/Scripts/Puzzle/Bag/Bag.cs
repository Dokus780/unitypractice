
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ��ǥ �ٱ��� ������Ʈ
// TDOD : 1. �ӽ÷� PuzzleManager���� Bag ������Ʈ ó����
//           ���߿� ���� Bag�� �ش� ��ũ��Ʈ �÷����� ó���ϵ��� ����
public class Bag : MonoBehaviour
{
    public GameObject _Bag;

    //public SpriteRenderer _SpriteRenderer = null;

    public TMP_Text _Text = null;

    public PotionType _PotionType;

    public int CurrentCount;
    public int GoalCount;

    // �ٱ��� �̹�����
    private Image[] bagImageList;

    // ��ǥ�� ä���� �� üũ�Ǵ� �̹��� ���� ����
    private Image ClearImage;

    public bool ClearCheck = false; // currentCount == GoalCount �Ǹ� check��

    // TODO : 1. Instituate �Լ��� ������ �����ϱ�? �� �� ������ �� ������
    public Bag(int _GoalCount)
    {
        CurrentCount = 15;
        GoalCount = _GoalCount;
    }

    public void SetGoalCount(int _GoalCount)
    {
        GoalCount = _GoalCount;
        _Text.text = CurrentCount.ToString() + " / " + GoalCount.ToString();
    }

    private void Awake()
    {
        //_SpriteRenderer = _Bag.GetComponentInChildren<SpriteRenderer>();
        _Text = _Bag.GetComponentInChildren<TMP_Text>();;
        _Text.text = CurrentCount.ToString() + " / " + GoalCount.ToString();
        bagImageList = _Bag.GetComponentsInChildren<Image>();
        ClearImage = bagImageList[2];
        ClearImage.gameObject.SetActive(false);
        ClearCheck = false;
    }

    private void Update()
    {
        _Text.text = CurrentCount.ToString() + " / " + GoalCount.ToString();

        // TODO : 1. �Ϸ�Ǹ� ������ ���� ���� üũ �̹����� ����ǰ�
        if (CurrentCount >= GoalCount)
        {
            CurrentCount = GoalCount;
            // sprite üũ�� ����
            ClearImage.gameObject.SetActive(true);
            _Text.gameObject.SetActive(false);
            ClearCheck = true;
            //_Text.color = Color.red;
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
