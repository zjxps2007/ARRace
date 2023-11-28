using TMPro;
using UnityEngine;

public class ButtonEvent : PlayerControl
{
    
    [SerializeField]
    protected TextMeshProUGUI coin;
    
    public void MovingForwardUp()
    {
        isMovingForward = false;
    }
    
    public void MovingForwardDown()
    {
        isMovingForward = true;
    }

    public void MovingBackwardUp()
    {
        isMovingBackward = false;
    }
    
    public void MovingBackwardDown()
    {
        isMovingBackward = true;
    }

    public void MovingLeftUp()
    {
        isMovingLeft = false;
    }
    
    public void MovingLeftDown()
    {
        isMovingLeft = true;
    }

    public void MovingRightUp()
    {
        isMovingRight = false;
    }
    
    public void MovingRightDown()
    {
        isMovingRight = true;
    }
}
