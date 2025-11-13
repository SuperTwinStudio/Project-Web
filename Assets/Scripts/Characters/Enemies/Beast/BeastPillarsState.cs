public class BeastPillarsState : BeastState {

    //Constructor
    public BeastPillarsState(EnemyBehaviour behaviour) : base(behaviour) {}

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
            //Still 2+ pillars -> Spawn minions
            foreach (var spawn in Beast.MinionSpawns) Enemy.SpawnEnemy(Beast.MinionPrefab, spawn);
        } else if (Beast.Pillars.Count == 1) {
            //Only 1 pillar remaining -> Destroy it
            BeastPillar remainingPillar = Beast.Pillars[0];
            remainingPillar.Damage(remainingPillar.Health, Enemy, DamageType.None);

            //Update walkable surface (to fix pillars holes in navmesh)
            Game.Current.Level.UpdateWalkableSurface();

            //Start rage phase
            Beast.SetState(new BeastRageState(Beast));
        }
    }

}
