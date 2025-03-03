using System.Collections;
using UnityEngine;

public class MonsterCatalogManager : MonoBehaviour
{
    public MonsterDataObject monsterDataObject;
    public bool isCatalogOpen = false;

    private Vector3 OriginPos;

    void Awake()
    {
        OriginPos = transform.position;
        for(int i = 0 ; i < transform.childCount ; i ++){
            if(transform.GetChild(i).GetComponent<MonsterProfile>() != null){
                transform.GetChild(i).GetComponent<MonsterProfile>().UseData(monsterDataObject.monsterDataList[i]);
            }
            
        }
    }

    public void OnButtonClicked(){
        if(isCatalogOpen){
            HideCatalog();
        }else{
            ShowCatalog();
        }
    }

    public void ShowCatalog(){
        isCatalogOpen = true;
        StartCoroutine(ShowCatalogCoroutine(0.3f));
    }
    IEnumerator ShowCatalogCoroutine(float duration){
        float elaspseTime = 0;
        while(elaspseTime <= duration){
            transform.position = Vector3.Lerp(OriginPos,new Vector3(0, 0, 0), elaspseTime / duration);
            elaspseTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(0, 0, 0);
    }

    public void HideCatalog(){
        isCatalogOpen = false;
        StartCoroutine(HideCatalogCoroutine(0.3f));
    }

    IEnumerator HideCatalogCoroutine(float duration){
        float elaspseTime = 0;
        while(elaspseTime <= duration){
            transform.position = Vector3.Lerp(new Vector3(0, 0, 0), OriginPos, elaspseTime / duration);
            elaspseTime += Time.deltaTime;
            yield return null;
        }
        transform.position = OriginPos;
    }
}
