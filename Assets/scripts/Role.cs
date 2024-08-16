using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Role
{
    public int id;
    public string name;
    public int hp;
    public int damage;
    public int knockback;
    public float knockbackCooldown;
    public float attackRange;
    public float detectionZone;
    public float speed;
    public float bodyRemainTime;

}

public class RootRole
{
    public List<Role> roles = new List<Role>();

}

