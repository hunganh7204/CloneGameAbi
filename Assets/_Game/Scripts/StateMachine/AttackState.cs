using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackState : IState
{
    private float timer;
    public void OnEnter(Enemy enemy)
    {
        
        if (enemy.Target != null)
        {
            //xoay vao huong player
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
            enemy.StopMoving();
            enemy.Attack();
        }
        timer = 0f;
    }
    public void OnExcute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer >= 1.5f)
        {
            enemy.ChangeState(new PatrolState());
        }

    }
    public void OnExit(Enemy enemy)
    {

    }
}
