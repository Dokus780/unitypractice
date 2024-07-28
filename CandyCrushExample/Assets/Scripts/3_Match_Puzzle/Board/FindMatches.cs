using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class FindMatches : MonoBehaviour
{
    private PotionBoard board;

    // �������� �� ���⿡ �����ؼ� ����
    public List<Potion> potionsToRemove = new();

    // Ư���� ���� ����
    // 4 ���� üũ �� �帱(����) ����
    public bool isCheckedHorizontal_4 = false;
    // 4 ���� üũ �� �帱(����) ����
    public bool isCheckedVertical_4 = false;
    // �׸� üũ �� ��� ����
    public bool isCheckedSquare = false;
    // 5 ����, ���� üũ �� ������ ����
    public bool isCheckedMatched_5 = false;
    // 5 L�� üũ �� ��ź ����
    public bool isCheckedSuper = false;


    void Awake()
    {
        board = FindObjectOfType<PotionBoard>();
    }

    // ���忡 ��Ī�Ǿ� �ִ°� �ִ��� üũ
    // TODO : 1. üũ �� ��Ī ����� ���� ���� ��� �ٽ� ������ ��
    //        2. ���� �ð��� ������ ������ ���� ��� ��Ī�Ǵ� �� ǥ��
    public bool FindAllMatches()
    {
        if (GameManager.Instance.isGameEnded)
        {
            return false;
        }

        bool hasMatched = false;

        potionsToRemove.Clear();

        foreach (Node nodePotion in board.potionBoard)
        {
            if (nodePotion.potion != null)
            {
                nodePotion.potion.GetComponent<Potion>().isMatched = false;
            }
        }

        Potion currentPotion = board.selectedPotion;
        Potion targetPotion = board.targetedPotion;

        // Ư���� üũ
        // ����, Ÿ�ϵ� ������ �ִ� ���(���ʷ� �������� ���� ��) Ư�� �� üũ �� �� �޼��� �� �Ʒ����� board�� ���� currentPotion, targetPotion ����
        if (currentPotion != null && targetPotion != null)
        {
            if (currentPotion.potionType == PotionType.Bomb || currentPotion.potionType == PotionType.DrillHorizontal ||
                currentPotion.potionType == PotionType.DrillVertical || currentPotion.potionType == PotionType.Prism ||
                currentPotion.potionType == PotionType.Pick)
            {
                potionsToRemove.AddRange(RunSpecialBlock(currentPotion, targetPotion));
                hasMatched = true;
            }

            if (targetPotion.potionType == PotionType.Bomb ||
                targetPotion.potionType == PotionType.DrillHorizontal || targetPotion.potionType == PotionType.DrillVertical ||
                targetPotion.potionType == PotionType.Prism || targetPotion.potionType == PotionType.Pick)
            {
                potionsToRemove.AddRange(RunSpecialBlock(targetPotion, currentPotion));
                hasMatched = true;
            }
        }


        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                // checking if potion node is usable
                if (board.potionBoard[x, y].isUsable)
                {
                    // then proceed to get potion class in node.
                    Potion potion = board.potionBoard[x, y].potion.GetComponent<Potion>();

                    // ensure its not matched
                    if (!potion.isMatched)
                    {
                        // run some matching logic

                        MatchResult matchedPotions = IsConnected(potion);

                        if (matchedPotions.connectedPotions.Count >= 3)
                        {
                            // complex matching...
                            MatchResult superMatchedPotions = FindSuperMatch(matchedPotions);

                            potionsToRemove.AddRange(superMatchedPotions.connectedPotions);

                            foreach (Potion pot in superMatchedPotions.connectedPotions)
                            {
                                pot.isMatched = true;
                            }

                            hasMatched = true;
                        }
                    }
                }
            }
        }

        // Ư���� üũ������ ���⿡�� Ư���� üũ �� ����, Ÿ�� �� ���� ����
        // ������ ��쿡�� Ư���� �ִ��� üũ
        board.selectedPotion = null;
        board.targetedPotion = null;

        return hasMatched;
    }

    // ���� �Ǵ� ���� ��Ī�� �Ͼ�� �� �ݴ�(�����̸� ����, �����̸� ���� ��Ī�� �Ͼ����) üũ
    // �ݴ� ���⵵ ��Ī�� �Ͼ�� ��� Super
    private MatchResult FindSuperMatch(MatchResult _matchedResults)
    {
        // if we have a horizontal or long horizontal match
        // loop through the potions in my match
        // create a new list of potions 'extra matches'
        // CheckDirection up
        // CheckDirection down
        // do we have 2 or more extra matches.
        // we've made a super match - return a new matchresult of type super
        // return extra matches
        if (_matchedResults.direction == MatchDirection.Horizontal_3 || _matchedResults.direction == MatchDirection.Horizontal_4 || _matchedResults.direction == MatchDirection.LongHorizontal)
        {
            foreach (Potion pot in _matchedResults.connectedPotions)
            {
                List<Potion> extraConnectedPotions = new();

                CheckDirection(pot, new Vector2Int(0, 1), extraConnectedPotions);
                // ������ üũ�ߴµ� ���� üũ �ʿ����� �׽�Ʈ �ʿ�
                CheckDirection(pot, new Vector2Int(0, -1), extraConnectedPotions);

                if (extraConnectedPotions.Count >= 2)
                {
                    // TODO : 1. L�� ���� ��Ī �� ��ź ����
                    isCheckedSuper = true;
                    extraConnectedPotions.AddRange(_matchedResults.connectedPotions);

                    return new MatchResult
                    {
                        connectedPotions = extraConnectedPotions,
                        direction = MatchDirection.Super
                    };
                }
            }
            return new MatchResult
            {
                connectedPotions = _matchedResults.connectedPotions,
                direction = _matchedResults.direction
            };
        }

        // if we have a vertical or long vertical match
        // loop through the potions in my match
        // create a new list of potions 'extra matches'
        // CheckDirection up
        // CheckDirection down
        // do we have 2 or more extra matches.
        // we've made a super match - return a new matchresult of type super
        // return extra matches
        if (_matchedResults.direction == MatchDirection.Vertical_3 || _matchedResults.direction == MatchDirection.Vertical_4 || _matchedResults.direction == MatchDirection.LongVertical)
        {
            foreach (Potion pot in _matchedResults.connectedPotions)
            {
                List<Potion> extraConnectedPotions = new();

                CheckDirection(pot, new Vector2Int(1, 0), extraConnectedPotions);
                CheckDirection(pot, new Vector2Int(-1, 0), extraConnectedPotions);

                if (extraConnectedPotions.Count >= 2)
                {
                    isCheckedSuper = true;
                    extraConnectedPotions.AddRange(_matchedResults.connectedPotions);

                    return new MatchResult
                    {
                        connectedPotions = extraConnectedPotions,
                        direction = MatchDirection.Super
                    };
                }
            }
            return new MatchResult
            {
                connectedPotions = _matchedResults.connectedPotions,
                direction = _matchedResults.direction
            };
        }
        return null;
    }

    // �� Ÿ���� ��ġ�ϴ��� Ȯ�� �� Match ��� ��ȯ
    public MatchResult IsConnected(Potion potion)
    {
        List<Potion> connectedPotions = new()
        {
            potion
        };

        // ���� üũ
        CheckHorizontalMatch(potion, connectedPotions);

        // check right
        //CheckDirection(potion, new Vector2Int(1, 0), connectedPotions);

        // check left -- ��� �ɵ�? üũ �ʿ�
        //CheckDirection(potion, new Vector2Int(-1, 0), connectedPotions);

        // TODO : 1. �׸� üũ
        //if (connectedPotions.Count >= 2)
        //{
        //    CheckSquare(potion, connectedPotions);
        //}

        // have we made a 3 match? (Horizontal match)
        if (connectedPotions.Count == 3)
        {
            //Debug.Log("I have a horizontal_3 match, the color of my match is : " + connectedPotions[0].potionType);

            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Horizontal_3
            };
        }
        // 4 �̻� ���� ��ġ
        else if (connectedPotions.Count == 4)
        {
            //Debug.Log("I have a horizontal_4 match, the color of my match is : " + connectedPotions[0].potionType);
            isCheckedHorizontal_4 = true;

            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Horizontal_4
            };
        }
        // 5 �̻� ���� ��ġ
        else if (connectedPotions.Count >= 5)
        {
            //Debug.Log("I have a Long horizontal match, the color of my match is : " + connectedPotions[0].potionType);
            isCheckedMatched_5 = true;

            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.LongHorizontal
            };
        }

        // clear out the connectedpotions
        connectedPotions.Clear();

        // read our initial potion
        connectedPotions.Add(potion);

        // ���� üũ
        CheckVerticalMatch(potion, connectedPotions);

        // check up
        //CheckDirection(potion, new Vector2Int(0, 1), connectedPotions);

        // check down -- ��� �ɵ�? üũ �ʿ�
        //CheckDirection(potion, new Vector2Int(0, -1), connectedPotions);


        // 3 ���� ��ġ
        if (connectedPotions.Count == 3)
        {
            //Debug.Log("I have a Vertical_3 match, the color of my match is : " + connectedPotions[0].potionType);

            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Vertical_3
            };
        }
        // 4 ���� ��ġ
        else if (connectedPotions.Count == 4)
        {
            //Debug.Log("I have a Vertical_4 match, the color of my match is : " + connectedPotions[0].potionType);
            isCheckedVertical_4 = true;

            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Vertical_4
            };
        }
        // 5 �̻� ���� ��ġ
        else if (connectedPotions.Count >= 5)
        {
            //Debug.Log("I have a Long Vertical match, the color of my match is : " + connectedPotions[0].potionType);
            isCheckedMatched_5 = true;

            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.LongVertical
            };
        }
        else
        {
            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.None
            };
        }
    }

    void CheckHorizontalMatch(Potion pot, List<Potion> connectedPotions)
    {
        PotionType potionType = pot.potionType;
        int x = pot.xIndex + 1;
        int y = pot.yIndex;

        // check that we're within the boundaries of the board
        while (x >= 0 && x < board.width)
        {
            if (board.potionBoard[x, y].isUsable)
            {
                Potion neighbourPotion = board.potionBoard[x, y].potion.GetComponent<Potion>();

                // does our potionType Match? it must also not be matched
                if (!neighbourPotion.isMatched && neighbourPotion.potionType == potionType)
                {
                    connectedPotions.Add(neighbourPotion);

                    // �׸� �߰� üũ
                    if (!isCheckedSquare && y < board.height - 1 && board.potionBoard[x - 1, y].isUsable && board.potionBoard[x, y + 1].isUsable && board.potionBoard[x - 1, y + 1].isUsable)
                    {
                        PotionType leftNeighbourPotionType = board.potionBoard[x - 1, y].potion.GetComponent<Potion>().potionType;
                        PotionType upNeighbourPotionType = board.potionBoard[x, y + 1].potion.GetComponent<Potion>().potionType;
                        PotionType leftupNeighbourPotionType = board.potionBoard[x - 1, y + 1].potion.GetComponent<Potion>().potionType;

                        if (neighbourPotion.potionType == leftNeighbourPotionType && neighbourPotion.potionType == upNeighbourPotionType && neighbourPotion.potionType == leftupNeighbourPotionType)
                        {
                            isCheckedSquare = true;
                        }
                    }

                    x += 1;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }

        }
    }

    void CheckVerticalMatch(Potion pot, List<Potion> connectedPotions)
    {
        PotionType potionType = pot.potionType;
        int x = pot.xIndex;
        int y = pot.yIndex + 1;

        // check that we're within the boundaries of the board
        while (y >= 0 && y < board.height)
        {
            if (board.potionBoard[x, y].isUsable)
            {
                Potion neighbourPotion = board.potionBoard[x, y].potion.GetComponent<Potion>();

                // does our potionType Match? it must also not be matched
                if (!neighbourPotion.isMatched && neighbourPotion.potionType == potionType)
                {
                    connectedPotions.Add(neighbourPotion);

                    // �׸� �߰� üũ
                    // ���� üũ���� ������ �׸� �߰��ϰ� �Ǿ� ����
                    //if (!isCheckedSquare && x < width - 1 && potionBoard[x + 1, y].isUsable && potionBoard[x, y - 1].isUsable && potionBoard[x + 1, y - 1].isUsable)
                    //{
                    //    PotionType rightNeighbourPotionType = potionBoard[x + 1, y].potion.GetComponent<Potion>().potionType;
                    //    PotionType downNeighbourPotionType = potionBoard[x, y - 1].potion.GetComponent<Potion>().potionType;
                    //    PotionType rightdownNeighbourPotionType = potionBoard[x + 1, y - 1].potion.GetComponent<Potion>().potionType;

                    //    if (neighbourPotion.potionType == rightNeighbourPotionType && neighbourPotion.potionType == downNeighbourPotionType && neighbourPotion.potionType == rightdownNeighbourPotionType)
                    //    {
                    //        Debug.Log("�׸� üũ��");
                    //        isCheckedSquare = true;
                    //    }
                    //}

                    y += 1;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }

        }
    }

    // ����, ���� ����(direction) ��Ī üũ
    // ���� üũ ��� -> �׸� üũ ������ ���� ���� ������� 
    // Super ��Ī�� �ش� �޼��� ������̶� �켱 ���ܳ���
    // �Ű� ������ �� direction �������� ������ ��� üũ��
    void CheckDirection(Potion pot, Vector2Int direction, List<Potion> connectedPotions)
    {
        PotionType potionType = pot.potionType;
        int x = pot.xIndex + direction.x;
        int y = pot.yIndex + direction.y;

        // check that we're within the boundaries of the board
        while (x >= 0 && x < board.width && y >= 0 && y < board.height)
        {
            if (board.potionBoard[x, y].isUsable)
            {
                Potion neighbourPotion = board.potionBoard[x, y].potion.GetComponent<Potion>();

                // does our potionType Match? it must also not be matched
                if (!neighbourPotion.isMatched && neighbourPotion.potionType == potionType)
                {
                    connectedPotions.Add(neighbourPotion);

                    x += direction.x;
                    y += direction.y;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }

        }
    }

    public List<Potion> RunSpecialBlock(Potion _potion, Potion _anotherPotion)
    {
        switch (_potion.potionType)
        {
            case PotionType.DrillVertical :
                Debug.Log("�帱(����) ��� �ߵ�");
                return GetColumnPieces(_potion.xIndex);

            case PotionType.DrillHorizontal:
                Debug.Log("�帱(����) ��� �ߵ�");
                return GetRowPieces(_potion.yIndex);

            case PotionType.Bomb:
                Debug.Log("��ź ��� �ߵ�");
                return Get2DistancePieces(_potion.xIndex, _potion.yIndex);

            case PotionType.Prism:
                Debug.Log("������ ��� �ߵ�");
                return MatchPiecesOfColor(_anotherPotion.potionType);

                // TODO : 1. ��� ���ľ���
                //  �ӽ÷� �밢��(������) ��� �ߵ�
            case PotionType.Pick:
                Debug.Log("��� ��� �ߵ�");
                return GetDiagonalPieces(_potion.xIndex, _potion.yIndex);
        }

        return null;
    }

    // �帱(����) ���
    List<Potion> GetColumnPieces(int _xIndex)
    {
        List<Potion> blocks = new List<Potion>();

        for (int i = 0; i < board.height; i++)
        {
            if (board.potionBoard[_xIndex, i].isUsable)
            {
                //Potion potion = board.potionBoard[_xIndex, i].potion.GetComponent<Potion>();
                if (!board.potionBoard[_xIndex,i].potion.GetComponent<Potion>().isMatched)
                {
                    blocks.Add(board.potionBoard[_xIndex, i].potion.GetComponent<Potion>());
                    //board.potionBoard[_xIndex, i].potion.GetComponent<Potion>().isMatched = true;
                }
            }
        }

        return blocks;
    }


    // �帱(����) ���
    List<Potion> GetRowPieces(int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();
        
        for (int i=0; i<board.width; i++)
        {
            if (board.potionBoard[i, _yIndex].isUsable)
            {
                //Potion potion = board.potionBoard[i, _yIndex].potion.GetComponent<Potion>();
                if (!board.potionBoard[i, _yIndex].potion.GetComponent<Potion>().isMatched)
                {
                    blocks.Add(board.potionBoard[i, _yIndex].potion.GetComponent<Potion>());
                    //board.potionBoard[i, _yIndex].potion.GetComponent<Potion>().isMatched = true;
                }
            }
        }

        return blocks;
    }

    // ��ź ���
    // TODO : 1. �ֺ� 2ĭ �Ÿ� 1 3 5 3 1 üũ�� �ٲ���� v
    //        -> ���� �׽�Ʈ �ʿ�
    List<Potion> Get2DistancePieces(int _xIndex, int  _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        // 3x3 ��� dot �������� ���� �Ʒ� ������ ������ �� �������� ����
        // TODO : 1. �ֺ� 2ĭ �Ÿ� 1 3 5 3 1 üũ�� �ٲ����
        for (int i = _xIndex - 1; i <= _xIndex + 1; i++)
        {
            for (int j = _yIndex - 1; j <= _yIndex + 1; j++)
            {
                // Check if the piece is inside the board �𼭸� üũ
                if (i >= 0 && i < board.width && j >= 0 && j < board.height && board.potionBoard[i, j] != null && !board.potionBoard[i, j].potion.GetComponent<Potion>().isMatched)
                {
                    blocks.Add(board.potionBoard[i, j].potion.GetComponent<Potion>());
                    //board.potionBoard[i, j].potion.GetComponent<Potion>().isMatched = true;
                }
            }
        }

        if (_xIndex >= 2)
        {
            blocks.Add(board.potionBoard[_xIndex - 2, _yIndex].potion.GetComponent<Potion>());
        }
        if (_xIndex < board.width - 2)
        {
            blocks.Add(board.potionBoard[_xIndex + 2, _yIndex].potion.GetComponent<Potion>());

        }
        if (_yIndex >= 2)
        {
            blocks.Add(board.potionBoard[_xIndex, _yIndex - 2].potion.GetComponent<Potion>());
        }
        if (_yIndex < board.height - 2)
        {
            blocks.Add(board.potionBoard[_xIndex, _yIndex + 2].potion.GetComponent<Potion>());
        }

        return blocks;
    }

    // ��� �밢(������) ���
    List<Potion> GetDiagonalPieces(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        int index = 0;

        while (_xIndex - index >= 0 && _yIndex - index >= 0)
        {
            if (board.potionBoard[_xIndex - index, _yIndex - index] != null && !board.potionBoard[_xIndex - index, _yIndex - index].potion.GetComponent<Potion>().isMatched)
            {
                blocks.Add(board.potionBoard[_xIndex - index, _yIndex - index].potion.GetComponent<Potion>());
            }
            index++;
        }

        index = 0;

        while (_xIndex + index < board.width && _yIndex + index < board.height)
        {
            if (board.potionBoard[_xIndex + index, _yIndex + index] != null && !board.potionBoard[_xIndex + index, _yIndex + index].potion.GetComponent<Potion>().isMatched)
            {
                blocks.Add(board.potionBoard[_xIndex + index, _yIndex + index].potion.GetComponent<Potion>());
            }
            index++;
        }

        return blocks;
    }

    // ��� ���밢(����) ���
    List<Potion> GetReverseDiagonalPieces(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        int index = 0;

        while (_xIndex - index >= 0 && _yIndex + index < board.height)
        {
            if (board.potionBoard[_xIndex - index, _yIndex + index] != null && !board.potionBoard[_xIndex - index, _yIndex + index].potion.GetComponent<Potion>().isMatched)
            {
                blocks.Add(board.potionBoard[_xIndex - index, _yIndex + index].potion.GetComponent<Potion>());
            }
            index++;
        }

        index = 0;

        while (_xIndex + index < board.width && _yIndex - index >= 0)
        {
            if (board.potionBoard[_xIndex + index, _yIndex - index] != null && !board.potionBoard[_xIndex + index, _yIndex - index].potion.GetComponent<Potion>().isMatched)
            {
                blocks.Add(board.potionBoard[_xIndex + index, _yIndex - index].potion.GetComponent<Potion>());
            }
            index++;
        }

        return blocks;
    }

    // ������ ���
    List<Potion> MatchPiecesOfColor(PotionType _targetPotionType)
    {
        List<Potion> blocks = new List<Potion>();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                // Check if that piece exists
                if (board.potionBoard[i, j] != null && !board.potionBoard[i, j].potion.GetComponent<Potion>().isMatched)
                {
                    // Check the tag on that dot
                    if (board.potionBoard[i, j].potion.GetComponent<Potion>().potionType == _targetPotionType)
                    {
                        // Set that dot to be matched
                        blocks.Add(board.potionBoard[i, j].potion.GetComponent<Potion>());
                        //board.potionBoard[i, j].potion.GetComponent<Potion>().isMatched = true;
                    }
                }
            }
        }

        return blocks;
    }

}