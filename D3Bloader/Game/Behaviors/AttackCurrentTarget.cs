using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeSharp;

namespace D3Bloader.Game
{
    public class AttackCurrentTargetDecorator
    {
        public Sequence this[Bot owner]
        {
            get
            {
                var seq = new Sequence(
                    
                    //first we must have a valid target
                    new Decorator(ret => Helpers.hasValidTarget(owner), new TreeSharp.Action(ret => RunStatus.Success)),

                    //if we are already busy, don't continue.
                    new Decorator(ret => !Helpers.isInAction(owner), new TreeSharp.Action(ret => RunStatus.Success)),

                    //send the attack action.
                    new TreeSharp.Action(ret =>
                    {
                        owner.Attack(owner.CurrentTarget.Value);
                    }),

                    //wait for the action to complete (or we time out).
                    new Wait(1, ret => Helpers.isInAction(owner) , new TreeSharp.Action(ret => RunStatus.Success))
            );

            return seq;

            }
        
        }            
    }
}