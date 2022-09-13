using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int width;
    public int height;
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    public GameObject [] dots;
    public GameObject[,] allDots;

    // Start is called before the first frame update
    void Start()
    {
        // generacion matriz]
        allTiles=new BackgroundTile[width, height];
        allDots=new GameObject[width,height];
        SetUp();
        //hola
        

    }

    private void SetUp (){

        for (int i = 0; i<width; i++){

            for (int j = 0; j<height; j++){
                // creacion de mosaicos o matriz
                Vector2 tempPosition= new Vector2(i,j);// representacion de vectores y posicion 2d con ejes X y Y
                GameObject backgroundTile = Instantiate (tilePrefab, tempPosition, Quaternion.identity) as GameObject; // clona objetos moviendo la posicion y con rotacion 0
                backgroundTile.transform.parent=this.transform; // se asigna cada objeto al objeto padre
                backgroundTile.name="( " + i + ", " + j + " )"; // se asigna el nombre a cada objeto

                // crear puntos de colores
                int dotToUse = Random.Range(0,dots.Length);
                int maxIterations=0;

                // bucle para evitar que se repitan puntos al inciar el juego 
                while(MatchesAt(i,j,dots[dotToUse])&& maxIterations<100){
                    dotToUse=Random.Range(0,dots.Length);
                    maxIterations++;
                }
                maxIterations=0;
                GameObject dot = Instantiate(dots[dotToUse],tempPosition, Quaternion.identity);
                dot.transform.parent= this.transform;
                dot.name= "( " + i + ", " + j + " )";
                allDots[i,j]=dot;


            }

        }

    }

    // Generar puntos no repetidos al iniciar el juego 
    private bool MatchesAt(int column, int row, GameObject piece){

        if(column>1 && row >1){

            if(allDots[column-1,row].tag==piece.tag && allDots[column-2, row].tag==piece.tag){
                return true;
            } 

            if(allDots[column,row-1].tag==piece.tag && allDots[column, row-2].tag==piece.tag){
                return true;
            }
        } else if (column<=1 || row<=1){

            if(row>1){
                
                if(allDots[column,row-1].tag==piece.tag && allDots[column,row-2].tag==piece.tag){
                    return true;
                }
            }

            if(column>1){
                
                if(allDots[column-1,row].tag==piece.tag && allDots[column-2,row].tag==piece.tag){
                    return true;
                }
            }
        }

        return false;

    }

    //Destruye el punto la badera true
    private void DestroyMatchesAt(int column, int row){
        
        if (allDots[column, row].GetComponent<Dot>().isMatched){

            Destroy(allDots[column, row]);
            allDots[column,row]= null;

        }
    }


    // recorre la matriz completa y llama a la función de destrucción
    public void DestroyMatches(){

        for (int i=0; i<width; i++){

            for (int j = 0; j<height; j++){
                if (allDots[i,j]!= null){
                    DestroyMatchesAt(i,j);
                }
            }
        }

        StartCoroutine(DecreaseRowCo());
    }


    //Corrutina para desplazamiento vertical de los puntos

    private IEnumerator DecreaseRowCo(){ 
        int nullCount=0;

        for (int i =0; i<width; i++){
            
            for(int j=0; j<height; j++){

                if (allDots[i,j]==null){
                    nullCount++;

                } else if(nullCount>0){
                    allDots[i,j].GetComponent<Dot>().row-=nullCount;
                    allDots[i,j]=null;
                }
            }
            nullCount=0;
        }

        yield return new WaitForSeconds(.4f);
    }
}
