namespace Assets.Scripts.ArtificialIntelligences
{
    /// <summary>
    /// A State-Machine.
    /// </summary>
    public interface IStateMachine
    {
    }

    /// <summary>
    /// A State-Machine with behaviours for its states.
    /// </summary>
    /// <typeparam name="TBehaviour"></typeparam>
    public interface IStateMachine<TBehaviour> : IStateMachine
        where TBehaviour : IBehaviour
    {
        void SetCurrentBehaviour(TBehaviour behaviour);
    }
}