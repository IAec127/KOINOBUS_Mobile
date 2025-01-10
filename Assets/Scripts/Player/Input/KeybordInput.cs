using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybordInput : PlayerInput
{
    // Start is called before the first frame update
    public new void Start()
    {
        //if(photonView.IsMine)
        {
            base.Start();
            Yaw_Pitch_Role_Sensitivity = playerMove.Data.KeyboardSens;
        }
    }

    // Update is called once per frame
    public new void Update()
    {
		//if (photonView.IsMine)
		{
		    base.Update();

            totalGyro = Vector3.zero;

            // デバック用のキー入力
            if (Input.GetKey(KeyCode.W))
            {
                totalGyro.y = 0.5f;
            }
            if (Input.GetKey(KeyCode.S))
            {
			    totalGyro.y = -0.5f;
		    }
		    if (Input.GetKey(KeyCode.A))
            {
			    totalGyro.z = -0.5f;
		    }
		    if (Input.GetKey(KeyCode.D))
            {
			    totalGyro.z = 0.5f;
		    }

            // アクセル
            if (Input.GetKey(KeyCode.Space))
            {
                float speed = (0.2f + (playerMove.Data.Item_acceleration * playerMove.GetItems[(int)Item.ITEM_EFFECT.ACCELERATION - 1]));
				speed = CheckCanBoost(speed);
				if (speed > 0.0f)
				{
					playerMove.Speed += speed;
					playerMove.IsAccel = true;
				}
            }

            // ブレーキ
            if(Input.GetKey(KeyCode.V))
            {
                playerMove.IsBrake = true;
            }

			if (Input.GetKey(KeyCode.F))
            {
                playerMove.IsCharging = true;
            }
			if (Input.GetKeyUp(KeyCode.F))
            {
                playerMove.IsShot = true;
            }
		}

    }

	public override void ResetGyro()
    {
		totalGyro = Vector3.zero;
		transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
        playerMove.Speed = playerMove.Data.MinSpeed;

	}

}
