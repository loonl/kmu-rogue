using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private BoxCollider2D _collider;

    private Portal connetecPortal = null;
    private bool _isActivated;
    private ushort _outDirect;       // 내 포탈의 방향
    private int _connectedRoomId;

    private void Start()
    {
        _collider = this.GetComponent<BoxCollider2D>();

        // DeActivate();
    }

    public void Connect(int roomId, ushort direct)
    {
        // 포탈과 연결될 방 id 설정
        _connectedRoomId = roomId;
        _outDirect = direct;

        // 방향 지정 및 문 방향으로 scale 조정
        // if (direct == 0 || direct == 2)
        // {
        //     this.transform.localScale = new Vector3(1f, 1.1f, 1f);
        // }
        // else
        // {
        //     this.transform.localScale = new Vector3(1.1f, 1f, 1f);
        // }
    }

    // -------------------------------------------------------------
    // 포탈 활성화
    // -------------------------------------------------------------
    public void Activate()
    {
        // 포탈 활성화
        _collider.enabled = true;
        _isActivated = true;
    }

    public void DeActivate()
    {
        // 포탈 비활성화
        _collider.enabled = false;
        _isActivated = false;
    }

    public void Enter(GameObject entering)
    {
        // connetecPortal
    }

    public void Exit(GameObject exiting)
    {

    }

    // -------------------------------------------------------------
    // 포탈 활성화
    // -------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            other.transform.position = DungeonSystem.Instance.GetTargetPortalPos(_connectedRoomId, _outDirect);
        }
    }
}
