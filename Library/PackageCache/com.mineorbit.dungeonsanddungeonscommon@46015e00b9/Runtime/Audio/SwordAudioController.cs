namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class SwordAudioController : AudioController
    {
        public Sword sword;

        private void Start()
        {
            sword.onUseEvent.AddListener(Swoosh);
            sword.onHitEvent.AddListener(Hit);
        }

        private void Swoosh()
        {
            Play(0);
        }

        private void Hit()
        {
            Play(1);
        }
    }
}