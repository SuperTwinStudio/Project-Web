public class BeastPillarsState : BeastState {

    //Constructor
    public BeastPillarsState(EnemyBehaviour behaviour) : base(behaviour) { }

    //Actions
    public override void OnEnter() {
        Beast.AddOnPillarDestroyed(OnPillarDestroyed);
    }

    public override void OnExit() {
        Beast.RemoveOnPillarDestroyed(OnPillarDestroyed);
    }

    //Helpers
    private void OnPillarDestroyed(BeastPillar pillar) {
        //Check remaining pillars
        if (Beast.Pillars.Count >= 2) {
            //Still 2+ columns -> Spawn enemies on the broken pillar
            Beast.SpawnEnemy(Beast.MinionPrefab, Beast.MinionSpawn1);
            Beast.SpawnEnemy(Beast.MinionPrefab, Beast.MinionSpawn2);
        } else if (Beast.Pillars.Count == 1) {
            //Only 1 column remaining -> Destroy it
            BeastPillar remainingPillar = Beast.Pillars[0];
            remainingPillar.Damage(remainingPillar.Health, Enemy, DamageType.None);

            //Start rage phase
            Beast.SetState(new BeastRageState(Beast));
        }
    }

}
