﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Pipes;
using Assets.Scripts;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Atmospherics;
using System.Data;
using System.Linq;
using InputSystem;


namespace MoreGasDisplayConsoleOptions
{
	public static class GasDisplayExtenderDict { 
		public static Dictionary<GasDisplay, GasDisplayExtender> gasDisplayDict = new Dictionary<GasDisplay, GasDisplayExtender>();

		public class GasDisplayExtender
		{
			public List<ISetable> SetableDevices = new List<ISetable>();
		}
	}

	[HarmonyPatch(typeof(GasDisplay))]
	[HarmonyPatch("Awake")]
	public class LinkExtenderNDisplay
	{
		static void Postfix(GasDisplay __instance)
		{
			GasDisplayExtenderDict.gasDisplayDict.Add(__instance, new GasDisplayExtenderDict.GasDisplayExtender());
		}
	}

	[HarmonyPatch(typeof(Motherboard))]
	[HarmonyPatch("OnDestroy")]
	public class UnLinkExtenderNDisplay
	{
		static bool Prefix(Motherboard __instance)
		{
			if (__instance is GasDisplay) 
			{
				GasDisplayExtenderDict.gasDisplayDict.Remove((GasDisplay)__instance);
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(GasDisplay))]
	[HarmonyPatch("CanDeviceLink")]
	public class LinkISettable
	{
		static void Postfix(GasDisplay __instance, Device device, ref bool __result)
		{
			if (__result == false)
			{
				if ((device is LogicMemory) || (device is LogicReader) || (device is CircuitHousing))
				{ 
					__result = true;
				}
			}
		}
	}

	[HarmonyPatch(typeof(GasDisplay))]
	[HarmonyPatch("OnDeviceListChanged")]
	public class PatchDeviceListChanged
	{
		static void Postfix(GasDisplay __instance)
		{
			//List<Device> linkedDevices = __instance.LinkedDevices;
			lock (__instance.LinkedDevices)
			{
				GasDisplayExtenderDict.gasDisplayDict[__instance].SetableDevices.Clear();
				foreach (Device device in __instance.LinkedDevices)
				{
					if (((device is LogicMemory) || (device is LogicReader) || (device is CircuitHousing)) && (__instance.IsDeviceConnected(device)))
					{
						ISetable setableDevice = device as ISetable;
						if (setableDevice != null)
						{
							GasDisplayExtenderDict.gasDisplayDict[__instance].SetableDevices.Add(setableDevice);
						}
					}
				}
			}
		}
	}

	[HarmonyPatch(typeof(GasDisplay))]
	[HarmonyPatch("ButtonToggleMode")]
	public class ButtonTogglePatch
	{
		static bool Prefix(GasDisplay __instance)
		{
			KeyCode quantityModifier = KeyMap.QuantityModifier;
			if (KeyManager.GetButton(quantityModifier))
			{ 
				__instance.Flag--;
			}
			else 
			{
				__instance.Flag++;
			}

			if (__instance.Flag == (int)MGDCOPatchHelper.PatchGasDisplayMode.TotalDisplays)
			{
				__instance.Flag = 0;
			}
			else if (__instance.Flag < 0)
			{
				__instance.Flag = (int)MGDCOPatchHelper.PatchGasDisplayMode.TotalDisplays - 1;
			}
			Motherboard.UseComputer(3, __instance.ReferenceId, __instance.ReferenceId, __instance.Flag, true, "");
			return false;
		}
	}

	[HarmonyPatch(typeof(Circuitboard))]
	[HarmonyPatch("InputFinished")]
	public class UpdateFilterButtonPatch
	{
		static bool Prefix(string value, string value2, Circuitboard __instance)
		{
			//if (__instance is GasDisplay && !string.IsNullOrEmpty(value) && value[0] == '/') // if using "/" as mode filter
			if (__instance is GasDisplay && !string.IsNullOrEmpty(value) && KeyManager.GetButton(KeyCode.LeftAlt))
			{
				//string tag = value.Substring(1); // if using "/" as mode filter
				string tag = value;
				int index = 0;
				if (int.TryParse(tag, out index))
				{
					if (index >= 1 && index <= (int)MGDCOPatchHelper.PatchGasDisplayMode.TotalDisplays)
						__instance.SetFlag(index-1);
					return false;
				}
				else {
					tag = tag.ToUpper();
					// query tags
					var queryTag = MGDCOPatchHelper.GasData.Where(p => p.Value.tag.Contains(tag))
															.Select(e => (KeyValuePair<int, (string, string, string, string, Chemistry.GasType?, MGDCOPatchHelper.PatchDataType, bool)>?)e)
															.FirstOrDefault();
					if (queryTag.HasValue)
					{
						__instance.SetFlag(queryTag.Value.Key);
						return false;
					}

					// query display names instead
					var queryDisplayNames = MGDCOPatchHelper.GasData.Where(p => p.Value.displayName.Contains(tag))
															.Select(e => (KeyValuePair<int, (string, string, string, string, Chemistry.GasType?, MGDCOPatchHelper.PatchDataType, bool)>?) e)
															.FirstOrDefault();
					if (queryDisplayNames.HasValue) { 
						__instance.SetFlag(queryDisplayNames.Value.Key);
						return false;
					}
				}
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(GasDisplay))]
	[HarmonyPatch("SetFlag")]
	public class SetFlagPatch {
		private static bool Prefix(int page, GasDisplay __instance, ref int ____lastUnitIndex)
		{
			__instance.Flag = page;
			__instance.DisplayMode = GasDisplayMode.Temperature;
			__instance.DisplayTitle.text = MGDCOPatchHelper.getGasDisplayModeTitle(page);
			__instance.ToggleModeButtonText.text = MGDCOPatchHelper.getGasDisplayModeButtonName(page);
			____lastUnitIndex = 0;
			//__instance.DisplayUnits.text = MGDCOPatchHelper.getDisplayModeUnits(page);
			//if (page == (int)MGDCOPatchHelper.PatchGasDisplayMode.Pressure)
			//{
			//	____lastUnitIndex = Array.IndexOf<string>(____displayUnits, __instance.DisplayUnits.text);
			//}
			// No clue what this diassembly code was supposed to do, but it doesn't work
			//Thing.Event displayModeType = __instance.DisplayModeType;
			//if (displayModeType == null)
			//	return false;
			//displayModeType();
			return false;
		}
	}


	[HarmonyPatch(typeof(GasDisplay))]
	[HarmonyPatch("UpdateEachFrame")]
	class UpdateEachFramePatch
	{
		static void Postfix(GasDisplay __instance, string ____displayText)
		{
			if (____displayText == null) {
				return;
			}
			if (WorldManager.IsGamePaused)
			{
				return;
			}
			var shouldDraw = AccessTools.Method(typeof(GasDisplay), "ShouldDraw");
			if (!(bool)shouldDraw.Invoke(__instance, null))
			{
				return;
			}
			string[] splitDisplayText = ____displayText.Split('|');
			if (splitDisplayText.Length == 2)
			{
				string labelText = ____displayText.Split('|')[0];
				string unitText = ____displayText.Split('|')[1];
				__instance.DisplayLabel.text = labelText;
				__instance.DisplayUnits.text = unitText;
			}
		}
	}

	[HarmonyPatch(typeof(GasDisplay))]
	[HarmonyPatch("OnThreadUpdate")]
	class OnThreadUpdatePatch
	{
		public static List<Device> LinkedDevices(Motherboard instance)
		{
			return instance.LinkedDevices;
		}

		static bool Prefix(GasDisplay __instance, ref float ____temperature, ref float ____pressure, ref int ____sensors, ref string ____displayText, ref bool ____notANumber, ref float ____displayPressure, ref int ____currentUnitIndex, string[] ____displayUnits)
		{
			var shouldDraw = AccessTools.Method(typeof(GasDisplay), "ShouldDraw");
			var errorCheckFromThread = AccessTools.Method(typeof(GasDisplay), "ErrorCheckFromThread");

			List<Device> linkedDevices = LinkedDevices(__instance);

			if (GameManager.IsBatchMode)
			{
				return false;
			}
			if (!(bool)shouldDraw.Invoke(__instance, null))
			{
				return false;
			}
			lock (linkedDevices)
			{
				string displayUnits = MGDCOPatchHelper.getDisplayModeUnits(__instance.Flag);
				if (linkedDevices.Count == 0)
				{
					____displayText = "-";
					if (__instance.Flag != (int)MGDCOPatchHelper.PatchGasDisplayMode.Pressure) 
					{
						____displayText += ("|" + displayUnits);
					}
				}
				else
				{
					GasDisplayMode displayMode = __instance.DisplayMode;
					Chemistry.GasType? gasSelected = MGDCOPatchHelper.getGasDisplayModeGas(__instance.Flag);
					bool combinedQuantity = MGDCOPatchHelper.getGasDisplayModeCombinedFlag(__instance.Flag);
					____sensors = 0;

					float settableNum = 0;
					float settableDevices = 0;
					foreach (ISetable setable in GasDisplayExtenderDict.gasDisplayDict[__instance].SetableDevices)
					{
						if (setable.Setting >= 0)
						{
							settableNum += (float)setable.Setting;
						}
						settableDevices++;
					}
					____temperature = settableNum;
					____pressure = settableNum;
					float ratio = settableNum;
					float quantity = settableNum;
					float volume = settableNum;
					float totalMoles = settableNum;
					float energy = settableNum;

					int gas_sensor_count = __instance.GasSensors.Count;
					while (gas_sensor_count-- > 0)
					{
						GasSensor gasSensor = __instance.GasSensors[gas_sensor_count];
						if (gasSensor && __instance.ParentComputer != null && __instance.ParentComputer.DataCableNetwork != null && __instance.IsDeviceConnected(gasSensor))
						{
							switch (MGDCOPatchHelper.getGasDisplayModePatchDataType(__instance.Flag)) {
								case MGDCOPatchHelper.PatchDataType.Pressure:
									____pressure += gasSensor.AirPressure;
									break;
								case MGDCOPatchHelper.PatchDataType.Temperature:
									____temperature += gasSensor.AirTemperature;
									break;
								case MGDCOPatchHelper.PatchDataType.Energy:
									energy += MGDCOPatchHelper.GetEnergy((MGDCOPatchHelper.PatchGasDisplayMode)__instance.Flag, gasSensor.WorldAtmosphere, gasSensor);
									break;
								case MGDCOPatchHelper.PatchDataType.Ratio:
									totalMoles += MGDCOPatchHelper.GetGasSensorQuantity(null, gasSensor.WorldAtmosphere, true);
									quantity += MGDCOPatchHelper.GetGasSensorQuantity(gasSelected, gasSensor.WorldAtmosphere, combinedQuantity);
									break;
								case MGDCOPatchHelper.PatchDataType.Quantity:
									quantity += MGDCOPatchHelper.GetGasSensorQuantity(gasSelected, gasSensor.WorldAtmosphere, combinedQuantity);
									break;
								case MGDCOPatchHelper.PatchDataType.Volume:
									volume += MGDCOPatchHelper.GetGasSensorLiquidVolume(gasSelected, gasSensor.WorldAtmosphere);
									break;
							}
							____sensors++;
						}
					}
					int pipe_analyzer_count = __instance.PipeAnalysizers.Count;
					while (pipe_analyzer_count-- > 0)
					{
						PipeAnalysizer pipeAnalysizer = __instance.PipeAnalysizers[pipe_analyzer_count];
						if (pipeAnalysizer && __instance.ParentComputer != null && __instance.ParentComputer.DataCableNetwork != null && __instance.IsDeviceConnected(pipeAnalysizer) && pipeAnalysizer.HasReadableAtmosphere)
						{
							switch (MGDCOPatchHelper.getGasDisplayModePatchDataType(__instance.Flag))
							{
								case MGDCOPatchHelper.PatchDataType.Pressure:
									____pressure += pipeAnalysizer.PipePressure;
									break;
								case MGDCOPatchHelper.PatchDataType.Temperature:
									____temperature += pipeAnalysizer.PipeTemperature;
									break;
								case MGDCOPatchHelper.PatchDataType.Energy:
									energy += MGDCOPatchHelper.GetEnergy((MGDCOPatchHelper.PatchGasDisplayMode)__instance.Flag, pipeAnalysizer.NetworkAtmosphere, pipeAnalysizer);
									break;
								case MGDCOPatchHelper.PatchDataType.Ratio:
									totalMoles += MGDCOPatchHelper.GetGasSensorQuantity(null, pipeAnalysizer.NetworkAtmosphere, true);
									quantity += MGDCOPatchHelper.GetGasSensorQuantity(gasSelected, pipeAnalysizer.NetworkAtmosphere, combinedQuantity);
									break;
								case MGDCOPatchHelper.PatchDataType.Quantity:
									quantity += MGDCOPatchHelper.GetGasSensorQuantity(gasSelected, pipeAnalysizer.NetworkAtmosphere, combinedQuantity);
									break;
								case MGDCOPatchHelper.PatchDataType.Volume:
									volume += MGDCOPatchHelper.GetGasSensorLiquidVolume(gasSelected, pipeAnalysizer.NetworkAtmosphere);
									break;
							}
							____sensors++;
						}
					}
					int gas_tank_storage_count = __instance.GasTankStorages.Count;
					while (gas_tank_storage_count-- > 0)
					{
						GasTankStorage gasTankStorage = __instance.GasTankStorages[gas_tank_storage_count];
						if (gasTankStorage && __instance.ParentComputer != null && __instance.ParentComputer.DataCableNetwork != null && __instance.IsDeviceConnected(gasTankStorage) && gasTankStorage.HasReadableAtmosphere)
						{
							switch (MGDCOPatchHelper.getGasDisplayModePatchDataType(__instance.Flag))
							{
								case MGDCOPatchHelper.PatchDataType.Pressure:
									____pressure += gasTankStorage.TankPressure;
									break;
								case MGDCOPatchHelper.PatchDataType.Temperature:
									____temperature += gasTankStorage.TankTemperature;
									break;
								case MGDCOPatchHelper.PatchDataType.Energy:
									energy += MGDCOPatchHelper.GetEnergy((MGDCOPatchHelper.PatchGasDisplayMode)__instance.Flag, gasTankStorage.InternalAtmosphere, gasTankStorage);
									break;
								case MGDCOPatchHelper.PatchDataType.Ratio:
									totalMoles += MGDCOPatchHelper.GetGasSensorQuantity(null, gasTankStorage.InternalAtmosphere, true);
									quantity += MGDCOPatchHelper.GetGasSensorQuantity(gasSelected, gasTankStorage.InternalAtmosphere, combinedQuantity);
									break;
								case MGDCOPatchHelper.PatchDataType.Quantity:
									quantity += MGDCOPatchHelper.GetGasSensorQuantity(gasSelected, gasTankStorage.InternalAtmosphere, combinedQuantity);
									break;
								case MGDCOPatchHelper.PatchDataType.Volume:
									volume += MGDCOPatchHelper.GetGasSensorLiquidVolume(gasSelected, gasTankStorage.InternalAtmosphere);
									break;
							}
							____sensors++;
						}
					}
					int count4 = __instance.Structures.Count;
					while (count4-- > 0)
					{
						Structure structure = __instance.Structures[count4];
						if (structure && __instance.ParentComputer != null && __instance.ParentComputer.DataCableNetwork != null && structure.InternalAtmosphere != null && structure.HasReadableAtmosphere)
						{
							switch (MGDCOPatchHelper.getGasDisplayModePatchDataType(__instance.Flag))
							{
								case MGDCOPatchHelper.PatchDataType.Pressure:
									____pressure += structure.InternalAtmosphere.PressureGassesAndLiquids;
									break;
								case MGDCOPatchHelper.PatchDataType.Temperature:
									____temperature += structure.InternalAtmosphere.Temperature;
									break;
								case MGDCOPatchHelper.PatchDataType.Energy:
									energy += MGDCOPatchHelper.GetEnergy((MGDCOPatchHelper.PatchGasDisplayMode)__instance.Flag, structure.InternalAtmosphere, structure);
									break;
								case MGDCOPatchHelper.PatchDataType.Ratio:
									totalMoles += MGDCOPatchHelper.GetGasSensorQuantity(null, structure.InternalAtmosphere, true);
									quantity += MGDCOPatchHelper.GetGasSensorQuantity(gasSelected, structure.InternalAtmosphere, combinedQuantity);
									break;
								case MGDCOPatchHelper.PatchDataType.Quantity:
									quantity += MGDCOPatchHelper.GetGasSensorQuantity(gasSelected, structure.InternalAtmosphere, combinedQuantity);
									break;
								case MGDCOPatchHelper.PatchDataType.Volume:
									volume += MGDCOPatchHelper.GetGasSensorLiquidVolume(gasSelected, structure.InternalAtmosphere);
									break;
							}
							____sensors++;
						}
					}
					if (settableNum == 0)
					{
						ratio = quantity / totalMoles;
					} 
					else 
					{
						ratio /= (float)settableDevices;
					}
					____temperature /= ((float)____sensors + settableDevices);
					____pressure /= ((float)____sensors + settableDevices);

					if (MGDCOPatchHelper.getGasDisplayModePatchDataType(__instance.Flag) == MGDCOPatchHelper.PatchDataType.Temperature)
					{
						if (float.IsNaN(____temperature))
						{
							____displayText = "NAN" + ("|" + displayUnits);
							if (!____notANumber)
							{
								____notANumber = true;
								var error_check = (UniTaskVoid)errorCheckFromThread.Invoke(__instance, null);
								error_check.Forget();
							}
						}
						else
						{
							if (__instance.Flag == (int)MGDCOPatchHelper.PatchGasDisplayMode.TemperatureKelvin)
							{
								____displayText = MGDCOPatchHelper.FormatSIUnits(____temperature, displayUnits);
							}
							else
							{
								string format = "F1";
								float num = ____temperature - 273.15f;
								if (num >= 1000f)
								{
									format = "F0";
								}
								if (num <= -100f)
								{
									format = "F0";
								}
								if (num == 0)
								{
									format = "F0";
								}
								____displayText = ((____temperature <= 1f) ? "-" : num.ToString(format)) + "|" + displayUnits;
							}
							if (____notANumber)
							{
								____notANumber = false;
								var error_check = (UniTaskVoid)errorCheckFromThread.Invoke(__instance, null);
								error_check.Forget();
							}
						}
					}
					else if (MGDCOPatchHelper.getGasDisplayModePatchDataType(__instance.Flag) == MGDCOPatchHelper.PatchDataType.Ratio)
					{
						if (float.IsNaN(ratio))
						{
							// "|" + displayUnits is at the end of the function
							____displayText = "NAN";
							if (!____notANumber)
							{
								____notANumber = true;
								var error_check = (UniTaskVoid)errorCheckFromThread.Invoke(__instance, null);
								error_check.Forget();
							}
						}
						else
						{
							if (ratio >= 100f) 
							{
								____displayText = "100";
							}
							else if (ratio <= 0f)
							{
								____displayText = "0";

							}
							else
							{
								____displayText = ratio.ToString("P");
							}
						}
						____displayText += ("|" + displayUnits);
					}
					else if (MGDCOPatchHelper.getGasDisplayModePatchDataType(__instance.Flag) == MGDCOPatchHelper.PatchDataType.Volume)
					{
						if (float.IsNaN(volume))
						{
							____displayText = "NAN" + ("|" + displayUnits);
							if (!____notANumber)
							{
								____notANumber = true;
								var error_check = (UniTaskVoid)errorCheckFromThread.Invoke(__instance, null);
								error_check.Forget();
							}
						}
						else
						{
							____displayText = MGDCOPatchHelper.FormatSIUnits(volume, displayUnits);
						}
					}
					else if (MGDCOPatchHelper.getGasDisplayModePatchDataType(__instance.Flag) == MGDCOPatchHelper.PatchDataType.Quantity)
					{
						if (float.IsNaN(quantity))
						{
							____displayText = "NAN" + ("|" + displayUnits);
							if (!____notANumber)
							{
								____notANumber = true;
								var error_check = (UniTaskVoid)errorCheckFromThread.Invoke(__instance, null);
								error_check.Forget();
							}
						}
						else
						{
							____displayText = MGDCOPatchHelper.FormatSIUnits(quantity, displayUnits);
						}
					}
					else if (MGDCOPatchHelper.getGasDisplayModePatchDataType(__instance.Flag) == MGDCOPatchHelper.PatchDataType.Energy)
					{
						if (float.IsNaN(energy))
						{
							____displayText = "NAN" + ("|" + displayUnits);
							if (!____notANumber)
							{
								____notANumber = true;
								var error_check = (UniTaskVoid)errorCheckFromThread.Invoke(__instance, null);
								error_check.Forget();
							}
						}
						else
						{
							____displayText = MGDCOPatchHelper.FormatSIUnits(energy, displayUnits);
						}
					}
					else
					{
						if (float.IsNaN(____pressure))
						{
							____displayText = "NAN";
							if (__instance.Flag == (int)MGDCOPatchHelper.PatchGasDisplayMode.PrecisePressure)
							{
								____displayText += ("|" + displayUnits);
							}
							if (!____notANumber)
							{
								____notANumber = true;
								var error_check = (UniTaskVoid)errorCheckFromThread.Invoke(__instance, null);
								error_check.Forget();
							}
						}
						else
						{
							____displayPressure = Mathf.Lerp(____displayPressure, ____pressure, __instance.LerpSpeed);
							if (__instance.Flag == (int)MGDCOPatchHelper.PatchGasDisplayMode.Pressure)
							{
								____displayText = __instance.FormatDisplayPressure(____displayPressure);
								__instance.DisplayUnits.text = ____displayUnits[____currentUnitIndex];
							}
							else // mode is precise pressure
							{
								float pressureInPa = ____displayPressure * 1000f;
								____displayText = MGDCOPatchHelper.FormatSIUnits(pressureInPa, displayUnits);
							}
							if (____notANumber)
							{
								____notANumber = false;
								var error_check = (UniTaskVoid)errorCheckFromThread.Invoke(__instance, null);
								error_check.Forget();
							}
						}
					}
				}
			
			}
			return false;
		}
	}
}
