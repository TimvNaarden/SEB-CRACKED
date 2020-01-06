﻿/*
 * Copyright (c) 2020 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using SafeExamBrowser.Settings;
using SafeExamBrowser.Settings.Security;
using SafeExamBrowser.Settings.Service;

namespace SafeExamBrowser.Configuration.ConfigurationData.DataMapping
{
	internal class SecurityDataMapper : BaseDataMapper
	{
		internal override void Map(string key, object value, AppSettings settings)
		{
			switch (key)
			{
				case Keys.ConfigurationFile.AdminPasswordHash:
					MapAdminPasswordHash(settings, value);
					break;
				case Keys.Security.AllowVirtualMachine:
					MapVirtualMachinePolicy(settings, value);
					break;
				case Keys.Security.QuitPasswordHash:
					MapQuitPasswordHash(settings, value);
					break;
				case Keys.Security.ServicePolicy:
					MapServicePolicy(settings, value);
					break;
			}
		}

		internal override void MapGlobal(IDictionary<string, object> rawData, AppSettings settings)
		{
			MapApplicationLogAccess(rawData, settings);
			MapKioskMode(rawData, settings);
		}

		private void MapAdminPasswordHash(AppSettings settings, object value)
		{
			if (value is string hash)
			{
				settings.Security.AdminPasswordHash = hash;
			}
		}

		private void MapApplicationLogAccess(IDictionary<string, object> rawData, AppSettings settings)
		{
			var hasValue = rawData.TryGetValue(Keys.General.AllowApplicationLog, out var value);

			if (hasValue && value is bool allow)
			{
				settings.Security.AllowApplicationLogAccess = allow;
			}

			if (settings.Security.AllowApplicationLogAccess)
			{
				settings.ActionCenter.ShowApplicationLog = true;
			}
			else
			{
				settings.ActionCenter.ShowApplicationLog = false;
				settings.Taskbar.ShowApplicationLog = false;
			}
		}

		private void MapKioskMode(IDictionary<string, object> rawData, AppSettings settings)
		{
			var hasCreateNewDesktop = rawData.TryGetValue(Keys.Security.KioskModeCreateNewDesktop, out var createNewDesktop);
			var hasDisableExplorerShell = rawData.TryGetValue(Keys.Security.KioskModeDisableExplorerShell, out var disableExplorerShell);

			if (hasDisableExplorerShell && disableExplorerShell as bool? == true)
			{
				settings.Security.KioskMode = KioskMode.DisableExplorerShell;
			}

			if (hasCreateNewDesktop && createNewDesktop as bool? == true)
			{
				settings.Security.KioskMode = KioskMode.CreateNewDesktop;
			}

			if (hasCreateNewDesktop && hasDisableExplorerShell && createNewDesktop as bool? == false && disableExplorerShell as bool? == false)
			{
				settings.Security.KioskMode = KioskMode.None;
			}
		}

		private void MapQuitPasswordHash(AppSettings settings, object value)
		{
			if (value is string hash)
			{
				settings.Security.QuitPasswordHash = hash;
			}
		}

		private void MapServicePolicy(AppSettings settings, object value)
		{
			const int WARN = 1;
			const int FORCE = 2;

			if (value is int policy)
			{
				settings.Service.Policy = policy == FORCE ? ServicePolicy.Mandatory : (policy == WARN ? ServicePolicy.Warn : ServicePolicy.Optional);
			}
		}

		private void MapVirtualMachinePolicy(AppSettings settings, object value)
		{
			if (value is bool allow)
			{
				settings.Security.VirtualMachinePolicy = allow ? VirtualMachinePolicy.Allow : VirtualMachinePolicy.Deny ;
			}
		}
	}
}
