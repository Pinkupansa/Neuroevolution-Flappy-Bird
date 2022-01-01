using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{
    private void Update()
    {
        if(canPlay)
        {
            if(Input.GetButtonDown("Jump"))
            {
                controlledBird.Jump();
            }
        }
        
    }
}
