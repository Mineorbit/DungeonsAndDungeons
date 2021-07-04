using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class Spike : InteractiveLevelObject
    {
        private int spikeDamage = 50;
        public GameObject[] spikes;
        // AUTOMATICALLY CONNECT TO ALL NEIGHBORING ON PLAY

        public Hitbox Hitbox;
        public Collider buildCollider;
        public override void OnStartRound()
        {
            base.OnStartRound();
            buildCollider.enabled = false;
            Hitbox.Attach("Entity");
            Hitbox.enterEvent.AddListener((x)=>TryDamage(x));
        }

        private void TryDamage(GameObject g)
        {
            Vector3 dir = g.transform.position - transform.position;
            dir.Normalize();
            GameConsole.Log($"Direction: {Vector3.Dot(transform.up,dir)}");
            if(Vector3.Dot(transform.up,dir)> 0.7f)
            {
                var c = g.GetComponentInParent<Entity>(true);
                if (c != null)
                {
                    c.Hit( this, spikeDamage);
                }
            }
        }
        public override void OnEndRound()
        {
            base.OnEndRound();
            Invoke(Deactivate);
            buildCollider.enabled = false;
        }
        
        public override void Activate()
        {
            base.Activate();
            foreach (GameObject g in spikes)
            {
                g.SetActive(false);
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            foreach (GameObject g in spikes)
            {
                g.SetActive(true);
            }
        }
    }
}