﻿/*
 * Copyright (c) 2022 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using SafeExamBrowser.Logging.Contracts;
using SafeExamBrowser.SystemComponents.Contracts;
using System.Windows.Forms;

namespace SafeExamBrowser.SystemComponents
{
	public class RemoteSessionDetector : IRemoteSessionDetector
	{
		private readonly ILogger logger;

		public RemoteSessionDetector(ILogger logger)
		{
			this.logger = logger;
		}

		public bool IsRemoteSession()
		{
			var isRemoteSession = SystemInformation.TerminalServerSession;

			logger.Debug($"System appears {(isRemoteSession ? "" : "not ")}to be running in a remote session.");

			return isRemoteSession;
		}
	}
}
