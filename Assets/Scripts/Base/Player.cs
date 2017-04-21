using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Player : TopDownSpriteEntity
{
    protected override void HandleUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            MoveDir += Vector3.up;
        else if (Input.GetKey(KeyCode.S))
            MoveDir += Vector3.down;

        if (Input.GetKey(KeyCode.A))
            MoveDir += Vector3.left;
        else if (Input.GetKey(KeyCode.D))
            MoveDir += Vector3.right;
        base.HandleUpdate();
    }
}
