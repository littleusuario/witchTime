public class State_SlimeDeath : State_EnemyDeath
{
    SecondEnemy enemy;
    public State_SlimeDeath(Enemy enemy)
    {
        this.enemy = (SecondEnemy)enemy;
    }

    public override void EnterState()
    {
        enemy.Death = true;
    }
}
