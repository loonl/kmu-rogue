using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimreciver : MonoBehaviour
{
    // ȣ���� ��������Ʈ
    public System.Action onDieComplete;
    public System.Action onAttackComplete;

    // Death �ִϸ��̼� ���� �޾��� �޼ҵ�
    public void OnDieComplete()
    {
        if (onDieComplete != null)
        {
            this.onDieComplete();
        }
    }

    // Attack �ִϸ��̼� ���� �޾��� �޼ҵ�
    public void OnAttackComplete()
    {
        if (onAttackComplete != null)
        {
            this.onAttackComplete();
        }
    }
}
