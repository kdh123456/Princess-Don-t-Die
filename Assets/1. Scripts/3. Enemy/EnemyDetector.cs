using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    //트리거를 통해서 찾고자 하는 목표 태그를 설정한다
    public string targetTag = string.Empty;

    //트리거 안에 들어오면 
    private void OnTriggerEnter(Collider other)
    {
        //트리거에 충돌한 오브젝트의 tag가 트루 이냐
        //compareTag가
        // if(other.gameObject.tag ==  targetTag) 보다 비교하기 좋다 
        if (other.gameObject.CompareTag(targetTag) == true)
        {
            //해당 오브젝트의 위로 샌드메시지를 발송한. 함수명, 샌드메시지 출발 위치 , 샌드메시지 받을 곳에 대한 옵션 필수냐 필수가 아니냐 설정 
            gameObject.SendMessageUpwards("OnCkTarget", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}
