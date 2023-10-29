using System;

using MMABooksTools;
using MMABooksProps;
using MMABooksDB;

using System.Collections.Generic;

namespace MMABooksBusiness
{
    public class State : BaseBusiness
    {
        public string Abbreviation
        {
            get
            {
                return ((StateProps)mProps).Code;
            }

            set
            {
                if (!(value == ((StateProps)mProps).Code))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 2)
                    {
                        mRules.RuleBroken("Abbreviation", false);
                        ((StateProps)mProps).Code = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Abbreviation must be no more than 2 characters long.");
                    }
                }
            }
        }

        public string Name
        {
            get
            {
                return ((StateProps)mProps).Name;
            }

            set
            {
                if (!(value == ((StateProps)mProps).Name))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 20)
                    {
                        mRules.RuleBroken("Name", false);
                        ((StateProps)mProps).Name = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Name must be no more than 20 characters long.");
                    }
                }
            }
        }

        public override object GetList()
        {
            List<State> states = new List<State>();
            List<StateProps> props = new List<StateProps>();


            props = (List<StateProps>)mdbReadable.RetrieveAll();
            foreach (StateProps prop in props)
            {
                State s = new State(prop);
                states.Add(s);
            }

            return states;
        }

        protected override void SetDefaultProperties()
        {
        }

        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("Abbreviation", true);
            mRules.RuleBroken("Name", true);
        }

        protected override void SetUp()
        {
            mProps = new StateProps();
            mOldProps = new StateProps();

            mdbReadable = new StateDB();
            mdbWriteable = new StateDB();
        }

        #region constructors
        public State() : base()
        {
        }

        public State(string key)
            : base(key)
        {
        }

        private State(StateProps props)
            : base(props)
        {
        }

        #endregion
    }
}
