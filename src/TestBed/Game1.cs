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

            // set up the Phoenix Framework by creating a channel manager, entity manager, and game manager
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
            
            // load some content and initialize the animationCache
            font = Content.Load<SpriteFont>("Status");
            SpriteSheetLoader loader = new SpriteSheetLoader(Content, _fileReader);
            var ss = loader.Load("fanatiblaster");

            var frame = ss.SpriteList[SpriteNames.Down_spritesheetforthegame_1_0];

            // the animation cache couuld be much more feature rich
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

            // set up the systems.  they get executed in order of priority (least to greatest) in the update and (for IDrawable systems) draw methods
            // the list of channels determines if the system executes based on the current channel.  "all" executes no matter what the current channel
            // is.  "default" is the default initial channel.  This is extremely useful for dealing with gamestates (such as paused etc)
            MovementSystem movementSystem = new MovementSystem(_channelManager, 25, new string[] { "default" });
            SpriteBatchRenderSystem textureRenderSystem = new SpriteBatchRenderSystem(GraphicsDevice, _channelManager, 101, "default");
            SpriteAnimationSystem spriteAnimationSystem = new SpriteAnimationSystem(animationCache, _channelManager, 30, "default");
            LerpColorSystem alphaTweenSystem = new LerpColorSystem(_channelManager, 40, "default");
            Camera2dSystem cameraSystem = new Camera2dSystem(_channelManager, 50, "all");
            TestBedIntentSystem testBedIntentSystem = new TestBedIntentSystem(_channelManager, 10, "all");
            CameraMovementSystem cameraMovementSystem = new CameraMovementSystem(_channelManager, 20, "default");
            _gameManager.AddSystem(movementSystem);
            _gameManager.AddSystem(textureRenderSystem);
            _gameManager.AddSystem(spriteAnimationSystem);
            _gameManager.AddSystem(alphaTweenSystem);
            _gameManager.AddSystem(cameraSystem);
            _gameManager.AddSystem(testBedIntentSystem);
            _gameManager.AddSystem(cameraMovementSystem);

            // create some entities to test some stuff out... usually you'd have a manager class set up to manage the lifetime of entities (create and destroy them etc)
            // but in this case we're just playing around.

            // this entity simply triggers the necessary aspects to get the Movement and Intent systems.
            // this way we can use simple keyboard input to slide the camera around and zoom it in and out.
            var movementEntity = _gameManager.EntityManager.Get("movementEntity")
                                                .AddComponent(new CameraIntentMappingComponent())
                                                .AddComponent(new TestBedIntentComponent());
            _gameManager.AddEntity(movementEntity);

            //create camera entity.  We give it a VelocityComponent so it will be moved by our movementsystem
            var camera = _gameManager.EntityManager.Get("camera", new string[] { "all" });
            camera.AddComponent(new PositionComponent() { CurrentPosition = new Vector2(0,0) })
                  .AddComponent(new Camera2dComponent(GraphicsDevice.Viewport)
                  {
                      MaxZoom = 1.5f,
                      MinZoom = .5f,
                      Zoom = 1f,
                      Limits = new Rectangle(0,0,GraphicsDevice.Viewport.Width * 2, GraphicsDevice.Viewport.Height * 2)
                  })
                  .AddComponent(new RotationComponent())
                  .AddComponent(new SpriteBatchIdentifierComponent() { Identifier = "main" })
                  .AddComponent(new VelocityComponent() { Speed = new Vector2(250, 250) });
            _gameManager.AddEntity(camera);

            // this triggers the spritebatch system with an identifier of main.
            var spriteBatch = _gameManager.EntityManager.Get("mainSpriteBatch", new string[] { "all" })
                                        .AddComponent(new SpriteBatchComponent())
                                        .AddComponent(new SpriteBatchIdentifierComponent() { Identifier = "main" });
            _gameManager.AddEntity(spriteBatch);

            // a text component.  uses a helper extension method to add a bunch of components under the hood
            var te = _gameManager.EntityManager.Get("text", new string[] { "default" });
            te.CreateTextRenderEntity("Use W,A,S,D to move the camera.\nQ,E to zoom", Color.Black, new Vector2(0, 0), 5, 1.0f, font, "main");

            // more text.  I put this over on the right edge when I was messing with the camera to make sure the boundaries were working
            var te2 = _gameManager.EntityManager.Get("textWidth")
                                    .CreateTextRenderEntity(string.Format("({0},{1})", GraphicsDevice.Viewport.Width * 2, GraphicsDevice.Viewport.Height * 2), Color.Black,
                                                                new Vector2(GraphicsDevice.Viewport.Width * 2 - 200, 0), 5, 1.0f, font, "main");
            _gameManager.AddEntity(te2);

            // more text.  In this example we add velocity just to show how easy it is to make something move (or able to move)
            var teMove = _gameManager.EntityManager.Get("text2", new string[] { "default" });
            teMove.CreateTextRenderEntity("I'm Moving!", Color.Black, new Vector2(0, 250), 5, 1.0f, font, "main")
                    .AddComponent(new VelocityComponent() { Direction = new Vector2(1, 1), Speed = new Vector2(75, 0) });

            // this is the sprite.  we make this animated by adding an animation component.  Also it moves because velocity.
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

            // a sprite that's also animated and fades in and out using the LerpColorComponent
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
            // all entities need to be added to the game manager or they do nothing.
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
