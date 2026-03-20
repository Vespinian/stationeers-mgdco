using System.Collections.Generic;
using Assets.Scripts.Objects;
using Assets.Scripts.Atmospherics;

namespace MoreGasDisplayConsoleOptions
{
    static class MGDCOPatchHelper
    {
        public enum PatchGasDisplayMode
        {
            Debug = 0, // Having a 0 value in the Dict is important to avoid errors on game load.
            // pre 1.14 version had enum values up to 63 so skipped to 100 for a clean slate
            Pressure = 100, // Vanilla behavior for Pressure
            PrecisePressure,
            Temperature,
            TemperatureKelvin,
            EnergyConvected,
            EnergyRadiated,
            EnergyLatent,
            TotalMoles,
            TotalGaseousMoles,
            TotalLiquidMoles,
            TotalLiquidVolume,
            // O2
            RatioO2,
            QuantityO2,
            RatioGaseousO2,
            QuantityGaseousO2,
            RatioLiquidO2,
            QuantityLiquidO2,
            VolumeLiquidO2,
            // Methane
            RatioCH4,
            QuantityCH4,
            RatioGaseousCH4,
            QuantityGaseousCH4,
            RatioLiquidCH4,
            QuantityLiquidCH4,
            VolumeLiquidCH4,
            // Pollutant
            RatioPol,
            QuantityPol,
            RatioGaseousPol,
            QuantityGaseousPol,
            RatioLiquidPol,
            QuantityLiquidPol,
            VolumeLiquidPol,
            // CO2
            RatioCO2,
            QuantityCO2,
            RatioGaseousCO2,
            QuantityGaseousCO2,
            RatioLiquidCO2,
            QuantityLiquidCO2,
            VolumeLiquidCO2,
            // Nitrogen
            RatioN,
            QuantityN,
            RatioGaseousN,
            QuantityGaseousN,
            RatioLiquidN,
            QuantityLiquidN,
            VolumeLiquidN,
            // N2O
            RatioN2O,
            QuantityN2O,
            RatioGaseousN2O,
            QuantityGaseousN2O,
            RatioLiquidN2O,
            QuantityLiquidN2O,
            VolumeLiquidN2O,
            // H2O
            RatioH2O,
            QuantityH2O,
            RatioSteam,
            QuantitySteam,
            RatioWater,
            QuantityWater,
            VolumeWater,
            RatioPollutedH2O,
            QuantityPollutedH2O,
            VolumePollutedH2O,
            // Helium
            RatioHe,
            QuantityHe,
            // Hydrazine
            RatioHydrazine,
            QuantityHydrazine,
            RatioGaseousHydrazine,
            QuantityGaseousHydrazine,
            RatioLiquidHydrazine,
            QuantityLiquidHydrazine,
            VolumeLiquidHydrazine,
            // Hydrogen
            RatioH2,
            QuantityH2,
            RatioGaseousH2,
            QuantityGaseousH2,
            RatioLiquidH2,
            QuantityLiquidH2,
            VolumeLiquidH2,
            // Alcohol
            RatioLiquidAlcohol,
            QuantityLiquidAlcohol,
            VolumeLiquidAlcohol,
            // Ozone
            RatioO3,
            QuantityO3,
            RatioGaseousO3,
            QuantityGaseousO3,
            RatioLiquidO3,
            QuantityLiquidO3,
            VolumeLiquidO3,
            // Silanol
            RatioSiH4,
            QuantitySiH4,
            RatioGaseousSiH4,
            QuantityGaseousSiH4,
            RatioLiquidSiH4,
            QuantityLiquidSiH4,
            VolumeLiquidSiH4,
            // Sodium Chloride
            RatioNaCl,
            QuantityNaCl,
            VolumeNaCl,
            // Hydrochloric Acid
            RatioHCl,
            QuantityHCl,
            RatioGaseousHCl,
            QuantityGaseousHCl,
            RatioLiquidHCl,
            QuantityLiquidHCl,
            VolumeLiquidHCl,

            TotalDisplays,
        }
        public enum PatchDataType
        {
            Pressure,
            Temperature,
            Ratio,
            Quantity,
            Volume,
            Energy
        }

        // Changed enum ordering in 1.14 to have pressure at 100 as the first element
        // because the new gasses squeezed it in an akward place.
        // This function to to make the transition seemless
        public static int fixEnumOrdering(int flag)
        {
            if (flag == 62) // old Enum value of Pressure
            {
                flag = (int)MGDCOPatchHelper.PatchGasDisplayMode.Pressure;
            }
            else if (flag < (int)MGDCOPatchHelper.PatchGasDisplayMode.Pressure)
            {
                flag += (int)MGDCOPatchHelper.PatchGasDisplayMode.Pressure + 1;
            }
            return flag;
        }

        // Tag, DisplayTitle , DisplayUnits, ToggleModeButtonText, Gas type, Patch Type, Combine liquid and gas for ratio and quantity data types
        public static readonly Dictionary<int, (string tag, string displayName, string unit, string displayModeButton, Chemistry.GasType? gasType, PatchDataType dataType, bool combined)> GasData
            = new Dictionary<int, (string, string, string, string, Chemistry.GasType?, PatchDataType, bool)> {
            {(int)PatchGasDisplayMode.Debug,            ("DEBUG", "DEBUG",       "dbg",  "Mode: <b>Debug</b>", null, PatchDataType.Pressure, false)},
            {(int)PatchGasDisplayMode.Pressure,            ("P", "PRESSURE",       "Pa",  "Mode: <b>Pressure</b>", null, PatchDataType.Pressure, false)},
            {(int)PatchGasDisplayMode.PrecisePressure,     ("PP", "PRESSURE",      "Pa",  "Mode: <b>Precise Pressure</b>",    null, PatchDataType.Pressure, false)},
            {(int)PatchGasDisplayMode.Temperature,         ("TC", "TEMPERATURE",   "°C",  "Mode: <b>Temperature (°C)</b>",    null, PatchDataType.Temperature, false)},
            {(int)PatchGasDisplayMode.TemperatureKelvin,   ("TK", "TEMPERATURE",   "K",   "Mode: <b>Temperature (K)</b>",     null, PatchDataType.Temperature, false)},
            {(int)PatchGasDisplayMode.TotalMoles,          ("M", "TOTAL MOLES",    "mol", "Mode: <b>Total (mol)</b>",         null, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.TotalGaseousMoles,   ("GM", "TOTAL GASEOUS", "mol", "Mode: <b>Total Gas (mol)</b>",     null, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.TotalLiquidMoles,    ("LM", "TOTAL LIQUID",  "mol", "Mode: <b>Total Liquid (mol)</b>",  Chemistry.GasType.Undefined, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.TotalLiquidVolume,   ("LV", "TOTAL LIQUID",  "L",   "Mode: <b>Total Liquid (L)</b>",    null, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.EnergyConvected,     ("EC", "CONVECTED",     "J",   "Mode: <b>Convected (J)</b>",       null, PatchDataType.Energy, false)},
            {(int)PatchGasDisplayMode.EnergyRadiated,      ("ER", "RADIATED",      "J",   "Mode: <b>Radiated (J)</b>",        null, PatchDataType.Energy, false)},
            {(int)PatchGasDisplayMode.EnergyLatent,        ("EL", "LATENT",        "J",   "Mode: <b>Latent (J)</b>",          null, PatchDataType.Energy, false)},
            // O2
            {(int)PatchGasDisplayMode.RatioO2,             ("O2R", "O2",           "%",    "Mode: <b>O₂ (%)</b>",            Chemistry.GasType.Oxygen, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityO2,          ("O2M", "O2",           "mol",  "Mode: <b>O₂ (mol)</b>",          Chemistry.GasType.Oxygen, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousO2,      ("GO2R", "GASEOUS O2",  "%",    "Mode: <b>Gaseous O₂ (%)</b>",    Chemistry.GasType.Oxygen, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousO2,   ("GO2M", "GASEOUS O2",  "mol",  "Mode: <b>Gaseous O₂ (mol)</b>",  Chemistry.GasType.Oxygen, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidO2,       ("LO2R", "LIQUID O2",   "%",    "Mode: <b>Liquid O₂ (%)</b>",     Chemistry.GasType.LiquidOxygen, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidO2,      ("LO2V", "LIQUID O2",   "L",    "Mode: <b>Liquid O₂ (L)</b>",     Chemistry.GasType.LiquidOxygen, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidO2,    ("LO2M", "LIQUID O2",   "mol",  "Mode: <b>Liquid O₂ (mol)</b>",   Chemistry.GasType.LiquidOxygen, PatchDataType.Quantity, false)},
            // Nitrogen
            {(int)PatchGasDisplayMode.RatioN,              ("NR", "N",           "%",    "Mode: <b>N (%)</b>",           Chemistry.GasType.Nitrogen, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityN,           ("NM", "N",           "mol",  "Mode: <b>N (mol)</b>",         Chemistry.GasType.Nitrogen, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousN,       ("NR", "GASEOUS N",   "%",    "Mode: <b>Gaseous N (%)</b>",   Chemistry.GasType.Nitrogen, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousN,    ("GNM", "GASEOUS N",  "mol",  "Mode: <b>Gaseous N (mol)</b>", Chemistry.GasType.Nitrogen, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidN,        ("LNR", "LIQUID N",   "%",    "Mode: <b>Liquid N (%)</b>",    Chemistry.GasType.LiquidNitrogen, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidN,       ("LNV", "LIQUID N",   "L",    "Mode: <b>Liquid N (L)</b>",    Chemistry.GasType.LiquidNitrogen, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidN,     ("LNM", "LIQUID N",   "mol",  "Mode: <b>Liquid N (mol)</b>",  Chemistry.GasType.LiquidNitrogen, PatchDataType.Quantity, false)},
            // CO2
            {(int)PatchGasDisplayMode.RatioCO2,            ("CO2R", "CO2",           "%",    "Mode: <b>CO₂ (%)</b>",              Chemistry.GasType.CarbonDioxide, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityCO2,         ("CO2M", "CO2",           "mol",  "Mode: <b>CO₂ (mol)</b>",            Chemistry.GasType.CarbonDioxide, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousCO2,     ("GCO2R", "GASEOUS CO2",  "%",    "Mode: <b>Gaseous CO₂ (%)</b>",      Chemistry.GasType.CarbonDioxide, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousCO2,  ("GCO2M", "GASEOUS CO2",  "mol",  "Mode: <b>Gaseous CO₂ (mol)</b>",    Chemistry.GasType.CarbonDioxide, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidCO2,      ("LCO2R", "LIQUID CO2",   "%",    "Mode: <b>Liquid CO₂ (%)</b>",       Chemistry.GasType.LiquidCarbonDioxide, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidCO2,     ("LCO2V", "LIQUID CO2",   "L",    "Mode: <b>Liquid CO₂ (L)</b>",       Chemistry.GasType.LiquidCarbonDioxide, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidCO2,   ("LCO2M", "LIQUID CO2",   "mol",  "Mode: <b>Liquid CO₂ (mol)</b>",     Chemistry.GasType.LiquidCarbonDioxide, PatchDataType.Quantity, false)},
            // Pol
            {(int)PatchGasDisplayMode.RatioPol,            ("POLR", "POL",           "%",    "Mode: <b>POL (%)</b>",              Chemistry.GasType.Pollutant, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityPol,         ("POLM", "POL",           "mol",  "Mode: <b>POL (mol)</b>",            Chemistry.GasType.Pollutant, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousPol,     ("GPOLR", "GASEOUS POL",  "%",    "Mode: <b>Gaseous POL (%)</b>",      Chemistry.GasType.Pollutant, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousPol,  ("GPOLM", "GASEOUS POL",  "mol",  "Mode: <b>Gaseous POL (mol)</b>",    Chemistry.GasType.Pollutant, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidPol,      ("LPOLR", "LIQUID POL",   "%",    "Mode: <b>Liquid POL (%)</b>",       Chemistry.GasType.LiquidPollutant, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidPol,     ("LPOLV", "LIQUID POL",   "L",    "Mode: <b>Liquid POL (L)</b>",       Chemistry.GasType.LiquidPollutant, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidPol,   ("LPOLM", "LIQUID POL",   "mol",  "Mode: <b>Liquid POL (mol)</b>",     Chemistry.GasType.LiquidPollutant, PatchDataType.Quantity, false)},
            // Methane
            {(int)PatchGasDisplayMode.RatioCH4,            ("CH4R", "CH4",           "%",    "Mode: <b>Methane (CH₄) (%)</b>",              Chemistry.GasType.Methane, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityCH4,         ("CH4M", "CH4",           "mol",  "Mode: <b>Methane (CH₄) (mol)</b>",            Chemistry.GasType.Methane, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousCH4,     ("GCH4R", "GASEOUS CH4",  "%",    "Mode: <b>Gaseous CH₄ (%)</b>",      Chemistry.GasType.Methane, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousCH4,  ("GCH4M", "GASEOUS CH4",  "mol",  "Mode: <b>Gaseous CH₄ (mol)</b>",    Chemistry.GasType.Methane, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidCH4,      ("LCH4R", "LIQUID CH4",   "%",    "Mode: <b>Liquid CH₄ (%)</b>",       Chemistry.GasType.LiquidMethane, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidCH4,     ("LCH4V", "LIQUID CH4",   "L",    "Mode: <b>Liquid CH₄ (L)</b>",       Chemistry.GasType.LiquidMethane, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidCH4,   ("LCH4M", "LIQUID CH4",   "mol",  "Mode: <b>Liquid CH₄ (mol)</b>",     Chemistry.GasType.LiquidMethane, PatchDataType.Quantity, false)},
            // N2O
            {(int)PatchGasDisplayMode.RatioN2O,            ("N2OR", "N2O",           "%",    "Mode: <b>N₂O (%)</b>",              Chemistry.GasType.NitrousOxide, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityN2O,         ("N2OM", "N2O",           "mol",  "Mode: <b>N₂O (mol)</b>",            Chemistry.GasType.NitrousOxide, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousN2O,     ("GN2OR", "GASEOUS N2O",  "%",    "Mode: <b>Gaseous N₂O (%)</b>",      Chemistry.GasType.NitrousOxide, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousN2O,  ("GN2OM", "GASEOUS N2O",  "mol",  "Mode: <b>Gaseous N₂O (mol)</b>",    Chemistry.GasType.NitrousOxide, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidN2O,      ("LN2OR", "LIQUID N2O",   "%",    "Mode: <b>Liquid N₂O (%)</b>",       Chemistry.GasType.LiquidNitrousOxide, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidN2O,     ("LN2OV", "LIQUID N2O",   "L",    "Mode: <b>Liquid N₂O (L)</b>",       Chemistry.GasType.LiquidNitrousOxide, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidN2O,   ("LN2OM", "LIQUID N2O",   "mol",  "Mode: <b>Liquid N₂O (mol)</b>",     Chemistry.GasType.LiquidNitrousOxide, PatchDataType.Quantity, false)},
            // H2O
            {(int)PatchGasDisplayMode.RatioH2O,            ("H2OP ", "H2O",            "%",    "Mode: <b>H₂O (%)</b>",              Chemistry.GasType.Steam, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityH2O,         ("H2OM ", "H2O",            "mol",  "Mode: <b>H₂O (mol)</b>",            Chemistry.GasType.Steam, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioSteam,          ("GH2OR", "STEAM",          "%",    "Mode: <b>Steam (%)</b>",            Chemistry.GasType.Steam, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantitySteam,       ("GH2OM", "STEAM",          "mol",  "Mode: <b>Steam (mol)</b>",          Chemistry.GasType.Steam, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioWater,          ("LH2OR", "WATER",          "%",    "Mode: <b>Water (%)</b>",            Chemistry.GasType.Water, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeWater,         ("LH2OV", "WATER",          "L",    "Mode: <b>Water (L)</b>",            Chemistry.GasType.Water, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityWater,       ("LH2OM", "WATER",          "mol",  "Mode: <b>Water (mol)</b>",          Chemistry.GasType.Water, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioPollutedH2O,    ("LPH2OR", "POLLUTED WATER", "%",    "Mode: <b>Polluted Water (%)</b>",   Chemistry.GasType.PollutedWater, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumePollutedH2O,   ("LPH2OV", "POLLUTED WATER", "L",    "Mode: <b>Polluted Water (L)</b>",   Chemistry.GasType.PollutedWater, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityPollutedH2O, ("LPH2OM", "POLLUTED WATER", "mol",  "Mode: <b>Polluted Water (mol)</b>", Chemistry.GasType.PollutedWater, PatchDataType.Quantity, false)},
            // Helium
            {(int)PatchGasDisplayMode.RatioHe,             ("HeR", "HE",           "%",    "Mode: <b>He (%)</b>",              Chemistry.GasType.Helium, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityHe,          ("HeM", "HE",           "mol",  "Mode: <b>He (mol)</b>",            Chemistry.GasType.Helium, PatchDataType.Quantity, true)},
            // Hydrazine
            {(int)PatchGasDisplayMode.RatioHydrazine,      ("N2H4R", "N2H4",     "%",    "Mode: <b>N₂H₄ (%)</b>",           Chemistry.GasType.Hydrazine, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityHydrazine,   ("N2H4M", "N2H4",     "mol",  "Mode: <b>N₂H₄ (mol)</b>",         Chemistry.GasType.Hydrazine, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousHydrazine, ("GN2H4R", "GASEOUS N2H4",  "%",    "Mode: <b>Gaseous N₂H₄ (%)</b>",   Chemistry.GasType.Hydrazine, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousHydrazine, ("GN2H4M", "GASEOUS N2H4",  "mol",  "Mode: <b>Gaseous N₂H₄ (mol)</b>", Chemistry.GasType.Hydrazine, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidHydrazine, ("LN2H4R", "LIQUID N2H4",   "%",    "Mode: <b>Liquid N₂H₄ (%)</b>",    Chemistry.GasType.LiquidHydrazine, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidHydrazine, ("LN2H4V", "LIQUID N2H4",   "L",    "Mode: <b>Liquid N₂H₄ (L)</b>",    Chemistry.GasType.LiquidHydrazine, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidHydrazine, ("LN2H4M", "LIQUID N2H4",   "mol",  "Mode: <b>Liquid N₂H₄ (mol)</b>",  Chemistry.GasType.LiquidHydrazine, PatchDataType.Quantity, false)},
            // Hydrogen
            {(int)PatchGasDisplayMode.RatioH2,             ("H2R", "H2",           "%",    "Mode: <b>H₂ (%)</b>",               Chemistry.GasType.Hydrogen, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityH2,          ("H2M", "H2",           "mol",  "Mode: <b>H₂ (mol)</b>",             Chemistry.GasType.Hydrogen, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousH2,     ("GH2R", "GASEOUS H2",  "%",    "Mode: <b>Gaseous H₂ (%)</b>",       Chemistry.GasType.Hydrogen, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousH2,  ("GH2M", "GASEOUS H2",  "mol",  "Mode: <b>Gaseous H₂ (mol)</b>",     Chemistry.GasType.Hydrogen, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidH2,      ("LH2R", "LIQUID H2",   "%",    "Mode: <b>Liquid H₂ (%)</b>",        Chemistry.GasType.LiquidHydrogen, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidH2,     ("LH2V", "LIQUID H2",   "L",    "Mode: <b>Liquid H₂ (L)</b>",        Chemistry.GasType.LiquidHydrogen, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidH2,    ("LH2M", "LIQUID H2",   "mol",  "Mode: <b>Liquid H₂ (mol)</b>",      Chemistry.GasType.LiquidHydrogen, PatchDataType.Quantity, false)},
            // Alcohol
            {(int)PatchGasDisplayMode.RatioLiquidAlcohol, ("LAlcR", "ALCOHOL",          "%",    "Mode: <b>Alcohol (%)</b>",           Chemistry.GasType.LiquidAlcohol, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidAlcohol, ("LAlcV", "ALCOHOL",          "L",    "Mode: <b>Alcohol (L)</b>",           Chemistry.GasType.LiquidAlcohol, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidAlcohol, ("LAlcM", "ALCOHOL",          "mol",  "Mode: <b>Alcohol (mol)</b>",         Chemistry.GasType.LiquidAlcohol, PatchDataType.Quantity, false)},
            // Ozone
            {(int)PatchGasDisplayMode.RatioO3,            ("O3R", "O3",           "%",    "Mode: <b>O₃ (%)</b>",               Chemistry.GasType.Ozone, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityO3,         ("O3M", "O3",           "mol",  "Mode: <b>O₃ (mol)</b>",             Chemistry.GasType.Ozone, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousO3,     ("GO3R", "GASEOUS O3",  "%",    "Mode: <b>Gaseous O₃ (%)</b>",       Chemistry.GasType.Ozone, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousO3,  ("GO3M", "GASEOUS O3",  "mol",  "Mode: <b>Gaseous O₃ (mol)</b>",     Chemistry.GasType.Ozone, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidO3,      ("LO3R", "LIQUID O3",   "%",    "Mode: <b>Liquid O₃ (%)</b>",        Chemistry.GasType.LiquidOzone, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidO3,     ("LO3V", "LIQUID O3",   "L",    "Mode: <b>Liquid O₃ (L)</b>",        Chemistry.GasType.LiquidOzone, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidO3,    ("LO3M", "LIQUID O3",   "mol",  "Mode: <b>Liquid O₃ (mol)</b>",      Chemistry.GasType.LiquidOzone, PatchDataType.Quantity, false)},
            // Silanol
            {(int)PatchGasDisplayMode.RatioSiH4,          ("SIH4R", "SIL",           "%",    "Mode: <b>Sil (%)</b>",              Chemistry.GasType.Silanol, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantitySiH4,       ("SIH4M", "SIL",           "mol",  "Mode: <b>Sil (mol)</b>",            Chemistry.GasType.Silanol, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousSiH4,  ("GSIH4R", "GASEOUS SIL",  "%",    "Mode: <b>Gaseous Sil (%)</b>",      Chemistry.GasType.Silanol, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousSiH4, ("GSIH4M", "GASEOUS SIL",  "mol",  "Mode: <b>Gaseous Sil (mol)</b>",    Chemistry.GasType.Silanol, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidSiH4,   ("LSIH4R", "LIQUID SIL",   "%",    "Mode: <b>Liquid Sil (%)</b>",       Chemistry.GasType.LiquidSilanol, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidSiH4,  ("LSIH4V", "LIQUID SIL",   "L",    "Mode: <b>Liquid Sil (L)</b>",       Chemistry.GasType.LiquidSilanol, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidSiH4, ("LSIH4M", "LIQUID SIL",   "mol",  "Mode: <b>Liquid Sil (mol)</b>",     Chemistry.GasType.LiquidSilanol, PatchDataType.Quantity, false)},
            // Sodium Chloride
            {(int)PatchGasDisplayMode.RatioNaCl,          ("NAClR", "NACL",           "%",    "Mode: <b>NaCl (%)</b>",               Chemistry.GasType.LiquidSodiumChloride, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeNaCl,         ("NAClV", "NACL",           "L",    "Mode: <b>NaCl (L)</b>",               Chemistry.GasType.LiquidSodiumChloride, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityNaCl,       ("NAClM", "NACL",           "mol",  "Mode: <b>NaCl (mol)</b>",             Chemistry.GasType.LiquidSodiumChloride, PatchDataType.Quantity, false)},
            // Hydrochloric Acid
            {(int)PatchGasDisplayMode.RatioHCl,          ("HCLR", "HCL",           "%",    "Mode: <b>HCl (%)</b>",               Chemistry.GasType.HydrochloricAcid, PatchDataType.Ratio, true)},
            {(int)PatchGasDisplayMode.QuantityHCl,       ("HCLM", "HCL",           "mol",  "Mode: <b>HCl (mol)</b>",             Chemistry.GasType.HydrochloricAcid, PatchDataType.Quantity, true)},
            {(int)PatchGasDisplayMode.RatioGaseousHCl,  ("GHCLR", "GASEOUS HCL",  "%",    "Mode: <b>Gaseous HCl (%)</b>",       Chemistry.GasType.HydrochloricAcid, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.QuantityGaseousHCl, ("GHCLM", "GASEOUS HCL",  "mol",  "Mode: <b>Gaseous HCl (mol)</b>",     Chemistry.GasType.HydrochloricAcid, PatchDataType.Quantity, false)},
            {(int)PatchGasDisplayMode.RatioLiquidHCl,   ("LHCLR", "LIQUID HCL",   "%",    "Mode: <b>Liquid HCl (%)</b>",        Chemistry.GasType.LiquidHydrochloricAcid, PatchDataType.Ratio, false)},
            {(int)PatchGasDisplayMode.VolumeLiquidHCl,  ("LHCLV", "LIQUID HCL",   "L",    "Mode: <b>Liquid HCl (L)</b>",        Chemistry.GasType.LiquidHydrochloricAcid, PatchDataType.Volume, false)},
            {(int)PatchGasDisplayMode.QuantityLiquidHCl, ("LHCLM", "LIQUID HCL",   "mol",  "Mode: <b>Liquid HCl (mol)</b>",      Chemistry.GasType.LiquidHydrochloricAcid, PatchDataType.Quantity, false)},
            };

        public static string getGasDisplayModeTitle(int index)
        {
            return GasData[index].Item2;
        }
        public static string getDisplayModeUnits(int index)
        {
            return GasData[index].Item3;
        }
        public static string getGasDisplayModeButtonName(int index)
        {
            return GasData[index].Item4;
        }
        public static Chemistry.GasType? getGasDisplayModeGas(int index)
        {
            return GasData[index].Item5;
        }
        public static PatchDataType getGasDisplayModePatchDataType(int index)
        {
            return GasData[index].Item6;
        }
        public static bool getGasDisplayModeCombinedFlag(int index)
        {
            return GasData[index].Item7;
        }

        public static float GetGasSensorQuantity(Chemistry.GasType? gasType, Atmosphere atmosphere, bool combined)
        {
            float result = 0f;
            switch (gasType)
            {
                case null:
                    result = atmosphere.TotalMolesGases.ToFloat();
                    break;
                case Chemistry.GasType.Undefined: // hack to get liquid mols
                    result = atmosphere.TotalMolesLiquids.ToFloat();
                    break;
                case Chemistry.GasType.Oxygen:
                    result = atmosphere.GasMixture.Oxygen.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Nitrogen:
                    result = atmosphere.GasMixture.Nitrogen.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Methane:
                    result = atmosphere.GasMixture.Methane.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Pollutant:
                    result = atmosphere.GasMixture.Pollutant.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.CarbonDioxide:
                    result = atmosphere.GasMixture.CarbonDioxide.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Steam:
                    result = atmosphere.GasMixture.Steam.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.NitrousOxide:
                    result = atmosphere.GasMixture.NitrousOxide.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidOxygen:
                    result = atmosphere.GasMixture.LiquidOxygen.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidNitrogen:
                    result = atmosphere.GasMixture.LiquidNitrogen.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidMethane:
                    result = atmosphere.GasMixture.LiquidMethane.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidPollutant:
                    result = atmosphere.GasMixture.LiquidPollutant.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidCarbonDioxide:
                    result = atmosphere.GasMixture.LiquidCarbonDioxide.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Water:
                    result = atmosphere.GasMixture.Water.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidNitrousOxide:
                    result = atmosphere.GasMixture.LiquidNitrousOxide.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.PollutedWater:
                    result = atmosphere.GasMixture.PollutedWater.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Helium:
                    result = atmosphere.GasMixture.Helium.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Hydrazine:
                    result = atmosphere.GasMixture.Hydrazine.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Hydrogen:
                    result = atmosphere.GasMixture.Hydrogen.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Ozone:
                    result = atmosphere.GasMixture.Ozone.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Silanol:
                    result = atmosphere.GasMixture.Silanol.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidAlcohol:
                    result = atmosphere.GasMixture.LiquidAlcohol.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidSodiumChloride:
                    result = atmosphere.GasMixture.LiquidSodiumChloride.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidHydrogen:
                    result = atmosphere.GasMixture.LiquidHydrogen.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidHydrazine:
                    result = atmosphere.GasMixture.LiquidHydrazine.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidOzone:
                    result = atmosphere.GasMixture.LiquidOzone.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidSilanol:
                    result = atmosphere.GasMixture.LiquidSilanol.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.HydrochloricAcid:
                    result = atmosphere.GasMixture.HydrochloricAcid.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.LiquidHydrochloricAcid:
                    result = atmosphere.GasMixture.LiquidHydrochloricAcid.Quantity.ToFloat();
                    break;
            }
            if (combined == false)
                return result;

            switch (gasType)
            {
                case null:
                    result += atmosphere.TotalMolesLiquids.ToFloat();
                    break;
                case Chemistry.GasType.Oxygen:
                    result += atmosphere.GasMixture.LiquidOxygen.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Nitrogen:
                    result += atmosphere.GasMixture.LiquidNitrogen.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Methane:
                    result += atmosphere.GasMixture.LiquidMethane.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Pollutant:
                    result += atmosphere.GasMixture.LiquidPollutant.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.CarbonDioxide:
                    result += atmosphere.GasMixture.LiquidCarbonDioxide.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Steam:
                    result += atmosphere.GasMixture.Water.Quantity.ToFloat();
                    result += atmosphere.GasMixture.PollutedWater.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.NitrousOxide:
                    result += atmosphere.GasMixture.LiquidNitrousOxide.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Hydrazine:
                    result += atmosphere.GasMixture.LiquidHydrazine.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Hydrogen:
                    result += atmosphere.GasMixture.LiquidHydrogen.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Ozone:
                    result += atmosphere.GasMixture.LiquidOzone.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.Silanol:
                    result += atmosphere.GasMixture.LiquidSilanol.Quantity.ToFloat();
                    break;
                case Chemistry.GasType.HydrochloricAcid:
                    result += atmosphere.GasMixture.LiquidHydrochloricAcid.Quantity.ToFloat();
                    break;
            }
            return result;
        }

        public static float GetGasSensorLiquidVolume(Chemistry.GasType? gasType, Atmosphere atmosphere)
        {
            float result = 0f;
            switch (gasType)
            {
                case null:
                    result = atmosphere.TotalVolumeLiquids.ToFloat();
                    break;
                case Chemistry.GasType.LiquidOxygen:
                    result = atmosphere.GasMixture.LiquidOxygen.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidNitrogen:
                    result = atmosphere.GasMixture.LiquidNitrogen.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidMethane:
                    result = atmosphere.GasMixture.LiquidMethane.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidPollutant:
                    result = atmosphere.GasMixture.LiquidPollutant.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidCarbonDioxide:
                    result = atmosphere.GasMixture.LiquidCarbonDioxide.Volume.ToFloat();
                    break;
                case Chemistry.GasType.Water:
                    result = atmosphere.GasMixture.Water.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidNitrousOxide:
                    result = atmosphere.GasMixture.LiquidNitrousOxide.Volume.ToFloat();
                    break;
                case Chemistry.GasType.PollutedWater:
                    result = atmosphere.GasMixture.PollutedWater.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidHydrogen:
                    result = atmosphere.GasMixture.LiquidHydrogen.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidHydrazine:
                    result = atmosphere.GasMixture.LiquidHydrazine.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidAlcohol:
                    result = atmosphere.GasMixture.LiquidAlcohol.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidOzone:
                    result = atmosphere.GasMixture.LiquidOzone.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidSilanol:
                    result = atmosphere.GasMixture.LiquidSilanol.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidSodiumChloride:
                    result = atmosphere.GasMixture.LiquidSodiumChloride.Volume.ToFloat();
                    break;
                case Chemistry.GasType.LiquidHydrochloricAcid:
                    result = atmosphere.GasMixture.LiquidHydrochloricAcid.Volume.ToFloat();
                    break;
            }
            return result;
        }

        public static float GetEnergy(PatchGasDisplayMode energy_type, Atmosphere atmosphere, Thing thermalThing)
        {
            float energy = 0f;
            if (energy_type == PatchGasDisplayMode.EnergyConvected)
            {
                if (atmosphere.AtmosphericsNetwork != null)
                {
                    // for pipe analyzer
                    energy = atmosphere.AtmosphericsNetwork.EnergyConvected;
                }
                else
                {
                    energy = thermalThing.EnergyConvected;
                }
            }
            if (energy_type == PatchGasDisplayMode.EnergyRadiated)
            {
                if (atmosphere.AtmosphericsNetwork != null)
                {
                    // for pipe analyzer
                    energy = atmosphere.AtmosphericsNetwork.EnergyRadiated;
                }
                else
                {
                    energy = thermalThing.EnergyRadiated;
                }
            }
            if (energy_type == PatchGasDisplayMode.EnergyLatent)
            {
                energy = atmosphere.LastTickLatentEnergy.ToFloat();
            }
            return energy;
        }

        public static string FormatSIUnits(float value, string unit)
        {
            string formatedNumber = "";
            string formatedUnits = unit;
            string unitPrefix = "";

            if (value == 0)
            {
                // don't do anything special
            }
            else if ((1 > value) && (value > -1))
            {
                value *= 1000;
                unitPrefix = "m";
            }
            else
            {
                if (value >= 10000 || value <= -1000)
                {
                    value /= 1000;
                    unitPrefix = "k";
                }
                if (value >= 10000 || value <= -1000)
                {
                    value /= 1000;
                    unitPrefix = "M";
                }
                if (value >= 10000 || value <= -1000)
                {
                    value /= 1000;
                    unitPrefix = "G";
                }
            }
            formatedUnits = unitPrefix + formatedUnits;


            if (value < -100)
            {
                formatedNumber = string.Format("{0:0}", (object)value);
            }
            else if (value < -10)
            {
                formatedNumber = string.Format("{0:0.0}", (object)value);
            }
            else if (value < -1)
            {
                formatedNumber = string.Format("{0:0.00}", (object)value);
            }
            else if (value == 0)
            {
                formatedNumber = string.Format("{0:0}", (object)value);
            }
            else if (value < 10)
            {
                formatedNumber = string.Format("{0:0.000}", (object)value);
            }
            else if (value < 100)
            {
                formatedNumber = string.Format("{0:0.00}", (object)value);
            }
            else if (value < 1000)
            {
                formatedNumber = string.Format("{0:0.0}", (object)value);
            }
            else
            {
                formatedNumber = string.Format("{0:0}", (object)value);
            }

            return formatedNumber + "|" + formatedUnits;
        }

    }
}
