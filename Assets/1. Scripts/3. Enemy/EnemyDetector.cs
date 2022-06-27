using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public int radius;
    public LayerMask layer;

	private void Update()
	{
        Collider[] Ct = Physics.OverlapSphere(this.transform.position, radius, layer);

        if(Ct.Length >= 2)
		{
            Vector3 pos1 = Ct[0].transform.position - this.transform.position;
            Vector3 pos2 = Ct[1].transform.position - this.transform.position;

            if(pos1.magnitude < pos2.magnitude)
			{
                gameObject.SendMessageUpwards("OnCkTarget", Ct[0].gameObject, SendMessageOptions.DontRequireReceiver);
			}
            else
			{
                gameObject.SendMessageUpwards("OnCkTarget", Ct[1].gameObject, SendMessageOptions.DontRequireReceiver);
			}
        }
	}
}
