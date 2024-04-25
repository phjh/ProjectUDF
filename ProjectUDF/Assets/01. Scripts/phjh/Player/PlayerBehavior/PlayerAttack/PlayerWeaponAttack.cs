using UnityEngine;

public abstract class PlayerWeaponAttack : PlayerAttack
{
    [SerializeField]
    protected float timeToAttacking;
    [SerializeField]
    protected float timeToEnd;

    protected bool _showRange;

    protected virtual bool CanAttack()
    {
        if (PlayerMain.Instance.isAttacking)
        {
            Debug.Log("cooltime");
            return false;
        }
        return true;
    }
    protected override void TryAttack()
    {
        PlayerMain.Instance.OnAttackEvent?.Invoke();
    }

    public abstract void OnAttackPrepare();     //공격준비/차징 및 공격 범위를 보여준다
    protected abstract void OnAttackStart();    //직접적인 공격을 처리해준다
    protected abstract void OnAttacking();      //공격 후의 뒷처리를 해준다
    protected abstract void OnAttackEnd();      //공격 쿨타임 같은 마지막 작업을 해준다

}
