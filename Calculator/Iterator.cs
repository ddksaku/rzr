using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Calculator
{
    public class Iterator
    {
        /// <summary>
        /// Iterates through all possible hand combinations given a number of shared cards, 
        /// the number of dead cards, and the total number of cards. Precalculated values
        /// are used to speed up this calculation. 
        /// </summary>
        /// <param name="shared"></param>
        /// <param name="dead"></param>
        /// <param name="numberOfCards"></param>
        /// <returns></returns>
        public static IEnumerable<ulong> Hands(ulong shared, ulong dead, int numberOfCards)
        {
            int _i1, _i2, _i3, _i4, _i5, _i6, _i7, length;
            ulong _card1, _card2, _card3, _card4, _card5, _card6, _card7;
            ulong _n2, _n3, _n4, _n5, _n6;

            dead |= shared;

            switch (numberOfCards - PreCalc.BitCount(shared))
            {
                case 7:
                    for (_i1 = Values.NumberOfCards - 1; _i1 >= 0; _i1--)
                    {
                        _card1 = PreCalc.CardMasksTable[_i1];
                        if ((dead & _card1) != 0) continue;
                        for (_i2 = _i1 - 1; _i2 >= 0; _i2--)
                        {
                            _card2 = PreCalc.CardMasksTable[_i2];
                            if ((dead & _card2) != 0) continue;
                            _n2 = _card1 | _card2;
                            for (_i3 = _i2 - 1; _i3 >= 0; _i3--)
                            {
                                _card3 = PreCalc.CardMasksTable[_i3];
                                if ((dead & _card3) != 0) continue;
                                _n3 = _n2 | _card3;
                                for (_i4 = _i3 - 1; _i4 >= 0; _i4--)
                                {
                                    _card4 = PreCalc.CardMasksTable[_i4];
                                    if ((dead & _card4) != 0) continue;
                                    _n4 = _n3 | _card4;
                                    for (_i5 = _i4 - 1; _i5 >= 0; _i5--)
                                    {
                                        _card5 = PreCalc.CardMasksTable[_i5];
                                        if ((dead & _card5) != 0) continue;
                                        _n5 = _n4 | _card5;
                                        for (_i6 = _i5 - 1; _i6 >= 0; _i6--)
                                        {
                                            _card6 = PreCalc.CardMasksTable[_i6];
                                            if ((dead & _card6) != 0) continue;
                                            _n6 = _n5 | _card6;
                                            for (_i7 = _i6 - 1; _i7 >= 0; _i7--)
                                            {
                                                _card7 = PreCalc.CardMasksTable[_i7];
                                                if ((dead & _card7) != 0) continue;
                                                yield return _n6 | _card7 | shared;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 6:
                    for (_i1 = Values.NumberOfCards - 1; _i1 >= 0; _i1--)
                    {
                        _card1 = PreCalc.CardMasksTable[_i1];
                        if ((dead & _card1) != 0) continue;
                        for (_i2 = _i1 - 1; _i2 >= 0; _i2--)
                        {
                            _card2 = PreCalc.CardMasksTable[_i2];
                            if ((dead & _card2) != 0) continue;
                            _n2 = _card1 | _card2;
                            for (_i3 = _i2 - 1; _i3 >= 0; _i3--)
                            {
                                _card3 = PreCalc.CardMasksTable[_i3];
                                if ((dead & _card3) != 0) continue;
                                _n3 = _n2 | _card3;
                                for (_i4 = _i3 - 1; _i4 >= 0; _i4--)
                                {
                                    _card4 = PreCalc.CardMasksTable[_i4];
                                    if ((dead & _card4) != 0) continue;
                                    _n4 = _n3 | _card4;
                                    for (_i5 = _i4 - 1; _i5 >= 0; _i5--)
                                    {
                                        _card5 = PreCalc.CardMasksTable[_i5];
                                        if ((dead & _card5) != 0) continue;
                                        _n5 = _n4 | _card5;
                                        for (_i6 = _i5 - 1; _i6 >= 0; _i6--)
                                        {
                                            _card6 = PreCalc.CardMasksTable[_i6];
                                            if ((dead & _card6) != 0)
                                                continue;
                                            yield return _n5 | _card6 | shared;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 5:
                    for (_i1 = Values.NumberOfCards - 1; _i1 >= 0; _i1--)
                    {
                        _card1 = PreCalc.CardMasksTable[_i1];
                        if ((dead & _card1) != 0) continue;
                        for (_i2 = _i1 - 1; _i2 >= 0; _i2--)
                        {
                            _card2 = PreCalc.CardMasksTable[_i2];
                            if ((dead & _card2) != 0) continue;
                            _n2 = _card1 | _card2;
                            for (_i3 = _i2 - 1; _i3 >= 0; _i3--)
                            {
                                _card3 = PreCalc.CardMasksTable[_i3];
                                if ((dead & _card3) != 0) continue;
                                _n3 = _n2 | _card3;
                                for (_i4 = _i3 - 1; _i4 >= 0; _i4--)
                                {
                                    _card4 = PreCalc.CardMasksTable[_i4];
                                    if ((dead & _card4) != 0) continue;
                                    _n4 = _n3 | _card4;
                                    for (_i5 = _i4 - 1; _i5 >= 0; _i5--)
                                    {
                                        _card5 = PreCalc.CardMasksTable[_i5];
                                        if ((dead & _card5) != 0) continue;
                                        yield return _n4 | _card5 | shared;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 4:
                    for (_i1 = Values.NumberOfCards - 1; _i1 >= 0; _i1--)
                    {
                        _card1 = PreCalc.CardMasksTable[_i1];
                        if ((dead & _card1) != 0) continue;
                        for (_i2 = _i1 - 1; _i2 >= 0; _i2--)
                        {
                            _card2 = PreCalc.CardMasksTable[_i2];
                            if ((dead & _card2) != 0) continue;
                            _n2 = _card1 | _card2;
                            for (_i3 = _i2 - 1; _i3 >= 0; _i3--)
                            {
                                _card3 = PreCalc.CardMasksTable[_i3];
                                if ((dead & _card3) != 0) continue;
                                _n3 = _n2 | _card3;
                                for (_i4 = _i3 - 1; _i4 >= 0; _i4--)
                                {
                                    _card4 = PreCalc.CardMasksTable[_i4];
                                    if ((dead & _card4) != 0) continue;
                                    yield return _n3 | _card4 | shared;
                                }
                            }
                        }
                    }

                    break;
                case 3:
                    for (_i1 = Values.NumberOfCards - 1; _i1 >= 0; _i1--)
                    {
                        _card1 = PreCalc.CardMasksTable[_i1];
                        if ((dead & _card1) != 0) continue;
                        for (_i2 = _i1 - 1; _i2 >= 0; _i2--)
                        {
                            _card2 = PreCalc.CardMasksTable[_i2];
                            if ((dead & _card2) != 0) continue;
                            _n2 = _card1 | _card2;
                            for (_i3 = _i2 - 1; _i3 >= 0; _i3--)
                            {
                                _card3 = PreCalc.CardMasksTable[_i3];
                                if ((dead & _card3) != 0) continue;
                                yield return _n2 | _card3 | shared;
                            }
                        }
                    }
                    break;
                case 2:
                    length = PreCalc.TwoCardTable.Length;
                    for (_i1 = 0; _i1 < length; _i1++)
                    {
                        _card1 = PreCalc.TwoCardTable[_i1];
                        if ((dead & _card1) != 0) continue;
                        yield return _card1 | shared;
                    }
                    break;
                case 1:
                    length = PreCalc.CardMasksTable.Length;
                    for (_i1 = 0; _i1 < length; _i1++)
                    {
                        _card1 = PreCalc.CardMasksTable[_i1];
                        if ((dead & _card1) != 0) continue;
                        yield return _card1 | shared;
                    }
                    break;
                case 0:
                    yield return shared;
                    break;
                default:
                    yield return 0UL;
                    break;
            }
        }

        /*
         * Need to implement a random hands calculation for up to 10,000 hands
         */
        public static IEnumerable<ulong> Random(ulong dead, int numberOfCards, int numberOfIterations)
        {
            Random rand = new Random();
            ulong ret = 0;

            for (int i = 0; i < numberOfIterations; i++)
            {
                int card = rand.Next(Values.NumberOfCards);
                ulong myCard = PreCalc.CardMasksTable[card];
                for (int j = 0; j < numberOfCards; j++)
                {
                    while ((myCard & (dead | ret)) != 0)
                    {
                        card = rand.Next(Values.NumberOfCards);
                        myCard = PreCalc.CardMasksTable[card];
                    }

                    card = rand.Next(Values.NumberOfCards);
                    myCard = PreCalc.CardMasksTable[card];

                    ret = (ret | myCard);
                }
                yield return ret;
            }
        }
    }
}
