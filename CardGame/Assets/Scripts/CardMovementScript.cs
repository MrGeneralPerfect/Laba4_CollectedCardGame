using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CardMovementsScr : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera MainCamera;
    Vector3 offset;
    public Transform DefaultParent, DefaultTempCardParent;
    GameObject TempCardGO;
    public GameManegerScr GameManeger; 
    public bool IsGraddble;


    void Awake()
    {
        MainCamera = Camera.allCameras[0];

        TempCardGO = GameObject.Find("TempCardGO");

        GameManeger = FindObjectOfType<GameManegerScr>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);

        DefaultParent = DefaultTempCardParent = transform.parent;

        IsGraddble = GameManeger.IsPlayerTurn &&
            (
            (DefaultParent.GetComponent<DropPlayScr>().Type == FieldType.SELF_HAND &&
            GameManeger.PlayerMana >= GetComponent<CardInfoScr>().SelfCard.Manacost) ||
            (DefaultParent.GetComponent<DropPlayScr>().Type == FieldType.SELF_FIELD &&
            GetComponent<CardInfoScr>().SelfCard.CanAttack)
            );
            
            
            //(DefaultParent.GetComponent<DropPlayScr>().Type == FieldType.SELF_HAND || 
        //             DefaultParent.GetComponent<DropPlayScr>().Type == FieldType.SELF_FIELD) &&
        //             GameManeger.isActiveAndEnabled;

        if (!IsGraddble)
            return;

        TempCardGO.transform.SetParent(DefaultParent);
        TempCardGO.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsGraddble)
            return;

        Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;

        if (TempCardGO.transform.parent != DefaultTempCardParent)
            TempCardGO.transform.SetParent(DefaultTempCardParent);

        if(DefaultParent.GetComponent<DropPlayScr>().Type != FieldType.SELF_FIELD)
            CheckPosition();  
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsGraddble)
            return;

        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(TempCardGO.transform.GetSiblingIndex());
        TempCardGO.transform.SetParent(GameObject.Find("Canvas").transform);
        TempCardGO.transform.localPosition = new Vector3(2223, 0);
    }

    void CheckPosition()
    {
        int newIndex = DefaultTempCardParent.childCount;
        for (int i = 0; i < DefaultTempCardParent.childCount; i++)
        {
            if (transform.position.x < DefaultTempCardParent.GetChild(i).position.x)
            {
                newIndex = i;

                if (TempCardGO.transform.GetSiblingIndex() < newIndex)
                    newIndex--;

                break;
            }
        }
        TempCardGO.transform.SetSiblingIndex(newIndex);
    }
}
