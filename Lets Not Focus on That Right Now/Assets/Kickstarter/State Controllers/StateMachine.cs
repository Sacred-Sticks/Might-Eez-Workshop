using System;
using System.Collections.Generic;

namespace Kickstarter.StateControllers
{
    public class StateMachine<TEnum> where TEnum : Enum
    {
        private StateMachine(TEnum initialState, Dictionary<TEnum, List<TEnum>> transitions,
            Dictionary<TEnum, List<Action>> entryListeners, Dictionary<TEnum, List<Action>> exitListeners)
        {
            stateTransitions = transitions;
            CurrentState = initialState;
            this.entryListeners = entryListeners;
            this.exitListeners = exitListeners;
        }

        private readonly Dictionary<TEnum, List<TEnum>> stateTransitions;
        private readonly Dictionary<TEnum, List<Action>> entryListeners;
        private readonly Dictionary<TEnum, List<Action>> exitListeners;

        private TEnum currentState;
        public TEnum CurrentState
        {
            get => currentState;
            set
            {
                if (!stateTransitions.ContainsKey(currentState))
                    return;
                if (!stateTransitions[currentState].Contains(value))
                    return;
                if (exitListeners.ContainsKey(currentState))
                    exitListeners[currentState].ForEach(l => l.Invoke());
                currentState = value;
                if (entryListeners.ContainsKey(currentState))
                    entryListeners[currentState].ForEach(l => l.Invoke());
            }
        }
        
        public class Builder
        {
            private TEnum initialState;
            private readonly Dictionary<TEnum, List<TEnum>> transitions;
            private readonly Dictionary<TEnum, List<Action>> entryListeners;
            private readonly Dictionary<TEnum, List<Action>> exitListeners;

            public Builder()
            {
                transitions = new Dictionary<TEnum, List<TEnum>>();
                entryListeners = new Dictionary<TEnum, List<Action>>();
                exitListeners = new Dictionary<TEnum, List<Action>>();
            }

            public Builder WithInitialState(TEnum state)
            {
                initialState = state;
                return this;
            }

            public Builder WithTransition(TEnum fromState, TEnum toState)
            {
                if (!transitions.ContainsKey(fromState))
                    transitions.Add(fromState, new List<TEnum>());
                transitions[fromState].Add(toState);
                return this;
            }

            public Builder WithStateListener(TEnum state, transitionType transitionType, Action listener)
            {
                var listeners = transitionType switch
                {
                    transitionType.Start => entryListeners,
                    transitionType.End => exitListeners,
                    _ => throw new ArgumentOutOfRangeException(nameof(transitionType), transitionType, null),
                };
                if (!listeners.ContainsKey(state))
                    listeners.Add(state, new List<Action>());
                listeners[state].Add(listener);
                return this;
            }

            public StateMachine<TEnum> Build()
            {
                return new StateMachine<TEnum>(initialState, transitions, entryListeners, exitListeners);
            }
        }
    }

    public enum transitionType
    {
        Start,
        End,
    }
}
