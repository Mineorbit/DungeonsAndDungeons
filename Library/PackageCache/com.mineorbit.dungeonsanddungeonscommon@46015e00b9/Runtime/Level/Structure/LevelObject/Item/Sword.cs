using UnityEngine;
using UnityEngine.Events;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class Sword : Item
    {
        public Object hitboxPrefab;

        public ParticleSystem effectSystem;

        public UnityEvent onHitEvent = new UnityEvent();

        private bool block;

        private readonly int damage = 20;
        
        private Hitbox hitBox;


        private Vector3 lastPosition = new Vector3(0, 0, 0);

        private readonly float whipeTime = 0.02f;

        private void Start()
        {
            lastPosition = transform.position;
        }


        Vector3 holdOffset = new Vector3(0, 0.00181f, 0);
        Vector3 holdRotation = new Vector3(0, -75, 90f);
        Vector3 hitboxRotation = new Vector3(0, 135, 90);
        public void OnDestroy()
        {
            if (hitBox != null)
                Destroy(hitBox.gameObject);
        }

        public override void OnAttach()
        {
            base.OnAttach();
            transform.localPosition = holdOffset;
            transform.localEulerAngles = holdRotation;
            hitBox = (Instantiate(hitboxPrefab) as GameObject).GetComponent<Hitbox>();
            hitBox.Attach(owner.transform.Find("Model").gameObject, "Entity", new Vector3(0, 0, -2));
            hitBox.transform.localEulerAngles = hitboxRotation;

            hitBox.enterEvent.AddListener(x => { TryDamage(x); });
            hitBox.Deactivate();
        }

        public override void Use()
        {
            if (!block)
            {
                block = true;
                base.Use();

                owner.baseAnimator.Strike();
                hitBox.transform.localEulerAngles = hitboxRotation;
                hitBox.Activate();

                // factor out just like audio component
                effectSystem.Play();
                TimerManager.StartTimer(whipeTime, () => { effectSystem.Stop(); });
            }
        }


        private void TryDamage(GameObject g)
        {
            var c = g.GetComponentInParent<Entity>(true);
            if (c != null && c != owner)
            {
                onHitEvent.Invoke();
                c.Hit(owner, damage);
            }
        }

        public override void StopUse()
        {
            base.StopUse();
            effectSystem.Stop();
            //transform.localEulerAngles = new Vector3(0, -75, 90f);
            hitBox.Deactivate();
            block = false;
        }

        public override void OnDettach()
        {
            base.OnDettach();
            if (hitBox != null)
                Destroy(hitBox.gameObject);
        }
    }
}