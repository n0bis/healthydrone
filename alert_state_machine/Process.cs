using System;
using System.Collections.Generic;
using alert_state_machine.Commands;
using alert_state_machine.States;

namespace alert_state_machine
{
    public class Process
    {
        class StateTransition
        {
            private readonly ProcessState CurrentState;
            private readonly Command Command;

            public StateTransition(ProcessState currentState, Command command)
            {
                this.CurrentState = currentState;
                this.Command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }

        private Dictionary<StateTransition, ProcessState> transitions;
        public ProcessState CurrentState { get; private set; }

        public Process()
        {
            CurrentState = ProcessState.Inactive;
            transitions = new Dictionary<StateTransition, ProcessState>
            {
                { new StateTransition(ProcessState.Inactive, Command.Active),ProcessState.Active },
                { new StateTransition(ProcessState.Active, Command.Terminated),ProcessState.Inactive },
                { new StateTransition(ProcessState.Active, Command.Raised),ProcessState.Raised },
                { new StateTransition(ProcessState.Raised, Command.Handled), ProcessState.Inactive }
            };
        }

        public ProcessState GetNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            ProcessState nextState;
            if(!transitions.TryGetValue(transition,out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return nextState;
        }

        public ProcessState MoveNext(Command command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }
    }
    
}