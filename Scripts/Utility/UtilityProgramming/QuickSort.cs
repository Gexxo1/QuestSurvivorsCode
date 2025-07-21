using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort
{
    public static void Sort(int[] arr, bool isAscending)
    {
        QuickSortRecursive(arr, 0, arr.Length - 1, isAscending);
    }

    private static void QuickSortRecursive(int[] arr, int low, int high, bool isAscending)
    {
        if (low < high)
        {
            int partitionIndex = Partition(arr, low, high, isAscending);
            QuickSortRecursive(arr, low, partitionIndex - 1, isAscending);
            QuickSortRecursive(arr, partitionIndex + 1, high, isAscending);
        }
    }

    private static int Partition(int[] arr, int low, int high, bool isAscending)
    {
        int pivot = arr[high];
        int i = low - 1;
        if(isAscending) {
            for (int j = low; j < high; j++) {
                if (arr[j] < pivot) {
                    i++;
                    Swap(arr, i, j);
                }
            }
        }
        else {
            for (int j = low; j < high; j++) {
                if (arr[j] > pivot) {
                    i++;
                    Swap(arr, i, j);
                }
            }
        }
        

        Swap(arr, i + 1, high);
        return i + 1;
    }

    private static void Swap(int[] arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}
