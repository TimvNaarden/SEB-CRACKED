﻿/*
 * Copyright (c) 2022 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using SafeExamBrowser.Core.Contracts.Resources.Icons;
using SafeExamBrowser.I18n.Contracts;
using SafeExamBrowser.Proctoring.Contracts;
using SafeExamBrowser.Settings.Proctoring;
using SafeExamBrowser.UserInterface.Contracts.Shell;
using SafeExamBrowser.UserInterface.Shared.Utilities;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SafeExamBrowser.UserInterface.Mobile.Controls.ActionCenter
{
	public partial class RaiseHandControl : UserControl, INotificationControl
	{
		private readonly IProctoringController controller;
		private readonly ProctoringSettings settings;
		private readonly IText text;

		private IconResource LoweredIcon;
		private IconResource RaisedIcon;

		public RaiseHandControl(IProctoringController controller, ProctoringSettings settings, IText text)
		{
			this.controller = controller;
			this.settings = settings;
			this.text = text;

			InitializeComponent();
			InitializeRaiseHandControl();
		}

		private void InitializeRaiseHandControl()
		{
			var originalBrush = Grid.Background;

			controller.HandLowered += () => Dispatcher.Invoke(ShowLowered);
			controller.HandRaised += () => Dispatcher.Invoke(ShowRaised);

			HandButton.Click += RaiseHandButton_Click;
			HandButtonText.Text = text.Get(TextKey.Notification_ProctoringRaiseHand);

			LoweredIcon = new XamlIconResource { Uri = new Uri("pack://application:,,,/SafeExamBrowser.UserInterface.Desktop;component/Images/Hand_Lowered.xaml") };
			RaisedIcon = new XamlIconResource { Uri = new Uri("pack://application:,,,/SafeExamBrowser.UserInterface.Desktop;component/Images/Hand_Raised.xaml") };
			Icon.Content = IconResourceLoader.Load(LoweredIcon);

			NotificationButton.MouseLeave += (o, args) => Task.Delay(250).ContinueWith(_ => Dispatcher.Invoke(() => Popup.IsOpen = Popup.IsMouseOver));
			NotificationButton.PreviewMouseLeftButtonUp += NotificationButton_PreviewMouseLeftButtonUp;
			NotificationButton.PreviewMouseRightButtonUp += NotificationButton_PreviewMouseRightButtonUp;
			NotificationButton.ToolTip = text.Get(TextKey.Notification_ProctoringHandLowered);

			Popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(Popup_PlacementCallback);
			Popup.MouseLeave += (o, args) => Task.Delay(250).ContinueWith(_ => Dispatcher.Invoke(() => Popup.IsOpen = IsMouseOver));
			Popup.Opened += (o, args) => Grid.Background = Brushes.Gray;
			Popup.Closed += (o, args) => Grid.Background = originalBrush;

			Text.Text = text.Get(TextKey.Notification_ProctoringHandLowered);
		}

		private void NotificationButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (settings.ForceRaiseHandMessage || Popup.IsOpen)
			{
				Popup.IsOpen = !Popup.IsOpen;
			}
			else
			{
				ToggleHand();
			}
		}

		private void NotificationButton_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			Popup.IsOpen = !Popup.IsOpen;
		}

		private CustomPopupPlacement[] Popup_PlacementCallback(Size popupSize, Size targetSize, Point offset)
		{
			return new[]
			{
				new CustomPopupPlacement(new Point(targetSize.Width / 2 - popupSize.Width / 2, -popupSize.Height), PopupPrimaryAxis.None)
			};
		}

		private void RaiseHandButton_Click(object sender, RoutedEventArgs e)
		{
			ToggleHand();
		}

		private void ToggleHand()
		{
			if (controller.IsHandRaised)
			{
				controller.LowerHand();
			}
			else
			{
				controller.RaiseHand(Message.Text);
				Message.Clear();
			}
		}

		private void ShowLowered()
		{
			HandButtonText.Text = text.Get(TextKey.Notification_ProctoringRaiseHand);
			Icon.Content = IconResourceLoader.Load(LoweredIcon);
			Message.IsEnabled = true;
			NotificationButton.ToolTip = text.Get(TextKey.Notification_ProctoringHandLowered);
			Popup.IsOpen = false;
			Text.Text = text.Get(TextKey.Notification_ProctoringHandLowered);
		}

		private void ShowRaised()
		{
			HandButtonText.Text = text.Get(TextKey.Notification_ProctoringLowerHand);
			Icon.Content = IconResourceLoader.Load(RaisedIcon);
			Message.IsEnabled = false;
			NotificationButton.ToolTip = text.Get(TextKey.Notification_ProctoringHandRaised);
			Text.Text = text.Get(TextKey.Notification_ProctoringHandRaised);
		}
	}
}
