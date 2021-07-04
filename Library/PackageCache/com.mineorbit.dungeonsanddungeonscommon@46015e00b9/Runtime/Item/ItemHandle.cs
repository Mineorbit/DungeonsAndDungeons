using System.Collections;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class ItemHandle : MonoBehaviour
    {
        public enum HandleType
        {
            LeftHand,
            RightHand
        }

        public Item slot;
        public HandleType handleType;
        
        private PlayerController p;
        private Entity player;

        private void Start()
        {
            p = GetComponentInParent<PlayerController>();
            player = GetComponentInParent<Player>();
        }


        public void Attach(Item item)
        {
            item.transform.position = transform.position;
            item.transform.localRotation = new Quaternion(0, 0, 0, 0);
            item.transform.parent = transform;
            slot = item;
            if (!player.items.Contains(item)) player.items.Add(item);
            slot.OnAttach();
        }


        public void Dettach()
        {
            if (slot != null)
            {
                LevelManager.currentLevel.AddToDynamic(slot.gameObject, slot.gameObject.transform.position,
                    new Quaternion(0, 0, 0, 0));
                player.items.Remove(slot);
                slot.OnDettach();
                slot = null;
            }
        }

        public void Use()
        {
            if (slot != null)
            {
                slot.Use();

                var waitingStarted = false;
                if (slot.GetType() == typeof(Sword))
                {
                    waitingStarted = true;
                    UseWait();
                }

                if (waitingStarted)
                    player.setMovementStatus(false);
            }
            else
            {
                player.setMovementStatus(true);
            }
        }


        public virtual void UseWait()
        {
            StartCoroutine(useWaitTime(slot.useTime));
        }

        private IEnumerator useWaitTime(float t)
        {
            yield return new WaitForSeconds(t);
            StopUse();
            player.setMovementStatus(true);
        }


        public void StopUse()
        {
            if (slot != null)
                slot.StopUse();
            player.setMovementStatus(true);
        }
    }
}