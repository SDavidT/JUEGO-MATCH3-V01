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
    public GameObject otherDot;
    private Board board;
    public int targetX;
    public int targetY;
    private Vector2 tempPosition;
    public bool isMatched=false;
    public int previousColumn;
    public int previousRow;
    public float swipeResiste=1f;
    private FindMatches findMatches;
    public bool isColumnBomb;
    public bool isRowBomb;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public bool isColorBomb;
    public GameObject colorBomb;
    
    // Start is called before the first frame update
    void Start()
    {
        isColumnBomb=false;
        isRowBomb=false;
        board=FindObjectOfType<Board>();
        findMatches=FindObjectOfType<FindMatches>();
        // targetX=(int)transform.position.x;
        // targetY=(int)transform.position.y;
        // row=targetY;
        // column=targetX;

        // previousRow=row;
        // previousColumn=column;

    }

    private void OnMouseOver() {

        if (Input.GetMouseButtonDown(1)){

            isColorBomb=true;
            GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
            color.transform.parent=this.transform; 
        }    
    }

    // Update is called once per frame
    void Update()
    {
        

        // if(isMatched){
        //     SpriteRenderer mySprite = GetComponent<SpriteRenderer>(); //generamos un componente sprite render 
        //     mySprite.color=new Color(1f,1f,1f,.2f); // cambiamos de color los puntos
        // }



        // movimiento de puntos
        targetX=column;
        targetY=row;
        if(Mathf.Abs(targetX-transform.position.x)>.1){
            tempPosition=new Vector2 (targetX, transform.position.y);
            transform.position=Vector2.Lerp(transform.position, tempPosition, .4f);

            if(board.allDots[column,row]!= this.gameObject){ //**
                board.allDots[column,row]=this.gameObject;
            }

                //findMatches.FindAllMatches();
                StartCoroutine(MatchTwoCo());
                //MatchTwo();

        }else{
            tempPosition=new Vector2 (targetX,transform.position.y);
            transform.position= tempPosition;

        }
        if(Mathf.Abs(targetY-transform.position.y)>.1){
            tempPosition=new Vector2 (transform.position.x,targetY);
            transform.position=Vector2.Lerp(transform.position, tempPosition, .4f);

            if(board.allDots[column,row]!= this.gameObject){ //***
                board.allDots[column,row]=this.gameObject;
            }

                //findMatches.FindAllMatches();
                StartCoroutine(MatchTwoCo());
                //MatchTwo();

        }else{
            tempPosition=new Vector2 (transform.position.x,targetY);
            transform.position= tempPosition;
            
        }

    }

    // retorno del punto cuando no son iguales
    public IEnumerator CheckMoveCo(){ // corrutina 
        
        //******************
        if(isColorBomb){
            //This piece is a color bomb, and the other piece is the color to destroy
            findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched = true;
        }else if(otherDot.GetComponent<Dot>().isColorBomb){
            //The other piece is a color bomb, and this piece has the color to destroy
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatched = true;
        }

        yield return new WaitForSeconds(.5f); // timepo de retorno de pieza a su lugar
        if (otherDot!=null){
            if(!isMatched && !otherDot.GetComponent<Dot>().isMatched){

                otherDot.GetComponent<Dot>().row=row;
                otherDot.GetComponent<Dot>().column=column;
                row=previousRow;
                column=previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot=null;
                board.currentState=GameState.move;
            } else {
                board.DestroyMatches();
                }
            //otherDot=null;
        } 
    }

    private void OnMouseDown(){
        
        if(board.currentState==GameState.move){
            firstTouchPosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

    }

    private void OnMouseUp(){

        if (board.currentState==GameState.move){

            finalTouchPosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle(){
        if (Mathf.Abs(finalTouchPosition.y-firstTouchPosition.y)>swipeResiste || Mathf.Abs(finalTouchPosition.x-firstTouchPosition.x)>swipeResiste ){
            swipeAngle=Mathf.Atan2(finalTouchPosition.y-firstTouchPosition.y,finalTouchPosition.x-firstTouchPosition.x)*180/Mathf.PI;
            MovePieces();
            board.currentState=GameState.wait;
            board.currentDot = this;
        } else {
            board.currentState=GameState.move;
        }
    }

    // detecto y seteo el desplazamiento 
    void MovePieces(){
        if(swipeAngle>-45 && swipeAngle<=45 && column<board.width-1){ // ir a la derecha
            otherDot=board.allDots[column+1,row];
            previousRow=row;
            previousColumn=column;
            otherDot.GetComponent<Dot>().column=otherDot.GetComponent<Dot>().column-1;//desplazamiento del punto intercambiado - vecino
            column=column+1;//desplazamiento del punto seleccionado

            if(otherDot.tag==board.allDots[column-1,row].tag){

                if(board.allDots[column-1,row].tag=="O" || board.allDots[column-1,row].tag=="H"){

                    otherDot.GetComponent<Dot>().isMatched=true;
                    board.allDots[column-1,row].GetComponent<Dot>().isMatched=true;
                } 

            } else if(otherDot.tag=="Dark Green Dot" || board.allDots[column-1,row].tag=="Indigo Dot"){

                    otherDot.GetComponent<Dot>().isMatched=true;
                    board.allDots[column-1,row].GetComponent<Dot>().isMatched=true;
                } 

            //Debug.Log(otherDot.tag);//punto al que me dirijo 
            //Debug.Log(board.allDots[column-1,row].tag);// punto con el que me muevo
        }else if(swipeAngle>45 && swipeAngle<=135 && row< board.height-1){ // ir para arriba
            otherDot=board.allDots[column,row+1];
            previousRow=row;
            previousColumn=column;
            otherDot.GetComponent<Dot>().row-=1;
            row+=1;


        } else if ((swipeAngle>135 || swipeAngle<=-135)&&column>0){ // ir a la izquierda
            otherDot=board.allDots[column-1,row];
            previousRow=row;
            previousColumn=column;
            otherDot.GetComponent<Dot>().column+=1;
            column-=1;
        }else if(swipeAngle<-45 && swipeAngle>=-135 && row>0){// ir para abajo
            otherDot=board.allDots[column,row-1];
            previousRow=row;
            previousColumn=column;
            otherDot.GetComponent<Dot>().row+=1;
            row-=1;
        }

        StartCoroutine(CheckMoveCo());
    }



    private IEnumerator MatchTwoCo(){

        yield return new WaitForSeconds(.2f);

        if (column>0 && column < board.width-1){
            GameObject otro = board.allDots[column,row];
            //GameObject rightDot1 = board.allDots[column+1,row];

            if(otro!=null ){

                

                if(otro.tag==this.gameObject.tag ){
                    
                    //  otro.GetComponent<Dot>().isMatched=true;

                    //  isMatched=true;
                    Debug.Log("--------------------------");
                    Debug.Log(otro.tag);
                    Debug.Log(otro.transform.position);
                    Debug.Log(this.gameObject.tag );
                    Debug.Log(this.gameObject.transform.position);

                }
            }
        }

    }
    // buscar 3 puntos iguales 

    // void FindMatches(){
    //     if (column>0 && column < board.width-1){
    //         GameObject leftDot1 = board.allDots[column-1,row];
    //         GameObject rightDot1 = board.allDots[column+1,row];

    //         if(leftDot1!=null && rightDot1!=null){

    //             if(leftDot1.tag==this.gameObject.tag && rightDot1.tag==this.gameObject.tag){
                    
    //                 leftDot1.GetComponent<Dot>().isMatched=true;
    //                 rightDot1.GetComponent<Dot>().isMatched=true;

    //                 isMatched=true;

    //             }
    //         }
    //     }

    //     if (row>0 && row < board.height-1){
    //         GameObject upDot1 = board.allDots[column,row+1];
    //         GameObject downDot1 = board.allDots[column,row-1];

    //         if (upDot1!=null && downDot1!= null){

    //             if(upDot1.tag==this.gameObject.tag && downDot1.tag==this.gameObject.tag){
                    
    //                 upDot1.GetComponent<Dot>().isMatched=true;
    //                 downDot1.GetComponent<Dot>().isMatched=true;

    //                 isMatched=true;
                    

    //             }
    //         }     
    //     }
    // } 


    


// funcion para generacion de linea de bombas vertical y horizontal 
    public void MakeRowBomb(){
        isRowBomb=true;
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent=this.transform; 
    } 

    public void MakeColumnBomb(){
        isColumnBomb=true;
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent=this.transform; 
    } 
}
