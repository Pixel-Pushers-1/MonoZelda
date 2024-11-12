using MonoZelda.Trigger;
using MonoZelda.Link;
using System;

namespace MonoZelda.Commands.CollisionCommands
{
    public class PlayerTriggerCollisionCommand : ICommand
    {
        public PlayerTriggerCollisionCommand()
        {
            //empty
        }

        public void Execute(params object[] metadata)
        {
            if (metadata[3] is Direction direction)
            {
                if (metadata[0] is ITrigger t1)
                {
                    t1.Trigger(direction);
                }
                if (metadata[1] is ITrigger t2)
                {
                    t2.Trigger(direction);
                }
                if (metadata[2] is ITrigger t3)
                {
                    t3.Trigger(direction);
                }
            }

        }

        public void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
}
