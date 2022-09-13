using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("variables")]
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    public float swipeAngle=0;
    public int column;
    public int row;
    private GameObject otherDot;
    private Board board;
    public int targetX;
    public int targetY;
    private Vector2 tempPosition;
    public bool isMatched=false;
    public int previousColumn;
    public int previousRow;



    
    // Start is called before the first frame update
    void Start()
    {
        board=FindObjectOfType<Board>();
        targetX=(int)transform.position.x;
        targetY=(int)transform.position.y;
        row=targetY;
        column=targetX;

        previousRow=row;
        previousColumn=column;

    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        if(isMatched){
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color=new Color(1f,1f,1f,.2f);
        }




        targetX=column;
        targetY=row;
        if(Mathf.Abs(targetX-transform.position.x)>.1){
            tempPosition=new Vector2 (targetX, transform.position.y);
            transform.position=Vector2.Lerp(transform.position, tempPosition, .4f);

        }else{
            tempPosition=new Vector2 (targetX,transform.position.y);
            transform.position= tempPosition;
            board.allDots[column,row]=this.gameObject;

        }
        if(Mathf.Abs(targetY-transform.position.y)>.1){
            tempPosition=new Vector2 (transform.position.x,targetY);
            transform.position=Vector2.Lerp(transform.position, tempPosition, .4f);

        }else{
            tempPosition=new Vector2 (transform.position.x,targetY);
            transform.position= tempPosition;
            board.allDots[column,row]=this.gameObject;
            
        }

    }

    public IEnumerator CheckMoveCo(){
        yield return new WaitForSeconds(.5f);
        if (otherDot!=null){
            if(!isMatched && !otherDot.GetComponent<Dot>().isMatched){

                otherDot.GetComponent<Dot>().row=row;
                otherDot.GetComponent<Dot>().column=column;
                row=previousRow;
                column=previousColumn;
            }
            otherDot=null;
        }
    }

    private void OnMouseDown(){
        firstTouchPosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void OnMouseUp(){
        finalTouchPosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle(){
        swipeAngle=Mathf.Atan2(finalTouchPosition.y-firstTouchPosition.y,finalTouchPosition.x-firstTouchPosition.x)*180/Mathf.PI;
        //Debug.Log(swipeAngle);
        MovePieces();


    }

    void MovePieces(){
        if(swipeAngle>-45 && swipeAngle<=45 && column<board.width-1){ // ir a la derecha
            otherDot=board.allDots[column+1,row];
            otherDot.GetComponent<Dot>().column=otherDot.GetComponent<Dot>().column-1;//desplazamiento del punto intercambiado - vecino
            column=column+1;//desplazamiento del punto seleccionado
        }else if(swipeAngle>45 && swipeAngle<=135 && row< board.height-1){ // ir para arriba
            otherDot=board.allDots[column,row+1];
            otherDot.GetComponent<Dot>().row-=1;
            row+=1;
        } else if ((swipeAngle>135 || swipeAngle<=-135)&&column>0){ // ir a la izquierda
            otherDot=board.allDots[column-1,row];
            otherDot.GetComponent<Dot>().column+=1;
            column-=1;
        }else if(swipeAngle<-45 && swipeAngle>=-135 && row>0){// ir para abajo
            otherDot=board.allDots[column,row-1];
            otherDot.GetComponent<Dot>().row+=1;
            row-=1;
        }

        StartCoroutine(CheckMoveCo());
    }

    void FindMatches(){
        if (column>0 && column < board.width-1){
            GameObject leftDot1 = board.allDots[column-1,row];
            GameObject rightDot1 = board.allDots[column+1,row];
            
            if(leftDot1.tag==this.gameObject.tag && rightDot1.tag==this.gameObject.tag){
                
                leftDot1.GetComponent<Dot>().isMatched=true;
                rightDot1.GetComponent<Dot>().isMatched=true;

                isMatched=true;

            }
        }

        if (row>0 && row < board.height-1){
            GameObject upDot1 = board.allDots[column,row+1];
            GameObject downDot1 = board.allDots[column,row-1];
            
            if(upDot1.tag==this.gameObject.tag && downDot1.tag==this.gameObject.tag){
                
                upDot1.GetComponent<Dot>().isMatched=true;
                downDot1.GetComponent<Dot>().isMatched=true;

                isMatched=true;

            }
        }
        

    }

}
