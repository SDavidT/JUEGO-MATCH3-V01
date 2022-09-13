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

                while(MatchesAt(i,j,dots[dotToUse])){
                    dotToUse=Random.Range(0,dots.Length);
                }
                GameObject dot = Instantiate(dots[dotToUse],tempPosition, Quaternion.identity);
                dot.transform.parent= this.transform;
                dot.name= "( " + i + ", " + j + " )";
                allDots[i,j]=dot;


            }

        }

    }

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

                //
                if(allDots[column,row-1].tag==piece.tag && allDots[column,row-2].tag==piece.tag){
                    return true;
                }
            }
        }

        return false;

    }
}