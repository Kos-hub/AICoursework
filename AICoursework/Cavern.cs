using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AICoursework
{
    internal class Cavern
    {

        private int[] _coord = new int[2];
        private int _caveNum;
        private List<Cavern> _cavern = new List<Cavern>();
        private Cavern _parent = null;

        private double _gVal = 0;
        private double _hVal = 0;
        private double _fVal = 0;

        public int[] Coord
        {
            get
            {
                return _coord;
            }
        }

        public int Index
        {
            get
            {
                return _caveNum;
            }
        }

        public List<Cavern> Caverns
        {
            get
            {
                return _cavern;
            }
        }


        public double FVal
        {
            get => _fVal;
            set => _fVal = value;
        }

        public double GVal
        {
            get => _gVal;
            set => _gVal = value;
        }

        public double HVal
        {
            get => _hVal;
            set => _hVal = value;
        }

        public Cavern Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public Cavern(int caveNum, int x, int y)
        {
            _caveNum = caveNum + 1;
            _coord[0] = x;
            _coord[1] = y;
        }


        public void AddNeighbour(Cavern cavern)
        {
            _cavern.Add(cavern);
        }
    }
}
