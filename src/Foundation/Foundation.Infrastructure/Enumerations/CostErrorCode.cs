/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: CostErrorCode.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Error for cost module
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// Cost error code enum
    /// </summary>
    public enum CostErrorCode
    {
        /// <summary>
        /// EffectiveDataIsOverRange
        /// </summary>
        EffectiveDataIsOverRange = 001,

        /// <summary>
        /// PriceLessThanZero
        /// </summary>
        PriceLessThanZero = 002,
        DemandCostTypeIsEmpty = 003,
        HourPriceIsEmptyOrLessThanZero = 004,
        HourTagIdIsEmptyOrLessThanZero = 005,
        HourTagTypeIsEmptyOrWrong = 006,
        TouTariffIdIsWrong = 007,
        TransformerPriceLessThanZero = 008,
        TransformerCapacityLessThanZero = 009,
        ReactiveTagIdIsWrong = 010,
        ReactiveTagTypeIsWrong = 011,
        RealTagIdIsWrong = 012,
        RealTagTypeIsWrong = 013,
        PaddingCostLessThanZero = 014,
        CostHasExisted = 015,
        CostHasBeenUpdated = 016,
        CostHierarchyIsDeleted = 017,
        CostHierarchyIsWrong = 018,
        HourTagIsWrong = 019,
        ReactiveTagIsWrong = 020,
        RealTagIsWrong = 021,
        TouTariffIsWrong = 022
    }
}
