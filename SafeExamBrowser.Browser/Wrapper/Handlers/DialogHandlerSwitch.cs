﻿/*
 * Copyright (c) 2022 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Host;
using System.Collections.Generic;

namespace SafeExamBrowser.Browser.Wrapper.Handlers
{
	internal class DialogHandlerSwitch : IDialogHandler
	{
		public bool OnFileDialog(IWebBrowser webBrowser, IBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, List<string> acceptFilters, IFileDialogCallback callback)
		{
			if (browser.IsPopup)
			{
				var control = ChromiumHostControl.FromBrowser(browser) as CefSharpPopupControl;

				control?.OnFileDialog(webBrowser, browser, mode, title, defaultFilePath, acceptFilters, callback);
			}
			else
			{
				var control = ChromiumWebBrowser.FromBrowser(browser) as CefSharpBrowserControl;

				control?.OnFileDialog(webBrowser, browser, mode, title, defaultFilePath, acceptFilters, callback);
			}

			return true;
		}
	}
}
