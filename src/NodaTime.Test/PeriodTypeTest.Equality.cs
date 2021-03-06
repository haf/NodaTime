﻿#region Copyright and license information
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

using NUnit.Framework;

namespace NodaTime.Test
{
    public partial class PeriodTypeTest
    {
        [Test]
        public void Properties_YieldSingletons()
        {
            Assert.AreSame(PeriodType.Days, PeriodType.Days);
            Assert.AreSame(PeriodType.AllFields, PeriodType.AllFields);
            Assert.AreSame(PeriodType.Time, PeriodType.Time);
            Assert.AreSame(PeriodType.YearMonthDay, PeriodType.YearMonthDay);
        }

        [Test]
        public void Equals()
        {
            Assert.AreEqual(PeriodType.AllFields, PeriodType.AllFields);
            Assert.AreNotEqual(PeriodType.AllFields, PeriodType.Time);
            TestHelper.TestEqualsClass(PeriodType.YearMonthDay.WithMonthsRemoved(), PeriodType.YearDay, PeriodType.YearMonthDay);
        }
    }
}