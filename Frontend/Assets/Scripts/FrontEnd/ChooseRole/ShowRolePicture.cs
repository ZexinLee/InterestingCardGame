using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ShowRolePicture : MonoBehaviour, IPointerClickHandler
{
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        // transform.SetAsFirstSibling();
        // transform.SetSiblingIndex(2);
    }
}
