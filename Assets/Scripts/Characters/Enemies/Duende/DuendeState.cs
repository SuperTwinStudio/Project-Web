using System.Collections;
using UnityEngine;

public class DuendeState : EnemyState
{

    protected DuendeBehaviour Duende => Behaviour as DuendeBehaviour;

    public DuendeState(EnemyBehaviour behaviour) : base(behaviour) { }
}
