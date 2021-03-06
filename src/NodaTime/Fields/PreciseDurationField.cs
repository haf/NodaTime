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

namespace NodaTime.Fields
{
    /// <summary>
    /// Duration field class representing a field with a fixed unit length.
    /// </summary>
	[Serializable]
	internal sealed class PreciseDurationField : DurationField
    {
        internal static readonly PreciseDurationField Milliseconds = new PreciseDurationField(DurationFieldType.Milliseconds, NodaConstants.TicksPerMillisecond);
        internal static readonly PreciseDurationField Seconds = new PreciseDurationField(DurationFieldType.Seconds, NodaConstants.TicksPerSecond);
        internal static readonly PreciseDurationField Minutes = new PreciseDurationField(DurationFieldType.Minutes, NodaConstants.TicksPerMinute);
        internal static readonly PreciseDurationField Hours = new PreciseDurationField(DurationFieldType.Hours, NodaConstants.TicksPerHour);
        internal static readonly PreciseDurationField HalfDays = new PreciseDurationField(DurationFieldType.HalfDays, NodaConstants.TicksPerStandardDay / 2);
        internal static readonly PreciseDurationField Days = new PreciseDurationField(DurationFieldType.Days, NodaConstants.TicksPerStandardDay);
        internal static readonly PreciseDurationField Weeks = new PreciseDurationField(DurationFieldType.Weeks, NodaConstants.TicksPerStandardWeek);

        internal PreciseDurationField(DurationFieldType type, long unitTicks) : base(type, unitTicks, true, true)
        {
        }

        internal override long GetInt64Value(Duration duration, LocalInstant localInstant)
        {
            return duration.Ticks / UnitTicks;
        }

        internal override Duration GetDuration(long value, LocalInstant localInstant)
        {
            return new Duration(value * UnitTicks);
        }

        internal override LocalInstant Add(LocalInstant localInstant, int value)
        {
            return new LocalInstant(localInstant.Ticks + value * UnitTicks);
        }

        internal override LocalInstant Add(LocalInstant localInstant, long value)
        {
            return new LocalInstant(localInstant.Ticks + value * UnitTicks);
        }

        internal override long GetInt64Difference(LocalInstant minuendInstant, LocalInstant subtrahendInstant)
        {
            return (minuendInstant.Ticks - subtrahendInstant.Ticks) / UnitTicks;
        }
    }
}