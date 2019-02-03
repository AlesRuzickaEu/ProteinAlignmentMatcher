using System;
using System.Collections;
using System.Collections.Generic;

namespace MASReader
{
    public class ProteinSequenceWithFundaments : IEnumerable<KeyValuePair<int, ProteinMatch>>
    {
        public ProteinSequence ProteinSequence { get; private set; }

        private readonly Dictionary<int, ProteinMatch> _fundaments = new Dictionary<int, ProteinMatch>(); 

        public ProteinSequenceWithFundaments(ProteinSequence sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            ProteinSequence = sequence;
        }

        public ProteinMatch this[int index]
        {
            get
            {
                ProteinMatch result;
                _fundaments.TryGetValue(index, out result);
                return result;
            }
            set
            {
                _fundaments[index] = value;
            }
        }

        public IEnumerator<KeyValuePair<int, ProteinMatch>> GetEnumerator()
        {
            return _fundaments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}