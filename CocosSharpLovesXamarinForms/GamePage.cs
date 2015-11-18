using System;
using System.Collections.Generic;
using Xamarin.Forms;
using CocosSharp;

namespace CocosSharpLovesXamarinForms
{
	public class GamePage : ContentPage
	{
		CocosSharpView gameView;
		ListView listView;
		AbsoluteLayout mainAbsoluteLayout;

		public GamePage ()
		{
			listView = new ListView () {
				ItemsSource = "These are some words I want to display in a list".Split (new[] { ' ' }),
			};
			gameView = new CocosSharpView () {
				// Set the game world dimensions
				DesignResolution = new Size (1024, 768),
				// Set the method to call once the view has been initialised
				ViewCreated = LoadGame
			};
			mainAbsoluteLayout = new AbsoluteLayout () {
				Children = {
					listView,
					gameView,
				},
			};

			Content = mainAbsoluteLayout;
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren(x, y, width, height);

			AbsoluteLayout.SetLayoutBounds (listView, new Rectangle (0, 0, width, height / 2));
			AbsoluteLayout.SetLayoutBounds (gameView, new Rectangle (0, height / 2, width, height / 2));
		}

		protected override void OnDisappearing ()
		{
			if (gameView != null) {
				gameView.Paused = true;
			}

			base.OnDisappearing ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			if (gameView != null)
				gameView.Paused = false;
		}

		void LoadGame (object sender, EventArgs e)
		{
			var nativeGameView = sender as CCGameView;

			if (nativeGameView != null) {
				var contentSearchPaths = new List<string> () { "Fonts", "Sounds" };
				CCSizeI viewSize = nativeGameView.ViewSize;
				CCSizeI designResolution = nativeGameView.DesignResolution;

				// Determine whether to use the high or low def versions of our images
				// Make sure the default texel to content size ratio is set correctly
				// Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
				if (designResolution.Width < viewSize.Width) {
					contentSearchPaths.Add ("Images/Hd");
					CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
				} else {
					contentSearchPaths.Add ("Images/Ld");
					CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
				}

				nativeGameView.ContentManager.SearchPaths = contentSearchPaths;

				CCScene gameScene = new CCScene (nativeGameView);
				gameScene.AddLayer (new GameLayer ());
				nativeGameView.RunWithScene (gameScene);
			}
		}
	}
}
