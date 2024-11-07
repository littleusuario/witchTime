public class State_SlimeNormal : State_EnemyNormal
{
    SecondEnemy enemy;
    public State_SlimeNormal(Enemy enemy)
    {
        this.enemy = (SecondEnemy)enemy;
    }

    public override void UpdateState()
    {
        if (enemy.HealthPoints <= 0)
        {
            StateManager.SwitchState(StateManager.enemyDeathState);
        }
    }
}
