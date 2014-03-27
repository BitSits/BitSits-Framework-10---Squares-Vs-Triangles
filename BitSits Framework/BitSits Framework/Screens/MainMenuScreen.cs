using Microsoft.Xna.Framework;
using GameDataLibrary;

namespace BitSits_Framework
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization


        public override void LoadContent()
        {
            titleTexture = ScreenManager.GameContent.mainMenu;
            //titlePosition = new Vector2(80, 200);

            // Create our menu entries.
            MenuEntry contGameMenuEntry = new MenuEntry(this, "Continue", new Vector2(510, 320));
            MenuEntry newGameMenuEntry = new MenuEntry(this, "New Game", new Vector2(470, 390));
            MenuEntry optionsMenuEntry = new MenuEntry(this, "Options", new Vector2(520, 460));
            MenuEntry exitMenuEntry = new MenuEntry(this, "Exit", new Vector2(570, 530));

            // Hook up menu event handlers.
            contGameMenuEntry.Selected += ContGameMenuEntrySelected;
            newGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            if (BitSitsGames.ScoreData.Level != 0) MenuEntries.Add(contGameMenuEntry);

            MenuEntries.Add(newGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BitSitsGames.ScoreData.Level = 0;
            BitSitsGames.ScoreData.Score = 0;

            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex,
                               new GameplayScreen(), new PauseMenuScreen());
        }


        void ContGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex,
                               new GameplayScreen(), new PauseMenuScreen());
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
#if WINDOWS_PHONE
            ScreenManager.Game.Exit();
#endif
        }


        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
