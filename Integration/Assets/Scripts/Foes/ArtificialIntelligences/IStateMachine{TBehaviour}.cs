namespace Assets.Scripts.Foes.ArtificialIntelligences
{
    /// <inheritdoc />
    /// <summary>
    /// A State-Machine with behaviours for its states.
    /// </summary>
    /// <typeparam name="TBehaviour"></typeparam>
    public interface IStateMachine<in TBehaviour> : IStateMachine
        where TBehaviour : IBehaviour
    {
        void SetCurrentBehaviour(TBehaviour behaviour);
    }
}