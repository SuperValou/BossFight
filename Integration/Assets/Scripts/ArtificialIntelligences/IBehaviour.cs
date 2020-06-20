namespace Assets.Scripts.ArtificialIntelligences
{
    /// <summary>
    /// The behaviour of some State-Machine, in a specific state.
    /// This behaviour can be something like "Walking", "Waiting", "Attacking", "Looking Around", etc.
    /// </summary>
    public interface IBehaviour
    {
    }

    /// <summary>
    /// The behaviour of a specific State-Machine, in a specific state.
    /// This behaviour can be something like "Walking", "Waiting", "Attacking", "Looking Around", etc.
    /// </summary>
    public interface IBehaviour<TStateMachine> : IBehaviour
        where TStateMachine : IStateMachine
    {
        /// <summary>
        /// Initialize the behaviour from the given state-machine.
        /// </summary>
        void Initialize(TStateMachine stateMachine);
    }
}