using System.Collections.Generic;
using UnityEngine;

public class SquidBossBehaviour : EnemyBehaviour
{
	public List<GameObject> Tentacles;
	private int m_MaxTentacleCount = -1;
    private int m_CurrentPhase = 1;
    
    public void RemoveTentacle(GameObject tentacle)
	{
		Tentacles.Remove(tentacle);
    }

    protected override void OnInit()
	{
		m_MaxTentacleCount = Tentacles.Count;
		
		SetState(new SquidBossIdleState(this));
    }

    public override void OnDeath()
    {
        base.OnDeath();

        //Set state to death
        SetState(new SquidBossDeathState(this));
    }
}
