using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
public class CatStateMachine : AbstractFiniteStateMachine
{
    public enum CatFSM
    {
        SIT,
        WALK,
        TALK,
        BOND,
        REJECT,
        ACCEPT
    }
    private void Awake()
    {
        Init(CatFSM.WALK,
            AbstractState.Create<SitState, CatFSM>(CatFSM.SIT, this),
            AbstractState.Create<WalkState, CatFSM>(CatFSM.WALK, this),
            AbstractState.Create<TalkState, CatFSM>(CatFSM.TALK, this),
            AbstractState.Create<BondState, CatFSM>(CatFSM.BOND, this),
            AbstractState.Create<RejectState, CatFSM>(CatFSM.REJECT, this),
            AbstractState.Create<AcceptState, CatFSM>(CatFSM.ACCEPT, this)
        );
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
    public class WalkState : AbstractState
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
