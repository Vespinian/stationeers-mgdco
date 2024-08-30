using System.Collections.Generic;
using Assets.Scripts.Objects;
using Assets.Scripts.Atmospherics;

namespace MoreGasDisplayConsoleOptions
{
	static class MGDCOPatchHelper
	{
		public enum PatchGasDisplayMode
		{
			PrecisePressure = 0,
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
			// Volatiles
			RatioVol,
			QuantityVol,
			RatioGaseousVol,
			QuantityGaseousVol,
			RatioLiquidVol,
			QuantityLiquidVol,
			VolumeLiquidVol,
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
			// Vanilla behavior
			Pressure,

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

		// DisplayTitle , DisplayUnits, ToggleModeButtonText, Gas type, Patch Type, Combine liquid and gas for ratio and quantity data types
		public static readonly Dictionary<int, (string tag, string displayName, string unit, string displayModeButton, Chemistry.GasType? gasType, PatchDataType dataType, bool combined)> GasData
			= new Dictionary<int, (string, string, string, string, Chemistry.GasType?, PatchDataType, bool)> {
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
			// Vol
			{(int)PatchGasDisplayMode.RatioVol,            ("VOLR", "VOL",           "%",    "Mode: <b>VOL (%)</b>",              Chemistry.GasType.Volatiles, PatchDataType.Ratio, true)},
			{(int)PatchGasDisplayMode.QuantityVol,         ("VOLM", "VOL",           "mol",  "Mode: <b>VOL (mol)</b>",            Chemistry.GasType.Volatiles, PatchDataType.Quantity, true)},
			{(int)PatchGasDisplayMode.RatioGaseousVol,     ("GVOLR", "GASEOUS VOL",  "%",    "Mode: <b>Gaseous VOL (%)</b>",      Chemistry.GasType.Volatiles, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.QuantityGaseousVol,  ("GVOLM", "GASEOUS VOL",  "mol",  "Mode: <b>Gaseous VOL (mol)</b>",    Chemistry.GasType.Volatiles, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.RatioLiquidVol,      ("LVOLR", "LIQUID VOL",   "%",    "Mode: <b>Liquid VOL (%)</b>",       Chemistry.GasType.LiquidVolatiles, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.VolumeLiquidVol,     ("LVOLV", "LIQUID VOL",   "L",    "Mode: <b>Liquid VOL (L)</b>",       Chemistry.GasType.LiquidVolatiles, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.QuantityLiquidVol,   ("LVOLM", "LIQUID VOL",   "mol",  "Mode: <b>Liquid VOL (mol)</b>",     Chemistry.GasType.LiquidVolatiles, PatchDataType.Quantity, false)},
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
		};

		public static string getGasDisplayModeTitle(int index) {
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
					result = atmosphere.TotalMolesGases;
					break;
				case Chemistry.GasType.Undefined: // hack to get liquid mols
					result = atmosphere.TotalMolesLiquids;
					break;
				case Chemistry.GasType.Oxygen:
					result = atmosphere.GasMixture.Oxygen.Quantity;
					break;
				case Chemistry.GasType.Nitrogen:
					result = atmosphere.GasMixture.Nitrogen.Quantity;
					break;
				case Chemistry.GasType.Volatiles:
					result = atmosphere.GasMixture.Volatiles.Quantity;
					break;
				case Chemistry.GasType.Pollutant:
					result = atmosphere.GasMixture.Pollutant.Quantity;
					break;
				case Chemistry.GasType.CarbonDioxide:
					result = atmosphere.GasMixture.CarbonDioxide.Quantity;
					break;
				case Chemistry.GasType.Steam:
					result = atmosphere.GasMixture.Steam.Quantity;
					break;
				case Chemistry.GasType.NitrousOxide:
					result = atmosphere.GasMixture.NitrousOxide.Quantity;
					break;
				case Chemistry.GasType.LiquidOxygen:
					result = atmosphere.GasMixture.LiquidOxygen.Quantity;
					break;
				case Chemistry.GasType.LiquidNitrogen:
					result = atmosphere.GasMixture.LiquidNitrogen.Quantity;
					break;
				case Chemistry.GasType.LiquidVolatiles:
					result = atmosphere.GasMixture.LiquidVolatiles.Quantity;
					break;
				case Chemistry.GasType.LiquidPollutant:
					result = atmosphere.GasMixture.LiquidPollutant.Quantity;
					break;
				case Chemistry.GasType.LiquidCarbonDioxide:
					result = atmosphere.GasMixture.LiquidCarbonDioxide.Quantity;
					break;
				case Chemistry.GasType.Water:
					result = atmosphere.GasMixture.Water.Quantity;
					break;
				case Chemistry.GasType.LiquidNitrousOxide:
					result = atmosphere.GasMixture.LiquidNitrousOxide.Quantity;
					break;
				case Chemistry.GasType.PollutedWater:
					result = atmosphere.GasMixture.PollutedWater.Quantity;
					break;
			}
			if(combined == false)
				return result;

			switch (gasType)
			{
				case null:
					result += atmosphere.TotalMolesLiquids;
					break;
				case Chemistry.GasType.Oxygen:
					result += atmosphere.GasMixture.LiquidOxygen.Quantity;
					break;
				case Chemistry.GasType.Nitrogen:
					result += atmosphere.GasMixture.LiquidNitrogen.Quantity;
					break;
				case Chemistry.GasType.Volatiles:
					result += atmosphere.GasMixture.LiquidVolatiles.Quantity;
					break;
				case Chemistry.GasType.Pollutant:
					result += atmosphere.GasMixture.LiquidPollutant.Quantity;
					break;
				case Chemistry.GasType.CarbonDioxide:
					result += atmosphere.GasMixture.LiquidCarbonDioxide.Quantity;
					break;
				case Chemistry.GasType.Steam:
					result += atmosphere.GasMixture.Water.Quantity;
					result += atmosphere.GasMixture.PollutedWater.Quantity;
					break;
				case Chemistry.GasType.NitrousOxide:
					result += atmosphere.GasMixture.LiquidNitrousOxide.Quantity;
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
					result = atmosphere.TotalVolumeLiquids;
					break;
				case Chemistry.GasType.LiquidOxygen:
					result = atmosphere.GasMixture.LiquidOxygen.Volume;
					break;
				case Chemistry.GasType.LiquidNitrogen:
					result = atmosphere.GasMixture.LiquidNitrogen.Volume;
					break;
				case Chemistry.GasType.LiquidVolatiles:
					result = atmosphere.GasMixture.LiquidVolatiles.Volume;
					break;
				case Chemistry.GasType.LiquidPollutant:
					result = atmosphere.GasMixture.LiquidPollutant.Volume;
					break;
				case Chemistry.GasType.LiquidCarbonDioxide:
					result = atmosphere.GasMixture.LiquidCarbonDioxide.Volume;
					break;
				case Chemistry.GasType.Water:
					result = atmosphere.GasMixture.Water.Volume;
					break;
				case Chemistry.GasType.LiquidNitrousOxide:
					result = atmosphere.GasMixture.LiquidNitrousOxide.Volume;
					break;
				case Chemistry.GasType.PollutedWater:
					result = atmosphere.GasMixture.PollutedWater.Volume;
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
				energy = atmosphere.LastTickLatentEnergy;
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
