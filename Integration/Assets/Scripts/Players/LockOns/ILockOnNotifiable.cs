namespace Assets.Scripts.Players.LockOns
{
    public interface ILockOnNotifiable
    {

        /// <summary>
        /// When the target gets locked-on.
        /// </summary>
        void OnLockOn();

        /// <summary>
        /// When the lock-on breaks.
        /// </summary>
        void OnLockBreak();

        /// <summary>
        /// When the lock-on is voluntarily released.
        /// </summary>
        void OnUnlock();
    }
}