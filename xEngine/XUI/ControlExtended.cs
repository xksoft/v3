﻿using System;
using System.Windows.Forms;

namespace xEngine.XUI
{
	public static class ControlExtended
	{
		public static void UpdateUI(this Control control, Action action)
		{
			if (control.InvokeRequired)
			{
				control.BeginInvoke(action);
			}
			else
			{
				action.Invoke();
			}
		}
	}

}
