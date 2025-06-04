using UnityEngine;

public class User
{
    [SerializeField] private int _hp;
    [SerializeField] private int _damage;

    public User(int hp, int damage)
    {
        _hp = hp;
        _damage = damage;
    }
}
