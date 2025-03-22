using System.Collections;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridTileManager : MonoBehaviour
{
    public Grid mapGrid;
    public Grid.GridType gridType;
    public Vector3 positionPivot, scalePivot;
    public int mapX;
    public int mapY;
    public Text monsterStat;
    public GameObject gameManagerObject;
    public SpriteScriptObject layer1sprites;
    public void InitialData() {
        this.gameObject.layer = 6;
        gameManagerObject = GameObject.Find("GameManager"); 
        name = mapGrid.GridTypeToWord + " " + mapX + " " + mapY;

        positionPivot = transform.position;
        scalePivot = transform.localScale;
    }
    public void UpdateData() {
        if (GameData.map[mapX,mapY] != null) {
            mapGrid = GameData.map[mapX,mapY];
            gridType = mapGrid.type;
            //UpdateText();
            //临时代码
            transform.name = mapGrid.GridTypeToWord + " " + mapX + " " + mapY;
            Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
            TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
            gridtext.text = " ";
            gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
            gridtext.text = " ";

            UpdateSprite();
        } else {
            //没东西就清空
            mapGrid = null;
            gridType = Grid.GridType.BARRIER;
            Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
            TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
            gridtext.text = " ";
            gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
            gridtext.text = " ";

            this.gameObject.GetComponent<SpriteRenderer>().sprite = layer1sprites.spriteData[8].sprites[0];
        }
    }
    public void UpdateText() {
        transform.name = mapGrid.GridTypeToWord + " " + mapX + " " + mapY;
        Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
        TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.GridTypeToWord;
        gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
        gridtext.text = mapGrid.stat.ToString();
    }
    public void UpdateSprite() {
        try {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = layer1sprites.spriteData[(int)gridType].sprites[mapGrid.stat - 1];
            GridMoveAnim();
        } catch (System.Exception e) {
            Debug.Log("Error: " + mapGrid.GridTypeToWord + " " + mapGrid.stat + e);
        }
    }

    void GridMoveAnim(){
        StartCoroutine(GridMoveAnimCoroutine());
    }

    IEnumerator GridMoveAnimCoroutine(){
        int X = GameData.map[mapX,mapY].fromX;
        int Y = GameData.map[mapX,mapY].fromY;
        GridTileManager originalGridTile = FindGridIn(X,Y);
        Vector3 originalPosition = new Vector3();
        Vector3 originalScale = new Vector3();

        if(originalGridTile != null){
            originalPosition = originalGridTile.positionPivot;
            originalScale = originalGridTile.scalePivot;
        }else{
            originalPosition = positionPivot - new Vector3(0, 2, 0);
            originalScale = scalePivot;
        }
        
        float duration = 0.1f;

        transform.position = originalPosition;
        transform.localScale = originalScale;

        transform.DOMoveX(positionPivot.x, duration * 1.5f).SetEase(Ease.Linear);
        transform.DOMoveY(positionPivot.y, duration * 3).SetEase(Ease.OutBounce);

        //Tween move = DOTween.To(()=>transform.position, x => transform.position = x, positionPivot, duration * 3);
        //move.SetEase(Ease.OutBounce);
        Tween scale = DOTween.To(()=>transform.localScale, x => transform.localScale = x, scalePivot, duration);
        scale.SetEase(Ease.Linear);

        yield return new WaitForSeconds(duration);

        GameData.map[mapX,mapY].fromX = mapX;
        GameData.map[mapX,mapY].fromY = mapY;
        
    }

    GridTileManager FindGridIn(Vector2Int map){
        return FindGridIn(map.x, map.y);
    }

    GridTileManager FindGridIn(int x, int y){
        GameObject[] grids = GameObject.FindGameObjectsWithTag("gridGameObject");
        foreach (GameObject grid in grids) {
            if (grid.GetComponent<GridTileManager>().mapX == x && grid.GetComponent<GridTileManager>().mapY == y){
                return grid.GetComponent<GridTileManager>();
            }
        }
        return null;
    }
}
