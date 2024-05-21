using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossConditioner : MonoBehaviour
{
	private BossMain BM;
	private BossStateMachine BMStateMachine;

	public BossConditioner(BossMain bm)
	{
		BM = bm;
		BMStateMachine = BM.StateMachine;
	}

	private void Start()
	{
		
	}


	
}
