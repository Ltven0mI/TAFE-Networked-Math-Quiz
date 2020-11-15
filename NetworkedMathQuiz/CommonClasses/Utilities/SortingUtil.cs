using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses.Utilities
{
    public static class SortingUtil
    {
        public static void SortArray<T>(T[] array, SortingMethod method, SortingDirection direction) where T : IComparable<T>
        {
            switch (method)
            {
                case SortingMethod.BubbleSort:
                    BubbleSort(array, direction);
                    break;
                case SortingMethod.InsertionSort:
                    InsertionSort(array, direction);
                    break;
                case SortingMethod.SelectionSort:
                    SelectionSort(array, direction);
                    break;
                default:
                    throw new NotImplementedException($"The {nameof(SortingMethod)} '{method}' is not currently supported.");
            }
        }

        private static void SelectionSort<T>(T[] array, SortingDirection direction=SortingDirection.Ascending)
            where T : IComparable<T>
        {
            for (int i=0; i<array.Length-1; i++)
            {
                for (int j=i+1; j<array.Length; j++)
                {
                    var compareResult = array[j].CompareTo(array[i]);

                    if ((direction == SortingDirection.Ascending && compareResult < 0) ||
                        (direction == SortingDirection.Descending && compareResult > 0))
                    {
                        var temp = array[j];
                        array[j] = array[i];
                        array[i] = temp;
                    }
                }
            }
        }

        private static void InsertionSort<T>(T[] array, SortingDirection direction = SortingDirection.Ascending)
            where T : IComparable<T>
        {
            for (int i=1; i<array.Length; ++i)
            {
                var key = array[i];
                var j = i - 1;
                while (j >= 0 &&
                    ((direction == SortingDirection.Ascending && array[j].CompareTo(key) > 0) ||
                    (direction == SortingDirection.Descending && array[j].CompareTo(key) < 0)))
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = key;
            }
        }

        private static void BubbleSort<T>(T[] array, SortingDirection direction = SortingDirection.Ascending)
            where T : IComparable<T>
        {
            var n = array.Length;
            for (int i=0; i<n-1; i++)
            {
                for (int j=0; j<n-i-1; j++)
                {
                    if ((direction == SortingDirection.Ascending && array[j].CompareTo(array[j + 1]) > 0) ||
                    (direction == SortingDirection.Descending && array[j].CompareTo(array[j + 1]) < 0))
                    {
                        var temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
    }
}
