using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{

    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();
    private Dot otroP;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {

        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < board.width; i++)
        {

            for (int j = 0; j < board.height; j++)
            {

                GameObject currentDot = board.allDots[i, j];

                if (currentDot != null)
                {

                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];


                        if (leftDot != null && rightDot != null)
                        {



                            if (leftDot.tag == currentDot.tag)
                            {

                                if (currentDot.GetComponent<Dot>().isRowBomb || leftDot.GetComponent<Dot>().isRowBomb || rightDot.GetComponent<Dot>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j));
                                }

                                if (!currentMatches.Contains(leftDot))
                                {
                                    currentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<Dot>().isMatched = true;
                                if (!currentMatches.Contains(rightDot))
                                {
                                    currentMatches.Add(rightDot);
                                }
                                rightDot.GetComponent<Dot>().isMatched = true;
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatched = true;

                            }
                        }
                    }


                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];

                        if (upDot != null && downDot != null)
                        {

                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                if (!currentMatches.Contains(upDot))
                                {
                                    currentMatches.Add(upDot);
                                }
                                upDot.GetComponent<Dot>().isMatched = true;
                                if (!currentMatches.Contains(downDot))
                                {
                                    currentMatches.Add(downDot);
                                }
                                downDot.GetComponent<Dot>().isMatched = true;
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }

    //*******************
    public void MatchPiecesOfColor(string color)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                //Check if that piece exists
                if (board.allDots[i, j] != null)
                {
                    //Check the tag on that dot
                    if (board.allDots[i, j].tag == color)
                    {
                        //Set that dot to be matched
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }
    }

    List<GameObject> GetColumnPiece(int column)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = 0; i < board.height; i++)
        {

            if (board.allDots[column, i] != null)
            {

                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }

    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = 0; i < board.width; i++)
        {

            if (board.allDots[i, row] != null)
            {

                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }


    // chequeo si la bomba debe generarse horizontal o vertical y llamado a la funcion de generacion de bombas
    public void CheckBombs()
    {

        if (board.currentDot != null)
        {

            if (board.currentDot.isMatched)
            {

                board.currentDot.isMatched = false;
                // int typeOfBomb =Random.Range(0,100);

                // if(typeOfBomb<50){

                //     board.currentDot.MakeRowBomb();
                // } else if (typeOfBomb>=50){

                //     board.currentDot.MakeColumnBomb();

                // }

                if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45) || (board.currentDot.swipeAngle < -135 && board.currentDot.swipeAngle >= 135))
                {

                    board.currentDot.MakeRowBomb();
                }
                else
                {
                    board.currentDot.MakeColumnBomb();
                }

            }
            else if (board.currentDot.otherDot != null)
            {

                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                if (otherDot.isMatched)
                {

                    otherDot.isMatched = false;

                    // int typeOfBomb =Random.Range(0,100);

                    // if(typeOfBomb<50){

                    //     otherDot.MakeRowBomb();
                    // } else if (typeOfBomb>=50){

                    //     otherDot.MakeColumnBomb();

                    // }

                    if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45) || (board.currentDot.swipeAngle < -135 && board.currentDot.swipeAngle >= 135))
                    {

                        otherDot.MakeRowBomb();
                    }
                    else
                    {
                        otherDot.MakeColumnBomb();
                    }

                }
            }
        }
    }
}

////esto es una prueba