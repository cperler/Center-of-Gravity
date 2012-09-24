using System;
using System.Collections.Generic;
using System.Text;

namespace CG
{
    class Position
    {
        #region Members
        int _maxPeople;
        int _maxLuggage;
        #endregion

        #region Constructor
        public Position(int pMaxPeople, int pMaxLuggage)
        {
            _maxPeople = pMaxPeople;
            _maxLuggage = pMaxLuggage;
        }
        #endregion

        #region Public Accessors
        public int MaxPeople
        {
            get
            {
                return _maxPeople;
            }
        }

        public int MaxLuggage
        {
            get
            {
                return _maxLuggage;
            }
        }
        #endregion
    }
}
