using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Screener.Models
{
    /// <summary>
    /// Collection of market data informations collected from the web.
    /// </summary>
    internal class MarketDataEntities : ICollection<MarketDataEntity>
    {
        private readonly List<MarketDataEntity> _entities = new List<MarketDataEntity>();

        /// <summary>
        /// Constructor.
        /// </summary>
        internal MarketDataEntities()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entities"></param>
        internal MarketDataEntities(IEnumerable<MarketDataEntity> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (!entities.Any())
            {
                throw new ArgumentException(nameof(entities));
            }
            _entities.AddRange(entities);
        }

        /// <summary>
        /// Performs the specified action on each element of the collection.
        /// </summary>
        /// <param name="action"></param>
        internal void ForEach(Action<MarketDataEntity> action) => _entities.ForEach(action);

        /// <summary>
        /// Sorts the market data informations.
        /// </summary>
        internal void Sort() => _entities.Sort();

        /// <summary>
        /// Gets the number of market data informations contained in the collection.
        /// </summary>
        public int Count => _entities.Count;

        /// <summary>
        /// Gets a value indicating whether the ICollection is read-only.
        /// </summary>
        public bool IsReadOnly => ((ICollection<MarketDataEntity>)_entities).IsReadOnly;

        /// <summary>
        /// Adds a market data information to the collection.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(MarketDataEntity entity)
        {
            var actualOnThatDay = _entities
                .FirstOrDefault(e =>
                    String.Equals(e.Name, entity.Name) 
                    && Equals(e.DateTime.Date, entity.DateTime.Date));

            var infoIsAddable = actualOnThatDay is null;
            bool infoIsUpdateable = infoIsAddable ? false : entity.DateTime > actualOnThatDay.DateTime;

            if (infoIsUpdateable)
            {
                Remove(actualOnThatDay);
            }
            if(infoIsAddable || infoIsUpdateable)
            {
                _entities.Add(entity);
            }
        }

        /// <summary>
        /// Adds the market data informations to the collection.
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<MarketDataEntity> entities)
        {
            entities.ToList().ForEach(Add);
            Console.WriteLine($"Number of market data records added: {entities.Count()}");
        }

        /// <summary>
        /// Removes all information from the collection.
        /// </summary>
        public void Clear() => _entities.Clear();

        public bool Contains(MarketDataEntity item) => _entities.Contains(item);

        public void CopyTo(MarketDataEntity[] array, int arrayIndex) => _entities.CopyTo(array, arrayIndex);

        public IEnumerator<MarketDataEntity> GetEnumerator() => ((IEnumerable<MarketDataEntity>)_entities).GetEnumerator();

        public bool Remove(MarketDataEntity item) => _entities.Remove(item);

        internal DateTime MaxDateTime()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<MarketDataEntity>)_entities).GetEnumerator();
    }
}
