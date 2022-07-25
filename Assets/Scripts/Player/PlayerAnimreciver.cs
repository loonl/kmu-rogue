using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimreciver : MonoBehaviour
{
    public System.Action onDieComplete;
    public System.Action onAttackComplete;

    // executed at the end of animation death
    public void OnDieComplete()
    {
        if (onDieComplete != null)
        {
            this.onDieComplete();
        }
    }

    // executed at the end of animation attack
    public void OnAttackComplete()
    {
        if (onAttackComplete != null)
        {
            this.onAttackComplete();
        }
    }
}
