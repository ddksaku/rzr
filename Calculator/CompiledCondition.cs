using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public class CompiledCondition
    {
        public ConditionContainer Container { get; private set; }

        public string ID { get { return Container.ID; } }

        public string Name { get { return Container.Name; } }

        public string Group { get { return Container.Group; } }

        public double Probability { get { return Container.ExpectedProbability; } }

        protected CompiledConditionWorker[] _andConditions;

        protected CompiledConditionWorker[] _orConditions;

        public CompiledCondition(ConditionContainer container, ConditionService service)
        {
            Container = container;
            List<CompiledConditionWorker> and = new List<CompiledConditionWorker>();
            if (container.AndConditions != null)
            {
                foreach (ConditionAtom atom in container.AndConditions)
                {
                    if (atom.Type == ConditionAtomType.Standard)
                    {
                        and.Add(new CompiledMaskCondition(atom.PrimaryMask));
                    }
                    else
                    {
                        ConditionContainer linked = service.Definition.ConfiguredConditions.Find(x => x.ID == atom.LinkedContainerId);
                        CompiledCondition subCondition = new CompiledCondition(linked, service);
                        and.Add(new CompiledSubCondition(subCondition));
                    }
                }
            }
            _andConditions = and.ToArray();


            List<CompiledConditionWorker> or = new List<CompiledConditionWorker>();
            if (container.OrConditions != null)
            {
                foreach (ConditionAtom atom in container.OrConditions)
                {
                    if (atom.Type == ConditionAtomType.Standard)
                    {
                        or.Add(new CompiledMaskCondition(atom.PrimaryMask));
                    }
                    else
                    {
                        ConditionContainer linked = service.Definition.ConfiguredConditions.Find(x => x.ID == atom.LinkedContainerId);
                        CompiledCondition subCondition = new CompiledCondition(linked, service);
                        or.Add(new CompiledSubCondition(subCondition));
                    }
                }
            }
            _orConditions = or.ToArray();
        }

        public bool Matches(HandMask mask)
        {
            foreach (CompiledConditionWorker atom in _andConditions)
            {
                if (!atom.Matches(mask))
                    return false;
            }

            foreach (CompiledConditionWorker atom in _orConditions)
            {
                if (atom.Matches(mask))
                    return true;
            }

            return _orConditions.Length == 0;
        }

        protected interface CompiledConditionWorker
        {
            bool Matches(HandMask mask);
        }

        protected class CompiledMaskCondition : CompiledConditionWorker
        {
            protected ulong _mask;

            public CompiledMaskCondition(ulong mask)
            {
                _mask = mask;
            }

            public bool Matches(HandMask mask)
            {
                ulong result = _mask & mask.Mask;
                return (result == _mask);
            }
        }

        protected class CompiledSubCondition : CompiledConditionWorker
        {
            protected CompiledCondition _condition;

            public CompiledSubCondition(CompiledCondition condition)
            {
                _condition = condition;
            }

            public bool Matches(HandMask mask)
            {
                return _condition.Matches(mask);
            }
        }        
    }
}
