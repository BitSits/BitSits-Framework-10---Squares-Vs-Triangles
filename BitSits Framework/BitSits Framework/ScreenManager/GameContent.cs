using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameDataLibrary;

#if WINDOWS_PHONE
using System.Windows;
#endif

namespace BitSits_Framework
{
    /// <summary>
    /// All the Contents of the Game is loaded and stored here
    /// so that all other screen can copy from here
    /// </summary>
    public class GameContent
    {
        public ContentManager content;
        
        public Random random = new Random();

        // Textures
        public Texture2D blank, gradient;
        public Texture2D menuBackground, mainMenu, options;

        public Texture2D background;
        public Texture2D[] walk = new Texture2D[2], idle = new Texture2D[2], die = new Texture2D[2];

        public Texture2D mouseOver;
        public Vector2 mouseOverOrigin;

        public Texture2D[] castle = new Texture2D[2];
        public Vector2 castleOrigin;

        public Texture2D pathArrow, pathCross;

        public Texture2D healthBar;

        const int MaxRank = 3;
        public Texture2D[] weapon = new Texture2D[MaxRank], armyOverlay = new Texture2D[MaxRank],
            bullet = new Texture2D[MaxRank];

        public Texture2D[] tutorial = new Texture2D[3];
        public Texture2D levelUp, retry, gameOver;

        // Fonts
        public SpriteFont debugFont, gameFont;
        public int gameFontSize = 68;

        // Audio objects
        public SoundEffect[] noise = new SoundEffect[3];
        public SoundEffect dieSound;
        

        /// <summary>
        /// Load GameContents
        /// </summary>
        public GameContent(GameComponent screenManager)
        {
            content = screenManager.Game.Content;

            blank = content.Load<Texture2D>("Graphics/blank");
            menuBackground = content.Load<Texture2D>("Graphics/menuBackground");
            mainMenu = content.Load<Texture2D>("Graphics/mainMenu");
            options = content.Load<Texture2D>("Graphics/options");

            background = content.Load<Texture2D>("Graphics/background");

            for (int i = 0; i < walk.Length; i++)
            {
                walk[i] = content.Load<Texture2D>("Graphics/" + (Shape)i + "Walk");
                idle[i] = content.Load<Texture2D>("Graphics/" + (Shape)i + "Idle");
                die[i] = content.Load<Texture2D>("Graphics/" + (Shape)i + "Die");
            }

            mouseOver = content.Load<Texture2D>("Graphics/mouseOver");
            mouseOverOrigin = new Vector2(mouseOver.Width, mouseOver.Height) / 2;

            for (int i = 0; i < castle.Length; i++)
                castle[i] = content.Load<Texture2D>("Graphics/" + (Shape)i + "Castle");
            castleOrigin = new Vector2(castle[0].Width / 2, castle[0].Height);

            pathArrow = content.Load<Texture2D>("Graphics/pathArrow");
            pathCross = content.Load<Texture2D>("Graphics/pathCross");

            healthBar = content.Load<Texture2D>("Graphics/healthBar");

            for (int i = 0; i < MaxRank; i++)
            {
                weapon[i] = content.Load<Texture2D>("Graphics/" + (Rank)i + "Weapon");
                //armyOverlay[i] = content.Load<Texture2D>("Graphics/" + ((Rank)(i)).ToString() + "Overlay");
                bullet[i] = content.Load<Texture2D>("Graphics/" + (Rank)i + "Bullet");
            }

            for (int i = 0; i < tutorial.Length; i++)
                tutorial[i] = content.Load<Texture2D>("Graphics/tutotrial" + i);

            levelUp = content.Load<Texture2D>("Graphics/levelUp");
            retry = content.Load<Texture2D>("Graphics/retry");
            gameOver = content.Load<Texture2D>("Graphics/gameOver");

            debugFont = content.Load<SpriteFont>("Fonts/debugFont");
            gameFont = content.Load<SpriteFont>("Fonts/bicho_plumon" + gameFontSize);

            for (int i = 0; i < noise.Length; i++)
                noise[i] = content.Load<SoundEffect>("Audio/" + ((Rank)i).ToString() + "WeaponNoise");

            dieSound = content.Load<SoundEffect>("Audio/die");

#if DEBUG
            MediaPlayer.Volume = .4f; SoundEffect.MasterVolume = .4f;
#else
            if (BitSitsGames.Settings.MusicEnabled) PlayMusic();

            if (BitSitsGames.Settings.SoundEnabled) SoundEffect.MasterVolume = 1;
            else SoundEffect.MasterVolume = 0;            
#endif

            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            screenManager.Game.ResetElapsedTime();
        }


        public void PlayMusic()
        {
            if (MediaPlayer.GameHasControl)
            {
                if (MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();

                    return;
                }
            }
            else if (MediaPlayer.State == MediaState.Playing)
            {
#if WINDOWS_PHONE
                MessageBoxResult Choice;

                Choice = MessageBox.Show("Media is currently playing, do you want to stop it?",
                    "Stop Player", MessageBoxButton.OKCancel);

                if (Choice == MessageBoxResult.OK) MediaPlayer.Pause();
                else
                {
                    BitSitsGames.Settings.MusicEnabled = false;
                    return;
                }
#endif
            }

            MediaPlayer.Play(content.Load<Song>("Audio/Marching by Pill"));
            MediaPlayer.IsRepeating = true;
        }

        
        /// <summary>
        /// Unload GameContents
        /// </summary>
        public void UnloadContent() { content.Unload(); }
    }
}
