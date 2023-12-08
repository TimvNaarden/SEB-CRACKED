﻿/*
 * Copyright (c) 2022 ETH Zürich, Educational Development and Technology (LET)
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using SafeExamBrowser.Logging.Contracts;
using System;
using System.Collections.Generic;

namespace SafeExamBrowser.Lockdown.FeatureConfigurations.RegistryConfigurations.MachineHive
{
	[Serializable]
	internal class MachinePowerOptionsConfiguration : MachineHiveConfiguration
	{
		protected override IEnumerable<RegistryConfigurationItem> Items => new[]
		{
			new RegistryConfigurationItem(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoClose", 1, 0)
		};

		public MachinePowerOptionsConfiguration(Guid groupId, ILogger logger) : base(groupId, logger)
		{
		}

		public override bool Reset()
		{
			return DeleteConfiguration();
		}
	}
}
