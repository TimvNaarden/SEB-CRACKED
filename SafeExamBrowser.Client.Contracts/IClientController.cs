﻿/*
 * Copyright (c) 2020 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace SafeExamBrowser.Client.Contracts
{
	/// <summary>
	/// Controls the lifetime and is responsible for the event handling of the client application component.
	/// </summary>
	public interface IClientController
	{
		/// <summary>
		/// Reverts any changes, releases all used resources and terminates the client.
		/// </summary>
		void Terminate();

		/// <summary>
		/// Tries to start the client. Returns <c>true</c> if successful, otherwise <c>false</c>.
		/// </summary>
		bool TryStart();

		/// <summary>
		/// Instructs the controller to update the application configuration.
		/// </summary>
		void UpdateAppConfig();
	}
}
