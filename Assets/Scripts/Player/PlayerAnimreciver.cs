using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimreciver : MonoBehaviour
{
    // 호출할 델리게이트
    public System.Action onDieComplete;
    public System.Action onAttackComplete;

    // Death 애니메이션 끝에 달아줄 메소드
    public void OnDieComplete()
    {
        if (onDieComplete != null)
        {
            this.onDieComplete();
        }
    }

    // Attack 애니메이션 끝에 달아줄 메소드
    public void OnAttackComplete()
    {
        if (onAttackComplete != null)
        {
            this.onAttackComplete();
        }
    }
}
