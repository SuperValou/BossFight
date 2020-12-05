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

        /// <summary>
        /// When any lockable target appears in range
        /// </summary>
        void OnLockableInSight();

        /// <summary>
        /// When all lockable targets are no longer in range
        /// </summary>
        void OnLockableOutOfSight();
    }
}