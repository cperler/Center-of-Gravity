using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CG
{
    public enum BlockType
    {
        Person,
        Luggage
    }

    class Block
    {
        #region Members
        string _name;
        double _weight;
        BlockType _type;
        Location _location;
        bool _locked;
        int _id;
        ArrayList _excludeLocations;
        #endregion

        #region Constructors
        public Block(string pName, double pWeight, BlockType pType)
        {
            _name = pName;
            _weight = pWeight;
            _type = pType;
            _location = null;
            _locked = false;
            _id = -1;
            _excludeLocations = new ArrayList();
        }

        public Block(string pName, double pWeight, BlockType pType, Location pLockedLocation)
        {
            _name = pName;
            _weight = pWeight;
            _type = pType;
            _location = null;
            _locked = false;
            _id = -1;
            _excludeLocations = new ArrayList();
            LockInPosition(pLockedLocation);
        }
        #endregion

        public void ExcludeLocation(Location pLocation)
        {
            _excludeLocations.Add(pLocation);
        }

        public bool ExcludesLocation(Location pLocation)
        {
            return _excludeLocations.Contains(pLocation);
        }

        public void SetPosition(Location pLocation)
        {
            if (_locked)
            {
                return;
            }

            _location = pLocation;
            pLocation.Blocks.Add(this);
        }

        public void ClearPosition()
        {
            if (!LockedInPosition && _location != null)
            {
                _location.Blocks.Remove(this);
                _location = null;
            }
        }

        public void LockInPosition(Location pLocation)
        {
            pLocation.Blocks.Add(this);
            _location = pLocation;
            _locked = true;
        }

        #region Public Accessors
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public double Weight
        {
            get
            {
                return _weight;
            }
        }

        public BlockType Type
        {
            get
            {
                return _type;
            }
        }

        public Location Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public bool LockedInPosition
        {
            get
            {
                return _locked;
            }
        }

        public ArrayList ExcludeLocations
        {
            get
            {
                return _excludeLocations;
            }
        }
        #endregion

        public override string ToString()
        {
            if (_type == BlockType.Person)
            {
                return _name + " (a person that weighs " + _weight + " pounds)";
            }
            else
            {
                return _name + " (a piece of luggage that weighs " + _weight + " pounds)";
            }
        }
    }
}
