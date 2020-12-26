namespace Assets.Scripts.Foes.ArtificialIntelligences.TargetTracking
{
    public interface ITargetTrackingStateMachine : IStateMachine
    {
        void QuietUpdate();
        void AlertUpdate();
        void HostileUpdate();

        void OnBecomeQuiet();
        void OnBecomeAlert();
        void OnBecomeHostile();
    }
}