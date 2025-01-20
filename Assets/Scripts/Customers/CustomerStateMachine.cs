using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
public class CustomerStateMachine : AbstractFiniteStateMachine
{
    public enum CustomerFSM
    {
        WALK,
        SIT,
        TALK,
        BOND,
        REJECT,
        ACCEPT
    }
    private void Awake()
    {
        Init(CustomerFSM.WALK,
            AbstractState.Create<WalkState, CustomerFSM>(CustomerFSM.WALK, this),
            AbstractState.Create<SitState, CustomerFSM>(CustomerFSM.SIT, this),
            AbstractState.Create<TalkState, CustomerFSM>(CustomerFSM.TALK, this),
            AbstractState.Create<BondState, CustomerFSM>(CustomerFSM.BOND, this),
            AbstractState.Create<RejectState, CustomerFSM>(CustomerFSM.REJECT, this),
            AbstractState.Create<AcceptState, CustomerFSM>(CustomerFSM.ACCEPT, this)
        );
    }
    public class WalkState : AbstractState
    {
        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
            // walk to a table
        }
        public override void OnExit()
        {
        }
    }
    public class SitState : AbstractState
    {
        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
            
        }
        public override void OnExit()
        {
        }
    }
    public class TalkState : AbstractState
    {
        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
        }
        public override void OnExit()
        {
        }
    }
    public class BondState : AbstractState
    {
        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
        }
        public override void OnExit()
        {
        }
    }
    public class RejectState : AbstractState
    {
        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
        }
        public override void OnExit()
        {
        }
    }
    public class AcceptState : AbstractState
    {
        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
        }
        public override void OnExit()
        {
        }
    }
}
