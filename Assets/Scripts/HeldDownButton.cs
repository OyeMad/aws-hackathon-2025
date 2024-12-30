using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HeldDownButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool buttonDown = false;

    [SerializeField]
    UnityEvent  buttonHeldDown;

    [SerializeField]
    UnityEvent buttonReleased;

    // Update is called once per frame
    void Update()
    {
        if (buttonDown)
            buttonHeldDown.Invoke();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
        buttonReleased.Invoke();
    }

}
