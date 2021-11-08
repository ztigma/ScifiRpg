using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PointerSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<GameObject> EnabledOnMouse;
    public UnityEvent OnMouseEnter;
    public UnityEvent OnMouseExit;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        EnabledOnMouse.ForEach(n => n.SetActive(true));
        OnMouseEnter.Invoke();
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        EnabledOnMouse.ForEach(n => n.SetActive(false));
        OnMouseExit.Invoke();
    }
}