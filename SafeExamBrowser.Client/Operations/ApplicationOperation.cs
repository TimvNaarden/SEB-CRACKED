﻿/*
 * Copyright (c) 2019 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.Linq;
using SafeExamBrowser.Client.Operations.Events;
using SafeExamBrowser.Core.Contracts.OperationModel;
using SafeExamBrowser.Core.Contracts.OperationModel.Events;
using SafeExamBrowser.I18n.Contracts;
using SafeExamBrowser.Logging.Contracts;
using SafeExamBrowser.Monitoring.Contracts.Applications;
using SafeExamBrowser.Settings;
using SafeExamBrowser.Settings.Applications;

namespace SafeExamBrowser.Client.Operations
{
	internal class ApplicationOperation : ClientOperation
	{
		private ILogger logger;
		private IApplicationMonitor applicationMonitor;

		public override event ActionRequiredEventHandler ActionRequired;
		public override event StatusChangedEventHandler StatusChanged;

		public ApplicationOperation(IApplicationMonitor applicationMonitor, ClientContext context, ILogger logger) : base(context)
		{
			this.applicationMonitor = applicationMonitor;
			this.logger = logger;
		}

		public override OperationResult Perform()
		{
			logger.Info("Initializing applications...");
			StatusChanged?.Invoke(TextKey.OperationStatus_InitializeProcessMonitoring);

			var result = InitializeApplications();

			if (result == OperationResult.Success)
			{
				StartMonitor();
			}

			return result;
		}

		public override OperationResult Revert()
		{
			logger.Info("Finalizing applications...");
			StatusChanged?.Invoke(TextKey.OperationStatus_StopProcessMonitoring);

			TerminateApplications();
			StopMonitor();

			return OperationResult.Success;
		}

		private OperationResult InitializeApplications()
		{
			var initialization = applicationMonitor.Initialize(Context.Settings.Applications);
			var result = OperationResult.Success;

			if (initialization.RunningApplications.Any())
			{
				result = TryTerminate(initialization.RunningApplications);
			}

			if (result == OperationResult.Success)
			{
				foreach (var application in Context.Settings.Applications.Whitelist)
				{
					Create(application);
				}
			}

			return result;
		}

		private void Create(WhitelistApplication application)
		{
			// TODO: Use IApplicationFactory to create new application according to configuration, load into Context.Applications
		}

		private void StartMonitor()
		{
			if (Context.Settings.KioskMode != KioskMode.None)
			{
				applicationMonitor.Start();
			}
		}

		private void StopMonitor()
		{
			if (Context.Settings.KioskMode != KioskMode.None)
			{
				applicationMonitor.Stop();
			}
		}

		private void TerminateApplications()
		{

		}

		private OperationResult TryTerminate(IEnumerable<RunningApplicationInfo> runningApplications)
		{
			var args = new ProcessTerminationEventArgs();
			var result = OperationResult.Success;

			ActionRequired?.Invoke(args);

			if (args.TerminateProcesses)
			{
				// TODO: Terminate all processes of all running applications

				//foreach (var application in runningApplications)
				//{
				//	foreach (var process in application.Processes)
				//	{
				//		process.Kill();
				//	}
				//}
			}
			else
			{
				result = OperationResult.Aborted;
			}

			return result;
		}
	}
}