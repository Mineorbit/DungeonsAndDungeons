using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    public UnityEngine.Object hitboxPrefab;

    Hitbox hitBox;
    int damage = 20;
    public override void OnAttach()
    {
        base.OnAttach();
        hitBox = (Instantiate(hitboxPrefab) as GameObject).GetComponent<Hitbox>();
        hitBox.Attach(owner, "Enemy", new Vector3(0,0,1));
        hitBox.enterEvent.AddListener((x)=> {TryDamage(x); });
        hitBox.Deactivate();
    }

    public override void Use()
    {
        base.Use();
        hitBox.Activate();
    }

    void TryDamage(GameObject g)
    {
        EnemyController c = g.GetComponentInParent<EnemyController>();
        if(c!=null)
        {
            c.Hit(damage);
        }
    }

    public override void StopUse()
    {
        hitBox.Deactivate();
    }

    public override void OnDettach()
    {
        Destroy(hitBox.gameObject);
    }
    public void OnDestroy()
    {
        Destroy(hitBox.gameObject);
    }
}
