using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CG
{
    class Vehicle
    {
        #region Members
        ArrayList _solutions;
        ArrayList _solutionHash;
        int _solutionCount;

        string _name;
        ArrayList _locations;
        ArrayList _blocks;
        double _emptyWeight;
        double _momentOfInertia;
        double _gasWeight;
        double _startingGas;
        double _gasUsed;
        double _gasMomentSlope;
        double _minCG;
        double _maxCG;
        double _maxWeight;
        #endregion

        #region Constructors
        public Vehicle(string pName, double pEmptyWeight, double pMomentOfInertia, double pGasWeight, 
            double pStartingGas, double pGasUsed, double pGasMomentSlope, double pMaxWeight)
        {
            _solutionHash = new ArrayList();
            _solutions = new ArrayList();
            _solutionCount = 0;

            _name = pName;
            _blocks = new ArrayList();
            _locations = new ArrayList();
            _emptyWeight = pEmptyWeight;
            _momentOfInertia = pMomentOfInertia;
            _gasWeight = pGasWeight;
            _startingGas = pStartingGas;
            _gasUsed = pGasUsed;
            _gasMomentSlope = pGasMomentSlope;
            _maxWeight = pMaxWeight;
        }
        #endregion

        #region Initialization
        private double AddBlocks(Block[] pBlocks)
        {
            double mTotalBlockWeight = 0;
            foreach (Block b in pBlocks)
            {
                Console.WriteLine("Adding " + b);
                AddBlock(b);
                mTotalBlockWeight += b.Weight;
            }


            foreach (Location l in _locations)
            {
                mTotalBlockWeight -= l.MaxWeight;
            }

            return mTotalBlockWeight;
        }

        private void AddBlock(Block pBlock)
        {
            pBlock.ID = _blocks.Count;
            _blocks.Add(pBlock);
        }

        public void AddLocations(Location[] pLocations)
        {
            foreach (Location l in pLocations)
            {
                Console.WriteLine("Adding Location: " + l);
                _locations.Add(l);
            }
        }

        public bool InitCGRange()
        {
            Console.WriteLine();
            double mTotalWeight = _emptyWeight;
            foreach (Block b in _blocks)
            {
                mTotalWeight += b.Weight;
            }
            mTotalWeight += (_startingGas * _gasWeight);
            Console.Write("Enter the Min CG for " + mTotalWeight + " pounds: ");
            try
            {
                _minCG = double.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Invalid number.");
                return false;
            }
            Console.Write("Enter the Max CG for " + mTotalWeight + " pounds: ");
            try
            {
                _maxCG = double.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Invalid number.");
                return false;
            }
            return true;
        }
        #endregion

        public Hashtable Positions()
        {
            Hashtable mAllPositions = new Hashtable();
            foreach (Location l in _locations)
            {
                for (int i = 0; i < l.MaxPositions(); i++)
                {
                    mAllPositions.Add(mAllPositions.Count, l);
                }
            }
            return mAllPositions;
        }

        #region Public Accessors
        public double MinCG
        {
            get
            {
                return _minCG;
            }
            set
            {
                _minCG = value;
            }
        }

        public double MaxCG
        {
            get
            {
                return _maxCG;
            }
            set
            {
                _maxCG = value;
            }
        }
        public ArrayList Blocks
        {
            get
            {
                return _blocks;
            }
        }        

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public ArrayList Locations
        {
            get
            {
                return _locations;
            }
        }

        public double EmptyWeight
        {
            get
            {
                return _emptyWeight;
            }
        }

        public double MomentOfInertia
        {
            get
            {
                return _momentOfInertia;
            }
        }

        public double GasWeight
        {
            get
            {
                return _gasWeight;
            }
        }

        public double StartingGas
        {
            get
            {
                return _startingGas;
            }
        }

        public double GasUsed
        {
            get
            {
                return _gasUsed;
            }
        }

        public double GasMomentSlope
        {
            get
            {
                return _gasMomentSlope;
            }
        }

        public double MaxWeight
        {
            get
            {
                return _maxWeight;
            }
        }
        #endregion

        #region Solve
        public DateTime Solve(Block[] pBlocks)
        {
            double mOverWeight = 0;
            if ((mOverWeight = AddBlocks(pBlocks)) > 0)
            {
                Console.WriteLine("\nToo much weight - your blocks are over by " + mOverWeight + " pounds.");
                return DateTime.Now; ;
            }

            double mTotalWeight = _emptyWeight + (_gasWeight * _startingGas);
            foreach (Block b in _blocks)
            {
                mTotalWeight += b.Weight;
            }
            if (mTotalWeight > _maxWeight)
            {
                Console.WriteLine("\nToo much weight - you're (overall) over by " + (mTotalWeight - _maxWeight) + " pounds.");
                return DateTime.Now;
            }

            if (!InitCGRange())
            {
                return DateTime.Now;
            }

            _solutionHash = new ArrayList();

            int[] mCurrent = new int[_blocks.Count];
            for (int i = 0; i < mCurrent.Length; i++)
            {
                mCurrent[i] = -1;
            }

            Hashtable mPositions = Positions();
            ArrayList mAttemptedLocations = new ArrayList();
            IDictionaryEnumerator mEnum = mPositions.GetEnumerator();
            while (mEnum.MoveNext())
            {
                if (mAttemptedLocations.Contains(mEnum.Value))
                {
                    continue;
                }

                ArrayList mAlreadyAdded = new ArrayList();
                mAlreadyAdded.Add((int)mEnum.Key);

                for (int i = 0; i < mCurrent.Length; i++)
                {
                    mCurrent[i] = -1;
                }

                mAttemptedLocations.Add(mEnum.Value);
                if (Check(mCurrent, mPositions, 0, (int)mEnum.Key))
                {
                    mCurrent[0] = (int)mEnum.Key;
                    Next(mCurrent, mPositions, mAlreadyAdded, 0);
                }
            }

            Console.WriteLine("Searching " + _solutionCount + " solutions for CGs within envelope, then determining optimal CG...");

            ArrayList mCGs = new ArrayList();
            ArrayList mConfigurationHashes = new ArrayList();
            Hashtable mConfigurations = new Hashtable();
            Hashtable mConfigurationsAndRepeats = new Hashtable();
            bool mRepeatConfigurationFound = false;
            double mTotalMOI = 0;
            double mBaseMOI = _momentOfInertia;            
            double mCG = 0;
            double mHash = 0;
            string mVal = "";

            if (_gasMomentSlope != 0)
            {
                mBaseMOI += ((_gasWeight * _startingGas) / _gasMomentSlope);
            }

            foreach (int[] ilist in _solutions)
            {
                if (mConfigurationsAndRepeats.Count != 0 && mConfigurationsAndRepeats.Count % 5000 == 0)
                {
                    Console.WriteLine("Searched through " + mConfigurationsAndRepeats.Count + " solutions...");
                }

                mTotalMOI = mBaseMOI;
                mVal = "";

                for (int i = 0; i < ilist.Length; i++)
                {
                    Block mBlock = (Block)_blocks[i];
                    mBlock.ClearPosition();
                    Location mLocation = (Location)mPositions[ilist[i]];
                    mBlock.SetPosition(mLocation);
                    mVal += "\n\t" + mBlock.Name + "-" + mLocation.Name;
                }
                
                foreach (Location l in _locations)
                {
                    mTotalMOI += l.GetMoment();
                }

                mCG = mTotalMOI / mTotalWeight;
                mVal = "CG: " + mCG + "\n" + mVal;

                mHash = GetHashCode(ilist);
                if (!mConfigurationHashes.Contains(mHash) && mCG >= _minCG && mCG <= _maxCG)
                {
                    if (mConfigurations.ContainsKey(mCG))
                    {
                        mRepeatConfigurationFound = true;
                        mConfigurationsAndRepeats.Add(mConfigurationsAndRepeats.Count, mVal);
                    }
                    else
                    {
                        mConfigurationsAndRepeats.Add(mCG, mVal);
                        mConfigurations.Add(mCG, mVal);
                        mCGs.Add(mCG);
                    }
                    mConfigurationHashes.Add(mHash);
                }
            }

            if (mConfigurations.Count == 0)
            {
                Console.WriteLine("\nNo valid solutions with a CG between " + _minCG + " and " + _maxCG + ".");
                return DateTime.Now;
            }

            double mMidCG = (_maxCG + _minCG) / 2.0;
            double mDelta = double.MaxValue;
            double mOptCG = (double)mCGs[0];

            for (int i = 0; i < mCGs.Count; i++)
            {
                double mCurrentCG = (double)mCGs[i];
                if (Math.Abs(mMidCG - mCurrentCG) < mDelta)
                {
                    mOptCG = mCurrentCG;
                    mDelta = Math.Abs(mMidCG - mOptCG);
                }
            }
            
            Console.WriteLine("Number of solutions within envelope: " + mConfigurationsAndRepeats.Count);
            if (mRepeatConfigurationFound)
            {
                Console.WriteLine("Number of (additional) repeated solutions (same CG, different configuration): " + (mConfigurationsAndRepeats.Count - mConfigurations.Count));
            }
            Console.WriteLine("\nOptimal Solution (only for w/starting gas)\n");
            Console.WriteLine("Optimal CG: " + mMidCG);
            Console.WriteLine("Distance from optimal CG: " + mDelta);
            Console.Write((string)mConfigurations[mOptCG]);
            Console.WriteLine();

            DateTime mDone = DateTime.Now;

            Console.Write("\nView all solutions? (Enter Y for yes, anything else to quit.) ");
            string mResponse = Console.ReadLine();
            if (mResponse.ToUpper() == "Y")
            {
                int mSolutionCount = 0;
                foreach (string s in mConfigurationsAndRepeats.Values)
                {
                    mSolutionCount++;
                    Console.WriteLine("Solution #" + mSolutionCount);
                    Console.WriteLine(s);
                }
            }

            return mDone;
        }

        private void Next(int[] pCurrentConfiguration, Hashtable pLocations, ArrayList pAlreadyAdded, int pCurrentBlockIndex)
        {
            if (((Block)_blocks[pCurrentBlockIndex + 1]).LockedInPosition)
            {
                return;
            }

            ArrayList mAttemptedLocations = new ArrayList();
            IDictionaryEnumerator mEnum = pLocations.GetEnumerator();
            while (mEnum.MoveNext())
            {
                int i = (int)mEnum.Key;
                if (mAttemptedLocations.Contains(mEnum.Value))
                {
                    continue;
                }
                
                if (!pAlreadyAdded.Contains(i))
                {
                    ArrayList mAlreadyAdded = new ArrayList(pAlreadyAdded);
                    mAlreadyAdded.Add(i);

                    int mNextBlockIndex = pCurrentBlockIndex + 1;

                    _solutionCount++;
                    if (mNextBlockIndex < _blocks.Count)
                    {
                        for (int j = mNextBlockIndex; j < _blocks.Count; j++)
                        {
                            pCurrentConfiguration[j] = -1;
                        }

                        mAttemptedLocations.Add(mEnum.Value);
                        
                        if (Check(pCurrentConfiguration, pLocations, mNextBlockIndex, i))
                        {
                            pCurrentConfiguration[mNextBlockIndex] = i;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (mNextBlockIndex == _blocks.Count - 1)
                    {
                        long mHash = GetHashCode(pCurrentConfiguration);
                        if (!_solutionHash.Contains(mHash))
                        {
                            _solutionHash.Add(mHash);
                            _solutions.Add(pCurrentConfiguration.Clone());
                        }
                    }
                    else
                    {
                        Next(pCurrentConfiguration, pLocations, mAlreadyAdded, mNextBlockIndex);
                    }
                }
            }
        }

        private bool Check(int[] pBlockLocations, Hashtable pPositions, int pBlockIndex, int pPosition)
        {
            if (_solutionCount != 0 && _solutionCount % 5000 == 0)
            {
                Console.WriteLine("Found " + _solutionCount + " solutions...");
            }

            Location mLocation = (Location)pPositions[pPosition];
            Block mBlock = (Block)_blocks[pBlockIndex];

            if (mBlock.LockedInPosition && mBlock.Location != mLocation)
            {
                return false;
            }

            if (mBlock.ExcludesLocation(mLocation))
            {
                return false;
            }

            int mTotalPeople = 0;
            int mTotalLuggage = 0;
            double mTotalWeight = 0;

            int mIndex = -1;
            foreach (int i in pBlockLocations)
            {
                mIndex++;
                if (i != -1)
                {
                    Location mCurrentLocation = (Location)pPositions[i];
                    if (mCurrentLocation == mLocation)
                    {
                        Block mCurrentBlock = (Block)_blocks[mIndex];
                        if (mCurrentBlock.Type == BlockType.Person)
                        {
                            mTotalPeople++;
                        }
                        if (mCurrentBlock.Type == BlockType.Luggage)
                        {
                            mTotalLuggage++;
                        }
                        mTotalWeight += mCurrentBlock.Weight;
                    }
                }
            }

            if (mLocation.MaxWeight < mTotalWeight + mBlock.Weight)
            {
                return false;
            }

            bool mTypeTest = false;
            foreach (Position p in mLocation.Positions)
            {
                if ((mBlock.Type == BlockType.Person && p.MaxPeople >= mTotalPeople + 1 && p.MaxLuggage >= mTotalLuggage) ||
                   (mBlock.Type == BlockType.Luggage && p.MaxLuggage >= mTotalLuggage + 1 && p.MaxPeople >= mTotalPeople))
                {
                    mTypeTest = true;
                    break;
                }
            }

            return mTypeTest;
        }        

        private long GetHashCode(int[] pConfiguration)
        {
            long mResult = 0;
            for (int i = 0; i < pConfiguration.Length; i++)
            {
                int mVal = pConfiguration[i];
                mResult += (long)(mVal * (Math.Pow(10, i)));
            }
            return mResult;
        }
        #endregion
    }
}