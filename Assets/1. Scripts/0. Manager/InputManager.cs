using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private float horizontal;
	private float vertical;

    private Vector2 moveInputs;

	private EventParam eventParam = new EventParam();


    void Update()
    {
		GetMoveInput();
	}

    private void GetMoveInput()
	{
		moveInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		horizontal = moveInputs.x;
		vertical = moveInputs.y;

		eventParam.isAttack = Input.GetMouseButtonDown(0);
		eventParam.runBool = Input.GetKey(KeyCode.LeftShift);
		eventParam.eventVector = new Vector2(horizontal, vertical);

		EventManager.TriggerEvent("MoveInput", eventParam);
		EventManager.TriggerEvent("IsAttack", eventParam);
	}
}
