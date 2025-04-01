using System;
using UnityEngine;
using NRandom.Collections;

namespace NRandom.Unity
{
    [Serializable]
    public class SerializableWeightedList<T> : WeightedList<T>, ISerializationCallbackReceiver
    {
        [SerializeField] WeightedValue<T>[] values;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            var span = WeightedCollectionsMarshal.AsSpan(this);
            values = new WeightedValue<T>[span.Length];
            span.CopyTo(values);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();

            WeightedCollectionsMarshal.SetCount(this, values.Length);
            values.CopyTo(WeightedCollectionsMarshal.AsSpan(this));

            var totalWeight = 0.0;
            foreach (var wv in values)
            {
                totalWeight += wv.Weight;
            }
            WeightedCollectionsMarshal.SetTotalWeight(this, totalWeight);
        }
    }
}