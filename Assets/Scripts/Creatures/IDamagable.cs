namespace Game
{
    public interface IDamagable
    {
        void TakeDamage(float amount);

        void Kill();
    }
}