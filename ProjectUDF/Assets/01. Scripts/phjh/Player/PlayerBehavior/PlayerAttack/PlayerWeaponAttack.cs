public abstract class PlayerWeaponAttack : PlayerBAttack
{
    protected float SetCooltime;
    protected float nowTime;

    protected override void TryAttack()
    {
        PlayerMain.Instance.OnAttackEvent.Invoke();
    }

    protected abstract void OnAttackStart();    //공격준비/차징 및 공격 범위를 보여준다
    protected abstract void OnAttacking();      //직접적인 공격을 처리해준다
    protected abstract void OnAttackEnd();      //공격 후의 뒷처리를 해준다
}
