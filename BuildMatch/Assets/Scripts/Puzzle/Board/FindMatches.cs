using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private PotionBoard board;

    // �������� �� ���⿡ �����ؼ� ����
    public List<Potion> potionsToRemove = new();

    void Awake()
    {
        board = FindObjectOfType<PotionBoard>();
    }

    private void Update()
    {

    }

    // �׽�Ʈ��
    public bool TestFindSpecialMatches()
    {
        if (PuzzleManager.Instance.isStageEnded)
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
        //Potion targetPotion = board.targetedPotion;

        // Ư���� üũ
        // ����, Ÿ�ϵ� ������ �ִ� ���(���ʷ� �������� ���� ��) Ư�� �� üũ �� �� �޼��� �� �Ʒ����� board�� ���� currentPotion, targetPotion ����
        if (currentPotion != null)
        {
            // test here
            potionsToRemove.AddRange(Get3DiagonalPieces(currentPotion.xIndex, currentPotion.yIndex));
            hasMatched = true;
        }

        // Ư���� üũ������ ���⿡�� Ư���� üũ �� ����, Ÿ�� �� ���� ����
        // ������ ��쿡�� Ư���� �ִ��� üũ
        board.selectedPotion = null;

        return hasMatched;
    }

    // ���ڸ� Ŭ�� �� Ư���� ȿ�� ó��
    public bool FindSpecialMatches()
    {
        if (PuzzleManager.Instance.isStageEnded)
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

        // Ư���� üũ
        // ����, Ÿ�ϵ� ������ �ִ� ���(���ʷ� �������� ���� ��) Ư�� �� üũ �� �� �޼��� �� �Ʒ����� board�� ���� currentPotion ����
        if (currentPotion != null)
        {
            if (IsSpecialBlock(currentPotion.potionType))
            {
                potionsToRemove.AddRange(RunSpecialBlock(currentPotion));
                hasMatched = true;
            }
        }

        // Ư���� üũ������ ���⿡�� Ư���� üũ �� ����, Ÿ�� �� ���� ����
        // ������ ��쿡�� Ư���� �ִ��� üũ
        board.selectedPotion = null;

        return hasMatched;
    }

    // ���忡 ��Ī�Ǿ� �ִ°� �ִ��� üũ
    // TODO : 1. üũ �� ��Ī ����� ���� ���� ��� �ٽ� ������ ��
    //        2. ���� �ð��� ������ ������ ���� ��� ��Ī�Ǵ� �� ǥ��
    public bool FindAllMatches()
    {
        if (PuzzleManager.Instance.isStageEnded)
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
            // �� �� Ư������ �� ȿ�� ����
            if (IsSpecialBlock(currentPotion.potionType) && IsSpecialBlock(targetPotion.potionType))
            {
                List<Potion> blocks = RunCombineSpecialBlocks(currentPotion, targetPotion);
                if (blocks != null)
                {
                    potionsToRemove.AddRange(blocks);
                    hasMatched = true;
                }
            }

            // ���õ� ���� Ư������ ���
            if (IsSpecialBlock(currentPotion.potionType) && !IsSpecialBlock(targetPotion.potionType))
            {
                potionsToRemove.AddRange(RunSpecialBlock(currentPotion, targetPotion));
                hasMatched = true;
            }

            // Ÿ�� ���� Ư������ ���
            if (IsSpecialBlock(targetPotion.potionType) && !IsSpecialBlock(currentPotion.potionType))
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

                        // TODO : 1. 
                        //         -> IsConnected ��ġ ����� ���� SuperMatch ���� ���� ����
                        //Vertical_3, // 3 ����
                        //Horizontal_3, // 3 ����
                        //Vertical_4, // 4 ���� : �帱(����)
                        //Horizontal_4, // 4 ���� : �帱(����)
                        //LongVertical, // 5 �̻� ���� : ������
                        //LongHorizontal, // 5 �̻� ���� -> ������
                        //Super, // ���� ���� ���ļ� �۵��� -> 5�迭 L�� �߰� ���� ���� : ��ź
                        //Square, // 4�迭 �׸� : ���(�밢), ���(���밢)
                        //None

                        if (matchedPotions.direction != MatchDirection.None)
                        {
                            // MatchDirection Vertical_3, Horizontal_3, Vertical_4, Horizontal_4 �� ��� SuperMatch���� üũ
                            MatchResult superMatchedPotions = FindSuperMatch(matchedPotions);

                            potionsToRemove.AddRange(superMatchedPotions.connectedPotions);

                            foreach (Potion pot in superMatchedPotions.connectedPotions)
                            {
                                pot.isMatched = true;

                                // MatchDirection�� ���� �ٲ� Ư���� ���� Potion�� ����
                                if (pot.isChangedBlock)
                                {
                                    pot.SetChangedSpecialBlockType(superMatchedPotions.direction);

                                    // �ΰ� �̻� �� �������� �� �ߺ� Ư���� ���� ������ isChangedBlock false�� ����
                                    foreach (Potion pot2 in superMatchedPotions.connectedPotions)
                                    {
                                        pot2.isChangedBlock = false;
                                    }
                                }
                            }

                            hasMatched = true;
                        }
                    }

                    // ��Ī üũ �� �� ���ŷ� ���� üũ�� isChangedBlock false�� ����
                    potion.isChangedBlock = false;
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
        // TODO : 1. ���� �켱���� ����
        if (_matchedResults.direction == MatchDirection.Horizontal_3 || _matchedResults.direction == MatchDirection.Horizontal_4 || _matchedResults.direction == MatchDirection.LongHorizontal)
        {
            foreach (Potion pot in _matchedResults.connectedPotions)
            {
                List<Potion> extraConnectedPotions = new();

                CheckSuperDirection(pot, new Vector2Int(0, 1), extraConnectedPotions);
                CheckSuperDirection(pot, new Vector2Int(0, -1), extraConnectedPotions);

                if (extraConnectedPotions.Count >= 2)
                {
                    //isCheckedSuper = true;
                    // ��ź ����

                    extraConnectedPotions.AddRange(_matchedResults.connectedPotions);

                    return new MatchResult
                    {
                        connectedPotions = extraConnectedPotions,
                        direction = _matchedResults.direction == MatchDirection.LongHorizontal ? MatchDirection.LongHorizontal : MatchDirection.Super
                    };
                }
            }
            return new MatchResult
            {
                connectedPotions = _matchedResults.connectedPotions,
                direction = _matchedResults.direction
            };
        }

        if (_matchedResults.direction == MatchDirection.Vertical_3 || _matchedResults.direction == MatchDirection.Vertical_4 || _matchedResults.direction == MatchDirection.LongVertical)
        {
            foreach (Potion pot in _matchedResults.connectedPotions)
            {
                List<Potion> extraConnectedPotions = new();

                CheckSuperDirection(pot, new Vector2Int(1, 0), extraConnectedPotions);
                CheckSuperDirection(pot, new Vector2Int(-1, 0), extraConnectedPotions);

                if (extraConnectedPotions.Count >= 2)
                {
                    //isCheckedSuper = true;
                    extraConnectedPotions.AddRange(_matchedResults.connectedPotions);

                    return new MatchResult
                    {
                        connectedPotions = extraConnectedPotions,
                        direction = _matchedResults.direction == MatchDirection.LongVertical ? MatchDirection.LongVertical : MatchDirection.Super
                    };
                }
            }
            return new MatchResult
            {
                connectedPotions = _matchedResults.connectedPotions,
                direction = _matchedResults.direction
            };
        }

        // _matchedResult.direction == MatchDirection.Square�� ��� �״�� �ѱ�
        return new MatchResult
        {
            connectedPotions = _matchedResults.connectedPotions,
            direction = _matchedResults.direction
        };
    }

    // �� Ÿ���� ��ġ�ϴ��� Ȯ�� �� Match ��� ��ȯ
    // ���� IsConnected������ Ư�� �� ���������� �� ��츦 üũ�ؼ� ��ȯ�ߴµ�
    // 3�� �̻� �ִ� ��� ���� �ʱ�ȭ�ǹǷ� �����ϰ� ó���ϵ��� ���� �Լ� �������
    public MatchResult IsInitializeConnected(Potion potion)
    {
        List<Potion> connectedPotions = new()
        {
            potion
        };

        // ���� üũ
        CheckHorizontalMatch(potion, connectedPotions);

        if (connectedPotions.Count >= 3)
        {
            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Horizontal_3
            };
        }

        connectedPotions.Clear();

        connectedPotions.Add(potion);

        // ���� üũ
        CheckVerticalMatch(potion, connectedPotions);

        if (connectedPotions.Count >= 3)
        {
            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Vertical_3
            };
        }

        connectedPotions.Clear();

        connectedPotions.Add(potion);

        // 4 �̻� �׸� : ��� üũ
        CheckSquareMatch(potion, connectedPotions);

        if (connectedPotions.Count >= 4)
        {
            //isCheckedSquare = true;

            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Square
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

    // �� Ÿ���� ��ġ�ϴ��� Ȯ�� �� Match ��� ��ȯ
    public MatchResult IsConnected(Potion potion)
    {
        List<Potion> connectedPotions = new()
        {
            potion
        };

        // �켱 ���� ���� :
        // 1. 5�� �̻� ����, ����(������)
        // 2. L��, T��(��ź) -> �� �޼��� ����� üũ
        // 3. 4�� �̻� ����, ����(�帱)
        // 4. �׸�(���) -> 2�� �̻� ������ ��� üũ
        // 5. 3�� �̻� ����, ����

        // ���� üũ
        CheckHorizontalMatch(potion, connectedPotions);

        // 1. 5 �̻� ���� ��ġ : ������
        if (connectedPotions.Count >= 5)
        {
            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.LongHorizontal
            };
        }

        // 3. 4 ���� ��ġ : �帱 ����

        else if (connectedPotions.Count == 4)
        {
            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Horizontal_4
            };
        }

        // 4. �׸�

        else if (connectedPotions.Count >= 2)
        {
            List<Potion> newConnectedPotions = new()
            {
                potion
            };

            // 4 �̻� �׸� : ���
            CheckSquareMatch(potion, newConnectedPotions);

            if (newConnectedPotions.Count >= 4)
            {
                return new MatchResult
                {
                    connectedPotions = newConnectedPotions,
                    direction = MatchDirection.Square
                };
            } 

            // 5. 3 ���� ��ġ
            if (connectedPotions.Count == 3)
            {
                // XOO
                // OOO �� ��� �Ʒ��� ����� -> ����ó��

                return new MatchResult
                {
                    connectedPotions = connectedPotions,
                    direction = MatchDirection.Horizontal_3
                };
            }
        }

        // 5. 3 ���� ��ġ
        //else if (connectedPotions.Count == 3)
        //{
        //    // 3���϶� ��� üũ ���� �ؾ���
        //    return new MatchResult
        //    {
        //        connectedPotions = connectedPotions,
        //        direction = MatchDirection.Horizontal_3
        //    };

        //}

        connectedPotions.Clear();

        connectedPotions.Add(potion);

        // ���� üũ
        CheckVerticalMatch(potion, connectedPotions);

        // �켱 ���� ����
        // 1. 5 �̻� ���� ��ġ : ������
        if (connectedPotions.Count >= 5)
        {
            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.LongVertical
            };
        }
        // 2. 4 ���� ��ġ : �帱 ����
        else if (connectedPotions.Count == 4)
        {
            return new MatchResult
            {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Vertical_4
            };
        }
        // 4. �׸� üũ
        else if (connectedPotions.Count >= 2)
        {
            List<Potion> newConnectedPotions = new()
            {
                potion
            };

            // 4 �̻� �׸� : ���
            CheckSquareMatch(potion, newConnectedPotions);

            if (newConnectedPotions.Count >= 4)
            {
                return new MatchResult
                {
                    connectedPotions = newConnectedPotions,
                    direction = MatchDirection.Square
                };
            }

            // 5. 3 ���� ��ġ
            if (connectedPotions.Count == 3)
            {
                // OO
                // OO
                // OX�� ��� ���ʸ� ����� -> ����ó��
                return new MatchResult
                {
                    connectedPotions = connectedPotions,
                    direction = MatchDirection.Vertical_3
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

        // 5. 3 ���� ��ġ
        //if (connectedPotions.Count == 3)
        //{
        //    return new MatchResult
        //    {
        //        connectedPotions = connectedPotions,
        //        direction = MatchDirection.Vertical_3
        //    };
        //}

        //// �׸� üũ
        //connectedPotions.Clear();

        //connectedPotions.Add(potion);

        //CheckSquareMatch(potion, connectedPotions);

        //if (connectedPotions.Count >= 4)
        //{
        //    return new MatchResult
        //    {
        //        connectedPotions = connectedPotions,
        //        direction = MatchDirection.Square
        //    };
        //}

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
            if (IsUsableAndNotBeforeMatched(x, y))
            {
                Potion neighbourPotion = board.potionBoard[x, y].potion.GetComponent<Potion>();

                // does our potionType Match? it must also not be matched
                if (neighbourPotion.potionType == potionType)
                {
                    connectedPotions.Add(neighbourPotion);
                    //neighbourPotion.isMatched = true;

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
            if (IsUsableAndNotBeforeMatched(x, y))
            {
                Potion neighbourPotion = board.potionBoard[x, y].potion.GetComponent<Potion>();

                // does our potionType Match? it must also not be matched
                if (neighbourPotion.potionType == potionType)
                {
                    connectedPotions.Add(neighbourPotion);
                    //neighbourPotion.isMatched = true;

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

    // TODO : 1. XOO
    //           OOO ��ϰ��� ��� ���� column���� üũ�� �ϱ� ������ 3��ġ ������ ó���Ǵ���..
    void CheckSquareMatch(Potion pot, List<Potion> connectedPotions)
    {
        PotionType potionType = pot.potionType;
        int x = pot.xIndex;
        int y = pot.yIndex;

        if (x < board.width - 1 && y < board.height - 1 && IsUsableAndNotBeforeMatched(x + 1, y))
        {
            Potion rightneighbourPotion = board.potionBoard[x + 1, y].potion.GetComponent<Potion>();

            if (!rightneighbourPotion.isMatched && rightneighbourPotion.potionType == potionType)
            {
                // ��, ������ �� �� ������ üũ
                if (IsUsableAndNotBeforeMatched(x, y + 1) && IsUsableAndNotBeforeMatched(x + 1, y + 1))
                {
                    Potion upNeighbourPotion = board.potionBoard[x, y + 1].potion.GetComponent<Potion>();
                    Potion rightupNeighbourPotion = board.potionBoard[x + 1, y + 1].potion.GetComponent<Potion>();

                    if (rightneighbourPotion.potionType == upNeighbourPotion.potionType && rightneighbourPotion.potionType == rightupNeighbourPotion.potionType)
                    {
                        connectedPotions.Add(rightneighbourPotion);
                        connectedPotions.Add(upNeighbourPotion);
                        connectedPotions.Add(rightupNeighbourPotion);
                        //rightneighbourPotion.isMatched = true;
                        //upNeighbourPotion.isMatched = true;
                        //rightupNeighbourPotion.isMatched = true;

                        // �簢���� �߰��ϸ� �߰� ���� üũ = �߰� ������ OtherPotion���� �ϴ� �������

                        // �Ʒ� ����
                        if (y > 0 && IsUsableAndNotBeforeMatched(x, y - 1) && board.potionBoard[x, y - 1].potion.GetComponent<Potion>().potionType == potionType)
                        {
                            Potion leftdownOtherPotion = board.potionBoard[x, y - 1].potion.GetComponent<Potion>();

                            connectedPotions.Add(leftdownOtherPotion);
                            //leftdownOtherPotion.isMatched = true;
                        }

                        // �Ʒ� ������
                        if (y > 0 && IsUsableAndNotBeforeMatched(x + 1, y - 1) && board.potionBoard[x + 1, y - 1].potion.GetComponent<Potion>().potionType == potionType)
                        {
                            Potion rightdownOtherPotion = board.potionBoard[x + 1, y - 1].potion.GetComponent<Potion>();

                            connectedPotions.Add(rightdownOtherPotion);
                            //rightdownOtherPotion.isMatched = true;
                        }

                        // ����
                        if (x > 0 && IsUsableAndNotBeforeMatched(x - 1, y) && board.potionBoard[x - 1, y].potion.GetComponent<Potion>().potionType == potionType)
                        {
                            Potion leftOtherPotion = board.potionBoard[x - 1, y].potion.GetComponent<Potion>();

                            connectedPotions.Add(leftOtherPotion);
                            //leftOtherPotion.isMatched = true;
                        }

                        // ���� ��
                        if (x > 0 && IsUsableAndNotBeforeMatched(x - 1, y + 1) && board.potionBoard[x - 1, y + 1].potion.GetComponent<Potion>().potionType == potionType)
                        {
                            Potion leftupOtherPotion = board.potionBoard[x - 1, y + 1].potion.GetComponent<Potion>();

                            connectedPotions.Add(leftupOtherPotion);
                            //leftupOtherPotion.isMatched = true;
                        }

                        // �� �� ����
                        if (y < board.height - 2 && IsUsableAndNotBeforeMatched(x, y + 2) && board.potionBoard[x, y + 2].potion.GetComponent<Potion>().potionType == potionType)
                        {
                            Potion farupleftOtherPotion = board.potionBoard[x, y + 2].potion.GetComponent<Potion>();

                            connectedPotions.Add(farupleftOtherPotion);
                            //farupleftOtherPotion.isMatched = true;
                        }

                        // �� �� ������
                        if (y < board.height - 2 && IsUsableAndNotBeforeMatched(x + 1, y + 2) && board.potionBoard[x + 1, y + 2].potion.GetComponent<Potion>().potionType == potionType)
                        {
                            Potion faruprightOtherPotion = board.potionBoard[x + 1, y + 2].potion.GetComponent<Potion>();

                            connectedPotions.Add(faruprightOtherPotion);
                            //faruprightOtherPotion.isMatched = true;
                        }

                        // �� ������
                        if (x < board.width - 2 && IsUsableAndNotBeforeMatched(x + 2, y) && board.potionBoard[x + 2, y].potion.GetComponent<Potion>().potionType == potionType)
                        {
                            Potion farrightOtherPotion = board.potionBoard[x + 2, y].potion.GetComponent<Potion>();

                            connectedPotions.Add(farrightOtherPotion);
                            //farrightOtherPotion.isMatched = true;
                        }

                        // �� ������ ��
                        if (x < board.width - 2 && IsUsableAndNotBeforeMatched(x + 2, y + 1) && board.potionBoard[x + 2, y + 1].potion.GetComponent<Potion>().potionType == potionType)
                        {
                            Potion farrightupOtherPotion = board.potionBoard[x + 2, y + 1].potion.GetComponent<Potion>();

                            connectedPotions.Add(farrightupOtherPotion);
                            //farrightupOtherPotion.isMatched = true;
                        }

                    }
                }
            }
        } 
    }

    void CheckLine3SquareMatch()
    {

    }

    // ����, ���� ����(direction) Super ��Ī(L��)�ǳ� üũ
    // �Ű� ������ �� direction �������� ������ ��� üũ��
    void CheckSuperDirection(Potion pot, Vector2Int direction, List<Potion> connectedPotions)
    {
        PotionType potionType = pot.potionType;
        int x = pot.xIndex + direction.x;
        int y = pot.yIndex + direction.y;

        // check that we're within the boundaries of the board
        while (x >= 0 && x < board.width && y >= 0 && y < board.height)
        {
            if (IsUsableAndNotBeforeMatched(x, y))
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

    // ���ڸ� Ư�� �� ���
    public List<Potion> RunSpecialBlock(Potion _potion)
    {
        switch (_potion.potionType)
        {
            case PotionType.DrillVertical:
                return GetColumnPieces(_potion.xIndex);

            case PotionType.DrillHorizontal:
                return GetRowPieces(_potion.yIndex);

            case PotionType.Bomb:
                return Get2DistancePieces(_potion.xIndex, _potion.yIndex);

            case PotionType.Prism:
                return MatchPiecesOfColor(_potion.xIndex, _potion.yIndex);

            case PotionType.PickLeft:
                return GetReverseDiagonalPieces(_potion.xIndex, _potion.yIndex);

            case PotionType.PickRight:
                return GetDiagonalPieces(_potion.xIndex, _potion.yIndex);
        }

        return null;
    }

    // �������� Ư�� �� ���(�����򶧹��� ù��° �Ű������� selectedPotion�ϼ��� �ְ� targetPotion�ϼ��� ����)
    public List<Potion> RunSpecialBlock(Potion _potion, Potion _anotherPotion)
    {
        switch (_potion.potionType)
        {
            case PotionType.DrillVertical :
                return GetColumnPieces(_potion.xIndex);

            case PotionType.DrillHorizontal:
                return GetRowPieces(_potion.yIndex);

            case PotionType.Bomb:
                return Get2DistancePieces(_potion.xIndex, _potion.yIndex);

            case PotionType.Prism:
                return MatchPiecesOfColor(_potion.xIndex, _potion.yIndex, _anotherPotion.potionType);

            case PotionType.PickLeft:
                return GetReverseDiagonalPieces(_potion.xIndex, _potion.yIndex);

            case PotionType.PickRight:
                return GetDiagonalPieces(_potion.xIndex, _potion.yIndex);
        }

        return null;
    }

    // currentPotion, targetPotion �Ѵ� Ư�� ���� �� �ߵ�
    public List<Potion> RunCombineSpecialBlocks(Potion _selectedPotion, Potion _targetedPotion)
    {
        switch (_selectedPotion.potionType)
        {
            // �帱(����) ���
            // ���� ȿ��
            // 1. + �帱(����, ���� �������) : ���յ� �κп��� ���� ���⿡ �ִ� ��� �� ����
            // 2. + ��� : ���յ� ��� �� ��翡 ���� ������ ��� �� ����(��ȹ�� P63 ����)
            // 3. + ������ : �ʵ忡 �����ϴ� �� �� ���� ���� ���� ���� ���յ� �帱�� �ٲ�
            case PotionType.DrillVertical:
                switch (_targetedPotion.potionType)
                {
                    case PotionType.DrillVertical:
                    case PotionType.DrillHorizontal:
                        return GetColumnAndRowPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                    case PotionType.PickLeft:
                        return GetRowAndDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.PickRight:
                        return GetRowAndReverseDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.Prism:
                        SwitchDrillVertical();
                        return GetCurrentPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                }

                // �� ��찡 �ƴ� �� ó�� -> �ȵǸ� if��
                return null;

            // �帱(����) ���
            // ���� ȿ��
            // 1. + �帱(����, ���� �������) : ���յ� �κп��� ���� ���⿡ �ִ� ��� �� ����
            // 2. + ��� : ���յ� ��� �� ��翡 ���� ������ ��� �� ����(��ȹ�� P63 ����)
            // 3. + ������ : �ʵ忡 �����ϴ� �� �� ���� ���� ���� ���� ���յ� �帱�� �ٲ�
            case PotionType.DrillHorizontal:
                switch (_targetedPotion.potionType)
                {
                    case PotionType.DrillVertical:
                    case PotionType.DrillHorizontal:
                        return GetColumnAndRowPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                    case PotionType.PickLeft:
                        return GetColumnAndDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.PickRight:
                        return GetColumnAndReverseDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.Prism:
                        SwitchDrillHorizontal();
                        return GetCurrentPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                }

                // �� ��찡 �ƴ� �� ó�� -> �ȵǸ� if��
                return null;

            // ��ź ���
            // ���� ȿ��
            // 1. + ��ź : �����Ͽ� ���յ� �κп��� 3ĭ ������ ��� �� ����
            // 2. + �帱 : �帱 ���� ���� ���� or �����ٿ� �ִ� ��� ���� 3ĭ �ʺ�� ����
            // 3. + ��� : ��� ���� ���� �밢 or ���밢�ٿ� �ִ� ��� ���� 3ĭ �ʺ�� ����
            // 4. + ������ : �ʵ忡 �����ϴ� �� �� ���� ���� ���� ���� ��ź���� �ٲ�
            //               �ٲ� ��ź ȿ���� ���� ������ �߻�
            case PotionType.Bomb:
                switch (_targetedPotion.potionType)
                {
                    case PotionType.Bomb:
                        return Get3DistancePieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                    case PotionType.DrillVertical:
                        return Get3ColumnPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                    case PotionType.DrillHorizontal:
                        return Get3RowPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                    case PotionType.PickLeft:
                        return Get3DiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                    case PotionType.PickRight:
                        // ������
                        return Get3ReverseDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                    case PotionType.Prism:
                        SwitchBomb();
                        return GetCurrentPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                }

                // �� ��찡 �ƴ� �� ó�� -> �ȵǸ� if��
                return null;
            // ���� ȿ��
            // 1. + ������ : ȭ�鿡 ���̴� ��� ���� ����
            case PotionType.Prism:
                switch (_targetedPotion.potionType)
                {
                    case PotionType.Prism:
                        return GetAllPieces();
                }

                // �� ��찡 �ƴ� �� ó�� -> �ȵǸ� if��
                return null;
            // ��� ���밢(����) ���
            // ���� ȿ��
            // 1. + �帱 : ���յ� �帱 �� ��翡 ���� ������ ��� �� ����(��ȹ�� P63 ����)
            // 2. + ���(�밢, ���밢 �������) : X ���� �� ����
            // 3. + ������ : �ʵ忡 �����ϴ� �� �� ���� ���� ���� ���� ���յ� ��̷� �ٲ�
            case PotionType.PickLeft:
                switch (_targetedPotion.potionType)
                {
                    case PotionType.DrillHorizontal:
                        return GetRowAndReverseDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.DrillVertical:
                        return GetColumnAndReverseDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.PickLeft:
                    case PotionType.PickRight:
                        return GetDiagonalAndReverseDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.Prism:
                        SwitchPickLeft();
                        return GetCurrentPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                }

                // �� ��찡 �ƴ� �� ó�� -> �ȵǸ� if��
                return null;
            // ��� �밢(������) ���
            // ���� ȿ��
            // 1. + �帱 : ���յ� �帱 �� ��翡 ���� ������ ��� �� ����(��ȹ�� P63 ����)
            // 2. + ���(�밢, ���밢 �������) : X ���� �� ����
            // 3. + ������ : �ʵ忡 �����ϴ� �� �� ���� ���� ���� ���� ���յ� ��̷� �ٲ�
            case PotionType.PickRight:
                switch (_targetedPotion.potionType)
                {
                    case PotionType.DrillHorizontal:
                        return GetRowAndDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.DrillVertical:
                        return GetColumnAndDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.PickLeft:
                    case PotionType.PickRight:
                        return GetDiagonalAndReverseDiagonalPieces(_selectedPotion.xIndex, _selectedPotion.yIndex, _targetedPotion.xIndex, _targetedPotion.yIndex);
                    case PotionType.Prism:
                        SwitchPickRight();
                        return GetCurrentPieces(_selectedPotion.xIndex, _selectedPotion.yIndex);
                }

                // �� ��찡 �ƴ� �� ó�� -> �ȵǸ� if��
                return null;
        }

        return null;
    }

    // �帱(����) ���
    List<Potion> GetColumnPieces(int _xIndex)
    {
        List<Potion> blocks = new List<Potion>();

        for (int i = 0; i < board.height; i++)
        {
            if (IsUsableAndNotBeforeMatched(_xIndex, i))
            {
                CheckSpecialChainMatch(_xIndex, i, blocks);
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
            if (IsUsableAndNotBeforeMatched(i, _yIndex))
            {
                CheckSpecialChainMatch(i, _yIndex, blocks);
            }
        }

        return blocks;
    }

    // ��ź ���
    List<Potion> Get2DistancePieces(int _xIndex, int  _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        // 3x3 ��� dot �������� ���� �Ʒ� ������ ������ �� �������� ����
        for (int i = _xIndex - 1; i <= _xIndex + 1; i++)
        {
            for (int j = _yIndex - 1; j <= _yIndex + 1; j++)
            {

                // Check if the piece is inside the board �𼭸� üũ
                if (i >= 0 && i < board.width && j >= 0 && j < board.height && IsUsableAndNotBeforeMatched(i, j))
                {
                    CheckSpecialChainMatch(i, j, blocks);
                }
                
            }
        }

        if (_xIndex >= 2 && board.potionBoard[_xIndex - 2, _yIndex] != null && IsUsableAndNotBeforeMatched(_xIndex - 2, _yIndex))
        {
            CheckSpecialChainMatch(_xIndex - 2, _yIndex, blocks);
        }
        if (_xIndex < board.width - 2 && board.potionBoard[_xIndex + 2, _yIndex] != null && IsUsableAndNotBeforeMatched(_xIndex + 2, _yIndex))
        {
            CheckSpecialChainMatch(_xIndex + 2, _yIndex, blocks);
        }
        if (_yIndex >= 2 && board.potionBoard[_xIndex, _yIndex - 2] != null && IsUsableAndNotBeforeMatched(_xIndex, _yIndex - 2))
        {
            CheckSpecialChainMatch(_xIndex, _yIndex - 2, blocks);
        }
        if (_yIndex < board.height - 2 && board.potionBoard[_xIndex, _yIndex + 2] != null && IsUsableAndNotBeforeMatched(_xIndex, _yIndex + 2))
        {
            CheckSpecialChainMatch(_xIndex, _yIndex + 2, blocks);
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
            if (_xIndex - index < board.width && IsUsableAndNotBeforeMatched(_xIndex - index, _yIndex - index))
            {
                CheckSpecialChainMatch(_xIndex - index, _yIndex - index, blocks);
            }
            index++;
        }

        index = 0;

        while (_xIndex + index < board.width && _yIndex + index < board.height)
        {
            if (_xIndex + index >= 0 && IsUsableAndNotBeforeMatched(_xIndex + index, _yIndex + index))
            {
                CheckSpecialChainMatch(_xIndex + index, _yIndex + index, blocks);
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
            if (_xIndex - index < board.width && IsUsableAndNotBeforeMatched(_xIndex - index, _yIndex + index))
            {
                CheckSpecialChainMatch(_xIndex - index, _yIndex + index, blocks);
            }
            index++;
        }

        index = 0;

        while (_xIndex + index < board.width && _yIndex - index >= 0)
        {
            if (_xIndex + index >= 0 && IsUsableAndNotBeforeMatched(_xIndex + index, _yIndex - index))
            {
                CheckSpecialChainMatch(_xIndex + index, _yIndex - index, blocks);
            }
            index++;
        }

        return blocks;
    }

    // ���ڸ� ������ ���
    List<Potion> MatchPiecesOfColor(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        // Enumerable.Repeat<int>(0, board.normalBlockLength).ToArray<int>()
        // �����ϸ鼭 0���� length��ŭ �ʱ�ȭ
        // (�ʱ�ȭ�ϰ� ���� ��, ����)
        int[] blockCounts = Enumerable.Repeat<int>(0, board.stageNormalBlockLength).ToArray<int>();

        // ���� ���� ���� ã���ִ� 
        for (int i=0; i<board.width; i++)
        {
            for (int j=0; j<board.height; j++)
            {
                if (board.potionBoard[i, j] != null && IsUsableAndNotBeforeMatched(i, j))
                {
                    if (!IsSpecialBlock(board.potionBoard[i, j].potion.GetComponent<Potion>().potionType))
                    blockCounts[(int)board.potionBoard[i, j].potion.GetComponent<Potion>().potionType]++;
                }

            }
        }

        // ���� ���� ���� potionType ã��
        int maxIndex = 0;
        int max = 0;

        for (int i=0; i<blockCounts.Length; i++)
        {
            if (max < blockCounts[i])
            {
                max = blockCounts[i];
                maxIndex = i;
            }
        }

        PotionType _targetPotionType = (PotionType)maxIndex;

        // Prism �� �߰�
        if (board.potionBoard[_xIndex, _yIndex] != null && IsUsableAndNotBeforeMatched(_xIndex, _yIndex))
        {
            blocks.Add(board.potionBoard[_xIndex, _yIndex].potion.GetComponent<Potion>());
            //board.potionBoard[_xIndex, _yIndex].potion.GetComponent<Potion>().isMatched = true;
        }

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.potionBoard[i, j] != null && IsUsableAndNotBeforeMatched(i, j))
                {
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

    // ������ ���
    List<Potion> MatchPiecesOfColor(int _xIndex, int _yIndex, PotionType _targetPotionType)
    {
        List<Potion> blocks = new List<Potion>();

        // Prism �� �߰�
        if (board.potionBoard[_xIndex, _yIndex] != null && IsUsableAndNotBeforeMatched(_xIndex, _yIndex))
        {
            blocks.Add(board.potionBoard[_xIndex, _yIndex].potion.GetComponent<Potion>());
        }

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                // Check if that piece exists
                if (board.potionBoard[i, j] != null && IsUsableAndNotBeforeMatched(i, j))
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

    // Ư���� ���� ȿ��
    // �׽�Ʈ ���� : O
    public List<Potion> GetRowAndDiagonalPieces(int _xIndex, int _yIndex, int _targetXIndex, int _targetYIndex)
    {
        List<Potion> blocks = new List<Potion>();

        blocks.AddRange(GetRowPieces(_yIndex));
        blocks.AddRange(GetDiagonalPieces(_xIndex, _yIndex));
        blocks.AddRange(GetCurrentPieces(_targetXIndex, _targetYIndex));

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> GetRowAndReverseDiagonalPieces(int _xIndex, int _yIndex, int _targetXIndex, int _targetYIndex)
    {
        List<Potion> blocks = new List<Potion>();

        blocks.AddRange(GetRowPieces(_yIndex));
        blocks.AddRange(GetReverseDiagonalPieces(_xIndex, _yIndex));
        blocks.AddRange(GetCurrentPieces(_targetXIndex, _targetYIndex));

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> GetColumnAndRowPieces(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        blocks.AddRange(GetColumnPieces(_xIndex));
        blocks.AddRange(GetRowPieces(_yIndex));

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> GetColumnAndDiagonalPieces(int _xIndex, int _yIndex, int _targetXIndex, int _targetYIndex)
    {
        List<Potion> blocks = new List<Potion>();

        blocks.AddRange(GetColumnPieces(_xIndex));
        blocks.AddRange(GetDiagonalPieces(_xIndex, _yIndex));
        blocks.AddRange(GetCurrentPieces(_targetXIndex, _targetYIndex));

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> GetColumnAndReverseDiagonalPieces(int _xIndex, int _yIndex, int _targetXIndex, int _targetYIndex)
    {
        List<Potion> blocks = new List<Potion>();

        blocks.AddRange(GetColumnPieces(_xIndex));
        blocks.AddRange(GetReverseDiagonalPieces(_xIndex, _yIndex));
        blocks.AddRange(GetCurrentPieces(_targetXIndex, _targetYIndex));

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> Get3DistancePieces(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        // 5x5 ��� dot �������� ���� �Ʒ� ������ ������ �� �������� ����
        for (int i = _xIndex - 2; i <= _xIndex + 2; i++)
        {
            for (int j = _yIndex - 2; j <= _yIndex + 2; j++)
            {
                // Check if the piece is inside the board �𼭸� üũ
                if (i >= 0 && i < board.width && j >= 0 && j < board.height && IsUsableAndNotBeforeMatched(i, j))
                {
                    CheckSpecialChainMatch(i, j, blocks);
                }
            }
        }

        if (_xIndex >= 3 && board.potionBoard[_xIndex - 3, _yIndex] != null && IsUsableAndNotBeforeMatched(_xIndex - 3, _yIndex))
        {
            CheckSpecialChainMatch(_xIndex - 3, _yIndex, blocks);
        }
        if (_xIndex < board.width - 3 && board.potionBoard[_xIndex + 3, _yIndex] != null && IsUsableAndNotBeforeMatched(_xIndex + 3, _yIndex))
        {
            CheckSpecialChainMatch(_xIndex + 3, _yIndex, blocks);

        }
        if (_yIndex >= 3 && board.potionBoard[_xIndex, _yIndex - 3] != null && IsUsableAndNotBeforeMatched(_xIndex, _yIndex - 3))
        {
            CheckSpecialChainMatch(_xIndex, _yIndex - 3, blocks);
        }
        if (_yIndex < board.height - 3 && board.potionBoard[_xIndex, _yIndex + 3] != null && IsUsableAndNotBeforeMatched(_xIndex, _yIndex + 3))
        {
            CheckSpecialChainMatch(_xIndex, _yIndex + 3, blocks);
        }

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> Get3ColumnPieces(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();
        
        for (int i = _xIndex - 1; i <= _xIndex + 1; i++ )
        {
            for (int j = 0; j < board.height; j++)
            {
                if (i >= 0 && i < board.width)
                {
                    blocks.AddRange(GetColumnPieces(i));

                }
            }
        }

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> Get3RowPieces(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        for (int i = _yIndex - 1; i <= _yIndex + 1; i++)
        {
            for (int j = 0; j < board.width; j++)
            {
                if (i >= 0 && i < board.height)
                {
                    blocks.AddRange(GetRowPieces(i));
                }
            }
        }

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> Get3DiagonalPieces(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        for (int i = -1; i<=1; i++)
        {
            blocks.AddRange(GetDiagonalPieces(_xIndex + i, _yIndex));
        }

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> Get3ReverseDiagonalPieces(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        for (int i = -1; i<=1; i++)
        {
            blocks.AddRange(GetReverseDiagonalPieces(_xIndex + i, _yIndex));
        }

        return blocks;
    }

    // �׽�Ʈ ���� : O
    // �̰ɷ� �׽�Ʈ�Ͽ� �ӵ� ����
    public List<Potion> GetAllPieces()
    {
        List<Potion> blocks = new List<Potion>();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (IsUsableAndNotBeforeMatched(i, j))
                {
                    blocks.Add(board.potionBoard[i, j].potion.GetComponent<Potion>());
                    //board.potionBoard[i, j].potion.GetComponent<Potion>().isMatched = true;
                }
            }
        }

        return blocks;
    }

    // �׽�Ʈ ���� : O
    public List<Potion> GetDiagonalAndReverseDiagonalPieces(int _xIndex, int _yIndex, int _targetXIndex, int _targetYIndex)
    {
        List<Potion> blocks = new List<Potion>();

        blocks.AddRange(GetDiagonalPieces(_xIndex, _yIndex));
        blocks.AddRange(GetReverseDiagonalPieces(_xIndex, _yIndex));
        blocks.AddRange(GetCurrentPieces(_targetXIndex, _targetYIndex));

        return blocks;
    }

    public List<Potion> GetCurrentPieces(int _xIndex, int _yIndex)
    {
        List<Potion> blocks = new List<Potion>();

        if (IsUsableAndNotBeforeMatched(_xIndex, _yIndex))
        {
            blocks.Add(board.potionBoard[_xIndex, _yIndex].potion.GetComponent<Potion>());
            board.potionBoard[_xIndex, _yIndex].potion.GetComponent<Potion>().isMatched = true;
        }

        return blocks;
    }

    private void SwitchDrillVertical()
    {
        Debug.Log("���� ���� �Ϲ� �� �帱(����)�� ��ü");
        throw new NotImplementedException();
    }

    private void SwitchDrillHorizontal()
    {
        Debug.Log("���� ���� �Ϲ� �� �帱(����)�� ��ü");
        throw new NotImplementedException();
    }

    private void SwitchBomb()
    {
        Debug.Log("���� ���� �Ϲ� �� ��ź���� ��ü");
        throw new NotImplementedException();
    }

    private void SwitchPickLeft()
    {
        Debug.Log("���� ���� �Ϲ� �� ���(���밢)���� ��ü");
        throw new NotImplementedException();
    }

    private void SwitchPickRight()
    {
        Debug.Log("���� ���� �Ϲ� �� ���(�밢)���� ��ü");
        throw new NotImplementedException();
    }

    private bool IsUsableAndNotBeforeMatched(int _xIndex, int _yIndex)
    {
        return board.potionBoard[_xIndex, _yIndex].isUsable && !board.potionBoard[_xIndex, _yIndex].potion.GetComponent<Potion>().isMatched ? true : false;
    }

    public bool IsSpecialBlock(PotionType potiontype)
    {
        switch (potiontype)
        {
            case PotionType.DrillVertical:
                return true;
            case PotionType.DrillHorizontal:
                return true;
            case PotionType.Bomb:
                return true;
            case PotionType.PickLeft:
                return true;
            case PotionType.PickRight:
                return true;
            case PotionType.Prism:
                return true;
            default:
                return false;
        }
    }

    // Ư�� �� ȿ���� ��� ���ŵǴ� �� ����Ʈ�� �߰� �� �߰��� Ư���� ȿ�� �ߵ��Ǵ��� üũ
    void CheckSpecialChainMatch(int _xIndex, int _yIndex, List<Potion> _blocks)
    {
        Potion potion = board.potionBoard[_xIndex, _yIndex].potion.GetComponent<Potion>();
        _blocks.Add(potion);
        potion.isMatched = true;

        if (IsSpecialBlock(potion.potionType))
        {
            _blocks.AddRange(RunSpecialBlock(potion));
        }
    }
}

public class MatchResult
{
    public List<Potion> connectedPotions;
    public MatchDirection direction;
}

// ���� : 3�迭, 4�迭 ����, 4�迭 �׸�, 5�迭 ����, 5�迭 L�� (�ý��� ��ȹ�� 27P)

public enum MatchDirection
{
    Vertical_3, // 3 ����
    Horizontal_3, // 3 ����
    Vertical_4, // 4 ���� : �帱(����)
    Horizontal_4, // 4 ���� : �帱(����)
    LongVertical, // 5 �̻� ���� : ������
    LongHorizontal, // 5 �̻� ���� -> ������
    Super, // ���� ���� ���ļ� �۵��� -> 5�迭 L�� �߰� ���� ���� : ��ź
    Square, // 4�迭 �׸� : ���(�밢), ���(���밢)
    None
}