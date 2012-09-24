using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CG
{
    class Location
    {
        #region Members
        string _name;
        double _maxWeight;
        double _momentSlope;
        ArrayList _positions;
        ArrayList _blocks;
        #endregion

        #region Constructor
        public Location(string pName, double pMaxWeight, double pMomentSlope)
        {
            _name = pName;
            _maxWeight = pMaxWeight;
            _momentSlope = pMomentSlope;
            _positions = new ArrayList();
            _blocks = new ArrayList();
        }
        #endregion

        public void AddPosition(int pMaxPeople, int pMaxLuggage)
        {
            _positions.Add(new Position(pMaxPeople, pMaxLuggage));
        }

        public double GetMoment()
        {
            double mTotalWeight = 0;
            foreach (Block b in _blocks)
            {
                mTotalWeight += b.Weight;
            }
            return (mTotalWeight / _momentSlope);
        }

        public int MaxPositions()
        {
            int mMaxPositions = 0;
            foreach (Position p in _positions)
            {
                mMaxPositions = Math.Max(mMaxPositions, p.MaxLuggage + p.MaxPeople);
            }
            return mMaxPositions;
        }

        #region Public Accessors
        public string Name
        {
            get
            {
                return _name;
            }
        }        

        public double MaxWeight
        {
            get
            {
                return _maxWeight;
            }
        }

        public double MomentSlope
        {
            get
            {
                return _momentSlope;
            }
        }

        public ArrayList Positions
        {
            get
            {
                return _positions;
            }
        }

        public ArrayList Blocks
        {
            get
            {
                return _blocks;
            }
        }
        #endregion

        public override string ToString()
        {
            return _name + " (has a max weight of " + _maxWeight + ")";
        }
    }
}