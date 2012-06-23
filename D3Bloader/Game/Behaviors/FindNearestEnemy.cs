using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3_Adventures.Structures;
using D3_Adventures;
using TreeSharp;

namespace D3Bloader.Game
{
    public class FindNearestEnemeyDecorator
    {

        public Sequence this[Bot owner]
        {
            get
            {
                var seq = new Sequence(

                //first we must not already have a target.
                new Decorator(ret => !Helpers.hasValidTarget(owner), new TreeSharp.Action(ret => RunStatus.Success)),

                //go grab a target
                new TreeSharp.Action(ret =>
                {
                    owner.CurrentTarget = Helpers.getNearestEnemy();
                })

            );

            return seq;

            }
        
        }        
    }
}
