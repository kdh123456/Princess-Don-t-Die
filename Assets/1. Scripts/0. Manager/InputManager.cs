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
		if(Input.GetKeyDown(KeyCode.I))
		{
			eventParam.input.isInventory = !eventParam.input.isInventory;
			EventManager.TriggerEvent("ONINVENTORY",eventParam);
		}
	}

    private void GetMoveInput()
	{
		moveInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		horizontal = moveInputs.x;
		vertical = moveInputs.y;

		eventParam.input.isAttack = Input.GetMouseButtonDown(0);
		eventParam.input.isRun = Input.GetKey(KeyCode.LeftShift);
		eventParam.input.moveVector = new Vector2(horizontal, vertical);

		EventManager.TriggerEvent("MoveInput", eventParam);
		EventManager.TriggerEvent("IsAttack", eventParam);
	}
}
