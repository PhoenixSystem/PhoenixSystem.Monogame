using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhoenixSystem.Engine.Channel;
using PhoenixSystem.Engine.Collections;
using PhoenixSystem.Engine.Entity;
using PhoenixSystem.Monogame;
using PhoenixSystem.Monogame.Components;
using PhoenixSystem.Monogame.Render;
using PhoenixSystem.Monogame.Systems;
using PhoenixSystem.Monogame.Aspects;
namespace TestBed
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        TestBedGameManager _gameManager;
        SpriteFont font;
        IChannelManager _channelManager;
        private IFileReader _fileReader;

        public Game(IFileReader fileReader)
        {

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            _channelManager = new ChannelManager();
            _fileReader = fileReader;
            var entityManager = new EntityManager(_channelManager, new EntityPool());
            _gameManager = new TestBedGameManager(new DefaultEntityAspectManager(_channelManager, entityManager), entityManager, _channelManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {


            // Create a new SpriteBatch, which can be used to draw textures.
            font = Content.Load<SpriteFont>("Status");
            SpriteSheetLoader loader = new SpriteSheetLoader(Content, _fileReader);
            var ss = loader.Load("fanatiblaster");

            var frame = ss.SpriteList[SpriteNames.Down_spritesheetforthegame_1_0];

            var animationCache = new AnimationCache(ss);
            animationCache.Animations.Add("down", new string[] {
                SpriteNames.Down_spritesheetforthegame_1_1,
                SpriteNames.Down_spritesheetforthegame_1_2,
                SpriteNames.Down_spritesheetforthegame_1_3,
                SpriteNames.Down_spritesheetforthegame_1_4,
                SpriteNames.Down_spritesheetforthegame_1_5,
                SpriteNames.Down_spritesheetforthegame_1_6,
                SpriteNames.Down_spritesheetforthegame_1_7,
                SpriteNames.Down_spritesheetforthegame_1_8
            });

            MovementSystem movementSystem = new MovementSystem(_channelManager, 25, new string[] { "default" });
            SpriteBatchRenderSystem textureRenderSystem = new SpriteBatchRenderSystem(GraphicsDevice, _channelManager, 101, "default");
            SpriteAnimationSystem spriteAnimationSystem = new SpriteAnimationSystem(animationCache, _channelManager, 30, "default");
            LerpColorSystem alphaTweenSystem = new LerpColorSystem(_channelManager, 40, "default");
            Camera2dSystem cameraSystem = new Camera2dSystem(_channelManager, 50, "all");
            _gameManager.AddSystem(movementSystem);
            _gameManager.AddSystem(textureRenderSystem);
            _gameManager.AddSystem(spriteAnimationSystem);
            _gameManager.AddSystem(alphaTweenSystem);
            _gameManager.AddSystem(cameraSystem);


            //create camera entity
            var camera = _gameManager.EntityManager.Get("camera", new string[] { "all" });
            camera.AddComponent(new PositionComponent() { CurrentPosition = new Vector2(5000,0) })
                  .AddComponent(new Camera2dComponent(GraphicsDevice.Viewport)
                  {
                      MaxZoom = 1.5f,
                      MinZoom = .5f,
                      Zoom = 1f,
                      Limits = new Rectangle(0,0,GraphicsDevice.Viewport.Width * 2, GraphicsDevice.Viewport.Height * 2)
                  })
                  .AddComponent(new RotationComponent())
                  .AddComponent(new SpriteBatchIdentifierComponent() { Identifier = "main" });
            _gameManager.AddEntity(camera);
            var spriteBatch = _gameManager.EntityManager.Get("mainSpriteBatch", new string[] { "all" }).AddComponent(new SpriteBatchComponent()).AddComponent(new SpriteBatchIdentifierComponent() { Identifier = "main" });
            _gameManager.AddEntity(spriteBatch);
            var te = _gameManager.EntityManager.Get("text", new string[] { "default" });
            te.CreateTextRenderEntity("(0,0)", Color.Black, new Vector2(0, 0), 5, 1.0f, font, "main");

            var te2 = _gameManager.EntityManager.Get("text2").CreateTextRenderEntity(string.Format("({0},{1})", GraphicsDevice.Viewport.Width * 2, GraphicsDevice.Viewport.Height * 2), Color.Black,
                                                                new Vector2(GraphicsDevice.Viewport.Width * 2 - 200, 0), 5, 1.0f, font, "main");
            _gameManager.AddEntity(te2);
            var teMove = _gameManager.EntityManager.Get("text2", new string[] { "default" });
            teMove.CreateTextRenderEntity("I'm Moving!", Color.Black, new Vector2(1, 1), 5, 1.0f, font, "main")
                    .AddComponent(new VelocityComponent() { Direction = new Vector2(1, 1), Speed = new Vector2(30, 0) });
            var teSprite = _gameManager.EntityManager.Get("sprite");
            teSprite.MakeTextureRenderAspect(new Vector2(150, 150), frame.IsRotated, frame.Origin, frame.SourceRectangle,
                                                frame.Texture, SpriteEffects.None, Color.White, 1.0f, 0.0f, "main")
                                            .AddComponent(new AnimationComponent()
                                            {
                                                Active = true,
                                                CurrentAnimation = "down",
                                                CurrentFrameIndex = 0,
                                                ShouldLoop = true,
                                                FPS = 8.0f
                                            })
                                            .AddComponent(new VelocityComponent() { Direction = new Vector2(1, 1), Speed = new Vector2(30, 0) }); ;

            var teFadingSprite = _gameManager.EntityManager.Get("fadingSprite");
            teFadingSprite.MakeTextureRenderAspect(new Vector2(300, 150), frame.IsRotated, frame.Origin, frame.SourceRectangle,
                                                        frame.Texture, SpriteEffects.None, Color.White, 1.0f, 0.0f, "main")
                                                        .AddComponent(new AnimationComponent()
                                                        {
                                                            Active = true,
                                                            CurrentAnimation = "down",
                                                            CurrentFrameIndex = 0,
                                                            ShouldLoop = true,
                                                            FPS = 8.0f
                                                        })
                                                        .AddComponent(new LerpColorComponent()
                                                        {
                                                            From = Color.White,
                                                            To = Color.Transparent,
                                                            Loop = true,
                                                            DurationInSeconds = 2.0f
                                                        });
            _gameManager.AddEntity(te);
            _gameManager.AddEntity(teMove);
            _gameManager.AddEntity(teSprite);
            _gameManager.AddEntity(teFadingSprite);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _gameManager.Update(new MonogameTickEvent(gameTime));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _gameManager.Draw(new MonogameTickEvent(gameTime));
            base.Draw(gameTime);
        }
    }
}
