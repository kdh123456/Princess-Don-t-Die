using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface OnHIt
{
	public void OnHit(int a);

	public int Atk { get; set; }
}
