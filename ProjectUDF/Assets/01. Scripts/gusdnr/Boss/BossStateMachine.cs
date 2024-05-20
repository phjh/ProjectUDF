public class BossStateMachine
{
	public BossPattern CurrentPattern { get; set; }

	public void Initialize(BossPattern startingPattern)
	{
		CurrentPattern = startingPattern;
		CurrentPattern?.EnterPattern();
	}

	public void ChangeState(BossPattern newPattern)
	{
		CurrentPattern?.ExitPattern();
		CurrentPattern = newPattern;
		CurrentPattern?.EnterPattern();
	}

	public void CancelAttack() => CurrentPattern?.ExitPattern();
}
