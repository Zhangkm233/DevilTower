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
    public SpriteScriptObject[] layerspritess;

    public void OnMouseEnter()
    {
        if(mapY != 3)return;
        transform.DOScale(scalePivot * 1.1f, 0.2f).SetEase(Ease.OutElastic);
    }

    public void OnMouseExit()
    {
        if(mapY != 3)return;
        transform.DOScale(scalePivot, 0.2f).SetEase(Ease.OutElastic);
    }

    void Update()
    {
        if (gridType == Grid.GridType.MONSTER){
            Vector3 scale =  new Vector3(0, Mathf.Sin(Time.time * 2) * 0.02f * Time.deltaTime, 0);
            transform.localScale += scale;
        }
    }

    public void InitialData() {
        this.gameObject.layer = 6;
        gameManagerObject = GameObject.Find("GameManager"); 
        name = mapGrid.GridTypeToWord + " " + mapX + " " + mapY;

        positionPivot = transform.position;
        scalePivot = transform.localScale;
    }
    void SetToBarrier() {
        //没东西就清空
        mapGrid = new Grid(1,Grid.GridType.BARRIER);
        gridType = Grid.GridType.BARRIER;

        Canvas gridCanvas = this.GetComponentInChildren<Canvas>();
        TMP_Text gridtext = gridCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
        gridtext.text = " ";
        gridtext = gridCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
        gridtext.text = " ";

        this.gameObject.GetComponent<SpriteRenderer>().sprite = layerspritess[GameData.layer - 1].spriteData[8].sprites[0];
        transform.name = "X" + " " + mapX + " " + mapY;
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
            SetToBarrier();
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
        //try {
        if (mapGrid.GridTypeToWord == "") {
            SetToBarrier();
        }
        if (gridType == Grid.GridType.BARRIER) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = layerspritess[GameData.layer - 1].spriteData[8].sprites[0];
            return;
        } else {
            try {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = layerspritess[GameData.layer - 1].spriteData[(int)gridType].sprites[mapGrid.stat - 1];
            } catch (System.Exception e) {
                Debug.LogWarning("Error: " + mapGrid.GridTypeToWord + " " + mapGrid.stat + "\n" + e);
            }
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = layerspritess[GameData.layer - 1].spriteData[(int)gridType].sprites[mapGrid.stat - 1];
        }
        GridMoveAnim();
        //} catch (System.Exception e) {
        //    Debug.LogWarning("Error: " + mapGrid.GridTypeToWord + " " + mapGrid.stat + "\n" + e);
        //}
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
