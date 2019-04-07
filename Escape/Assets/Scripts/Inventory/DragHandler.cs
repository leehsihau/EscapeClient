using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region IBeginDragHandler implementation
    public void OnBeginDrag (PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    #endregion

}
