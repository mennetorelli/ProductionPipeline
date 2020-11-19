using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputController : MonoBehaviour
{
    public void OnMouseDown(InputAction.CallbackContext context)
    {
        ShowDetailsManager.Instance.DeactivatePanels();
        Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit);
        if (hit.collider != null && hit.collider.gameObject.GetComponentInParent<ISelectable>() != null)
        {
            hit.collider.gameObject.GetComponentInParent<ISelectable>().Selected();
        }
    }
}
