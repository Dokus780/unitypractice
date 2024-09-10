
using TMPro;
using UnityEngine;

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

    private void Awake()
    {
        _SpriteRenderer = _Bag.GetComponentInChildren<SpriteRenderer>();
        _Text = _Bag.GetComponentInChildren<TMP_Text>();;
    }
    // Start is called before the first frame update
    void Start()
    {

    }
}
