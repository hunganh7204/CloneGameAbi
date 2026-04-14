using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void Attack()
    {
        ChangeAnim("throw");
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }
}
