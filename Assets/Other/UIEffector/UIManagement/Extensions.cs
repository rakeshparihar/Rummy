using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    public static bool IsPointerValid(this PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return false;

        return true;
    }
}
