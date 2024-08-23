using System.Collections.Generic;
using Assets.Scripts.Objects;
using Assets.Scripts.Atmospherics;

namespace MoreGasDisplayConsoleOptions
{
	static class MGDCOPatchHelper
	{
		public enum PatchGasDisplayMode
		{
			Pressure = 0,
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
		public static readonly Dictionary<int, (string, string, string, Chemistry.GasType?, PatchDataType, bool)> GasData
			= new Dictionary<int, (string, string, string, Chemistry.GasType?, PatchDataType, bool)> {
			{(int)PatchGasDisplayMode.Pressure,            ("PRESSURE",            "Pa",  "Mode: <b>Pressure</b>",             null, PatchDataType.Pressure, false)},
			{(int)PatchGasDisplayMode.Temperature,         ("TEMPERATURE",         "°C",  "Mode: <b>Temperature (°C)</b>",     null, PatchDataType.Temperature, false)},
			{(int)PatchGasDisplayMode.TemperatureKelvin,   ("TEMPERATURE",         "K",   "Mode: <b>Temperature (K)</b>",      null, PatchDataType.Temperature, false)},
			{(int)PatchGasDisplayMode.TotalMoles,          ("TOTAL MOL",           "mol", "Mode: <b>Total (mol)</b>",          null, PatchDataType.Quantity, true)},
			{(int)PatchGasDisplayMode.TotalGaseousMoles,   ("TOTAL GASEOUS",       "mol", "Mode: <b>Total Gas (mol)</b>",      null, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.TotalLiquidMoles,    ("TOTAL LIQUID",        "mol", "Mode: <b>Total Liquid (mol)</b>",   Chemistry.GasType.Undefined, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.TotalLiquidVolume,   ("TOTAL LIQUID",        "L",   "Mode: <b>Total Liquid (L)</b>",     null, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.EnergyConvected,     ("CONVECTED",           "J",   "Mode: <b>Convected (J)</b>",        null, PatchDataType.Energy, false)},
			{(int)PatchGasDisplayMode.EnergyRadiated,      ("RADIATED",            "J",   "Mode: <b>Radiated (J)</b>",         null, PatchDataType.Energy, false)},
			{(int)PatchGasDisplayMode.EnergyLatent,        ("LATENT",              "J",   "Mode: <b>Latent (J)</b>",           null, PatchDataType.Energy, false)},
			// O2
			{(int)PatchGasDisplayMode.RatioO2,             ("O2",          "%",    "Mode: <b>O₂ (%)</b>",            Chemistry.GasType.Oxygen, PatchDataType.Ratio, true)},
			{(int)PatchGasDisplayMode.QuantityO2,          ("O2",          "mol",  "Mode: <b>O₂ (mol)</b>",          Chemistry.GasType.Oxygen, PatchDataType.Quantity, true)},
			{(int)PatchGasDisplayMode.RatioGaseousO2,      ("GASEOUS O2",  "%",    "Mode: <b>Gaseous O₂ (%)</b>",    Chemistry.GasType.Oxygen, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.QuantityGaseousO2,   ("GASEOUS O2",  "mol",  "Mode: <b>Gaseous O₂ (mol)</b>",  Chemistry.GasType.Oxygen, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.RatioLiquidO2,       ("LIQUID O2",   "%",    "Mode: <b>Liquid O₂ (%)</b>",     Chemistry.GasType.LiquidOxygen, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.VolumeLiquidO2,      ("LIQUID O2",   "L",    "Mode: <b>Liquid O₂ (L)</b>",     Chemistry.GasType.LiquidOxygen, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.QuantityLiquidO2,    ("LIQUID O2",   "mol",  "Mode: <b>Liquid O₂ (mol)</b>",   Chemistry.GasType.LiquidOxygen, PatchDataType.Quantity, false)},
			// Nitrogen
			{(int)PatchGasDisplayMode.RatioN,              ("N",          "%",    "Mode: <b>N (%)</b>",           Chemistry.GasType.Nitrogen, PatchDataType.Ratio, true)},
			{(int)PatchGasDisplayMode.QuantityN,           ("N",          "mol",  "Mode: <b>N (mol)</b>",         Chemistry.GasType.Nitrogen, PatchDataType.Quantity, true)},
			{(int)PatchGasDisplayMode.RatioGaseousN,       ("GASEOUS N",  "%",    "Mode: <b>Gaseous N (%)</b>",   Chemistry.GasType.Nitrogen, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.QuantityGaseousN,    ("GASEOUS N",  "mol",  "Mode: <b>Gaseous N (mol)</b>", Chemistry.GasType.Nitrogen, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.RatioLiquidN,        ("LIQUID N",   "%",    "Mode: <b>Liquid N (%)</b>",    Chemistry.GasType.LiquidNitrogen, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.VolumeLiquidN,       ("LIQUID N",   "L",    "Mode: <b>Liquid N (L)</b>",    Chemistry.GasType.LiquidNitrogen, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.QuantityLiquidN,     ("LIQUID N",   "mol",  "Mode: <b>Liquid N (mol)</b>",  Chemistry.GasType.LiquidNitrogen, PatchDataType.Quantity, false)},
			// CO2
			{(int)PatchGasDisplayMode.RatioCO2,            ("CO2",          "%",    "Mode: <b>CO₂ (%)</b>",              Chemistry.GasType.CarbonDioxide, PatchDataType.Ratio, true)},
			{(int)PatchGasDisplayMode.QuantityCO2,         ("CO2",          "mol",  "Mode: <b>CO₂ (mol)</b>",            Chemistry.GasType.CarbonDioxide, PatchDataType.Quantity, true)},
			{(int)PatchGasDisplayMode.RatioGaseousCO2,     ("GASEOUS CO2",  "%",    "Mode: <b>Gaseous CO₂ (%)</b>",      Chemistry.GasType.CarbonDioxide, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.QuantityGaseousCO2,  ("GASEOUS CO2",  "mol",  "Mode: <b>Gaseous CO₂ (mol)</b>",    Chemistry.GasType.CarbonDioxide, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.RatioLiquidCO2,      ("LIQUID CO2",   "%",    "Mode: <b>Liquid CO₂ (%)</b>",       Chemistry.GasType.LiquidCarbonDioxide, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.VolumeLiquidCO2,     ("LIQUID CO2",   "L",    "Mode: <b>Liquid CO₂ (L)</b>",       Chemistry.GasType.LiquidCarbonDioxide, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.QuantityLiquidCO2,   ("LIQUID CO2",   "mol",  "Mode: <b>Liquid CO₂ (mol)</b>",     Chemistry.GasType.LiquidCarbonDioxide, PatchDataType.Quantity, false)},
			// Pol
			{(int)PatchGasDisplayMode.RatioPol,            ("POL",          "%",    "Mode: <b>POL (%)</b>",              Chemistry.GasType.Pollutant, PatchDataType.Ratio, true)},
			{(int)PatchGasDisplayMode.QuantityPol,         ("POL",          "mol",  "Mode: <b>POL (mol)</b>",            Chemistry.GasType.Pollutant, PatchDataType.Quantity, true)},
			{(int)PatchGasDisplayMode.RatioGaseousPol,     ("GASEOUS POL",  "%",    "Mode: <b>Gaseous POL (%)</b>",      Chemistry.GasType.Pollutant, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.QuantityGaseousPol,  ("GASEOUS POL",  "mol",  "Mode: <b>Gaseous POL (mol)</b>",    Chemistry.GasType.Pollutant, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.RatioLiquidPol,      ("LIQUID POL",   "%",    "Mode: <b>Liquid POL (%)</b>",       Chemistry.GasType.LiquidPollutant, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.VolumeLiquidPol,     ("LIQUID POL",   "L",    "Mode: <b>Liquid POL (L)</b>",       Chemistry.GasType.LiquidPollutant, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.QuantityLiquidPol,   ("LIQUID POL",   "mol",  "Mode: <b>Liquid POL (mol)</b>",     Chemistry.GasType.LiquidPollutant, PatchDataType.Quantity, false)},
			// Vol
			{(int)PatchGasDisplayMode.RatioVol,            ("VOL",          "%",    "Mode: <b>VOL (%)</b>",              Chemistry.GasType.Volatiles, PatchDataType.Ratio, true)},
			{(int)PatchGasDisplayMode.QuantityVol,         ("VOL",          "mol",  "Mode: <b>VOL (mol)</b>",            Chemistry.GasType.Volatiles, PatchDataType.Quantity, true)},
			{(int)PatchGasDisplayMode.RatioGaseousVol,     ("GASEOUS VOL",  "%",    "Mode: <b>Gaseous VOL (%)</b>",      Chemistry.GasType.Volatiles, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.QuantityGaseousVol,  ("GASEOUS VOL",  "mol",  "Mode: <b>Gaseous VOL (mol)</b>",    Chemistry.GasType.Volatiles, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.RatioLiquidVol,      ("LIQUID VOL",   "%",    "Mode: <b>Liquid VOL (%)</b>",       Chemistry.GasType.LiquidVolatiles, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.VolumeLiquidVol,     ("LIQUID VOL",   "L",    "Mode: <b>Liquid VOL (L)</b>",       Chemistry.GasType.LiquidVolatiles, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.QuantityLiquidVol,   ("LIQUID VOL",   "mol",  "Mode: <b>Liquid VOL (mol)</b>",     Chemistry.GasType.LiquidVolatiles, PatchDataType.Quantity, false)},
			// N2O
			{(int)PatchGasDisplayMode.RatioN2O,            ("N2O",          "%",    "Mode: <b>N₂O (%)</b>",              Chemistry.GasType.NitrousOxide, PatchDataType.Ratio, true)},
			{(int)PatchGasDisplayMode.QuantityN2O,         ("N2O",          "mol",  "Mode: <b>N₂O (mol)</b>",            Chemistry.GasType.NitrousOxide, PatchDataType.Quantity, true)},
			{(int)PatchGasDisplayMode.RatioGaseousN2O,     ("GASEOUS N2O",  "%",    "Mode: <b>Gaseous N₂O (%)</b>",      Chemistry.GasType.NitrousOxide, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.QuantityGaseousN2O,  ("GASEOUS N2O",  "mol",  "Mode: <b>Gaseous N₂O (mol)</b>",    Chemistry.GasType.NitrousOxide, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.RatioLiquidN2O,      ("LIQUID N2O",   "%",    "Mode: <b>Liquid N₂O (%)</b>",       Chemistry.GasType.LiquidNitrousOxide, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.VolumeLiquidN2O,     ("LIQUID N2O",   "L",    "Mode: <b>Liquid N₂O (L)</b>",       Chemistry.GasType.LiquidNitrousOxide, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.QuantityLiquidN2O,   ("LIQUID N2O",   "mol",  "Mode: <b>Liquid N₂O (mol)</b>",     Chemistry.GasType.LiquidNitrousOxide, PatchDataType.Quantity, false)},
			// H2O
			{(int)PatchGasDisplayMode.RatioH2O,            ("H2O",            "%",    "Mode: <b>H₂O (%)</b>",              Chemistry.GasType.Steam, PatchDataType.Ratio, true)},
			{(int)PatchGasDisplayMode.QuantityH2O,         ("H2O",            "mol",  "Mode: <b>H₂O (mol)</b>",            Chemistry.GasType.Steam, PatchDataType.Quantity, true)},
			{(int)PatchGasDisplayMode.RatioSteam,          ("STEAM",          "%",    "Mode: <b>Steam (%)</b>",            Chemistry.GasType.Steam, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.QuantitySteam,       ("STEAM",          "mol",  "Mode: <b>Steam (mol)</b>",          Chemistry.GasType.Steam, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.RatioWater,          ("WATER",          "%",    "Mode: <b>Water (%)</b>",            Chemistry.GasType.Water, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.VolumeWater,         ("WATER",          "L",    "Mode: <b>Water (L)</b>",            Chemistry.GasType.Water, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.QuantityWater,       ("WATER",          "mol",  "Mode: <b>Water (mol)</b>",          Chemistry.GasType.Water, PatchDataType.Quantity, false)},
			{(int)PatchGasDisplayMode.RatioPollutedH2O,    ("POLLUTED WATER", "%",    "Mode: <b>Polluted Water (%)</b>",   Chemistry.GasType.PollutedWater, PatchDataType.Ratio, false)},
			{(int)PatchGasDisplayMode.VolumePollutedH2O,   ("POLLUTED WATER", "L",    "Mode: <b>Polluted Water (L)</b>",   Chemistry.GasType.PollutedWater, PatchDataType.Volume, false)},
			{(int)PatchGasDisplayMode.QuantityPollutedH2O, ("POLLUTED WATER", "mol",  "Mode: <b>Polluted Water (mol)</b>", Chemistry.GasType.PollutedWater, PatchDataType.Quantity, false)},
		};

		public static string getGasDisplayModeTitle(int index) {
			return GasData[index].Item1;
		}
		public static string getDisplayModeUnits(int index)
		{
			return GasData[index].Item2;
		}
		public static string getGasDisplayModeButtonName(int index)
		{
			return GasData[index].Item3;
		}
		public static Chemistry.GasType? getGasDisplayModeGas(int index)
		{
			return GasData[index].Item4;
		}
		public static PatchDataType getGasDisplayModePatchDataType(int index)
		{
			return GasData[index].Item5;
		}
		public static bool getGasDisplayModeCombinedFlag(int index)
		{
			return GasData[index].Item6;
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

		public static (string, string) FormatSIUnits(float value, string unit)
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

			return (formatedNumber, formatedUnits);
		}

	}
}
