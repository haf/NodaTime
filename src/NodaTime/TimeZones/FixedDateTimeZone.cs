#region Copyright and license information
// Copyright 2001-2009 Stephen Colebourne
// Copyright 2009-2011 Jon Skeet
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Globalization;
using NodaTime.Utility;

namespace NodaTime.TimeZones
{
    /// <summary>
    /// Basic <see cref="DateTimeZone" /> implementation that has a fixed name key and offset i.e.
    /// no daylight savings.
    /// </summary>
    /// <remarks>
    /// This type is thread-safe and immutable.
    /// </remarks>
	[Serializable]
	internal sealed class FixedDateTimeZone : DateTimeZone, IEquatable<FixedDateTimeZone>
    {
        private readonly Offset offset;
        private readonly ZoneInterval interval;
        private readonly ZoneIntervalPair intervalPair;

        /// <summary>
        /// Creates a new fixed time zone.
        /// </summary>
        /// <param name="offset">The <see cref="Offset"/> from UTC.</param>
        public FixedDateTimeZone(Offset offset) : this(MakeId(offset), offset)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedDateTimeZone"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="offset">The offset.</param>
        public FixedDateTimeZone(string id, Offset offset) : base(id, true, offset, offset)
        {
            this.offset = offset;
            interval = new ZoneInterval(id, Instant.MinValue, Instant.MaxValue, offset, Offset.Zero);
            intervalPair = ZoneIntervalPair.Unambiguous(interval);
        }

        /// <summary>
        /// Makes the id for this time zone. The format is "UTC+/-Offset".
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>The generated id string.</returns>
        private static string MakeId(Offset offset)
        {
            if (offset == Offset.Zero)
            {
                return DateTimeZone.UtcId;
            }
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", DateTimeZone.UtcId, offset.ToString("g"));
        }

        /// <summary>
        /// Gets the zone interval for the given instant. This implementation always returns the same interval.
        /// </summary>
        public override ZoneInterval GetZoneInterval(Instant instant)
        {
            return interval;
        }

        /// <summary>
        /// Gets the zone interval pair for the given instant. This implementation always returns the same unambiguous interval pair.
        /// </summary>
        internal override ZoneIntervalPair GetZoneIntervals(LocalInstant localInstant)
        {
            return intervalPair;
        }

        /// <summary>
        /// Returns the offset from UTC, where a positive duration indicates that local time is later
        /// than UTC. In other words, local time = UTC + offset.
        /// </summary>
        /// <param name="instant">The instant for which to calculate the offset.</param>
        /// <returns>
        /// The offset from UTC at the specified instant.
        /// </returns>
        public override Offset GetOffsetFromUtc(Instant instant)
        {
            return offset;
        }

        /// <summary>
        /// Writes the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        internal override void Write(DateTimeZoneWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            writer.WriteOffset(offset);
        }

        /// <summary>
        /// Reads the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static DateTimeZone Read(DateTimeZoneReader reader, string id)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            var offset = reader.ReadOffset();
            return new FixedDateTimeZone(id, offset);
        }

        #region Implementation of IEquatable<FixedDateTimeZone>
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(FixedDateTimeZone other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return IsFixed == other.IsFixed && offset == other.offset && Id == other.Id;
        }
        #endregion

        #region Object overrides
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. 
        ///                 </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.
        ///                 </exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return ReferenceEquals(this, obj) || Equals(obj as FixedDateTimeZone);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, IsFixed);
            hash = HashCodeHelper.Hash(hash, offset);
            hash = HashCodeHelper.Hash(hash, Id);
            return hash;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Id;
        }
        #endregion // Object overrides
    }
}