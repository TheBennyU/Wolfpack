using Xamarin.Forms;
using System;

namespace Wolfpack
{
	public partial class WolfpackPage : ContentPage
	{
		public WolfpackPage()
		{
			InitializeComponent();
		}

		void OnButtonClicked(object sender, EventArgs args)
		{
			Navigation.PushModalAsync(new AddPlayersPage());
		}

	}
}

