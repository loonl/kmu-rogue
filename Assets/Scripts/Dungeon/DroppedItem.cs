using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    private CircleCollider2D _collider;
    // private sprite _sprite;
    private int _price = 0;

    private void Start()
    {
        _collider = this.GetComponent<CircleCollider2D>();
    }

    public void Set(int itemId, int price = 0)
    {
        // 이미지 할당

        // price가 있는 경우 상호작용 이벤트가 구매로 바뀜
            // 구매는 price를 0으로 수정하면 될듯
            // 한번 누르면 구매, 다시 구매하면 교체
        // price가 없는 경우 줍기 혹은 교체
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            // player.AddInteractEvent()
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            // player.RemoveInteractEvent()
        }
    }
}
