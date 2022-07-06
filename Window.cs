using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using LearnOpenTK.Common;

namespace ConsoleApp3
{
    class Window : GameWindow
    {
        /*private readonly string path = "../../../";*/

        List<Asset3d> objectList = new List<Asset3d>();

        List<Asset3d> pointLights = new List<Asset3d>();

        Asset3d light;

        Asset3d bug1;

        Camera camera;

        /*private bool firstMove = true;*/

        private int renderSetting = 1;

        private float cameraSpeed = 12.0f;
        /*private float sensitivity = 0.2f;*/
        /*private Vector2 lastPos;*/

        private float totalTime;

        private List<float> moveOffset = new List<float>();

        // variabel utk animasi cerita
        float a = 0;
        float b = 0;
        float c = 0;
        float delay = 1;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            base.OnLoad();

            camera = new Camera(new Vector3(0, 0, 16), Size.X / (float)Size.Y);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // GL.Enable(EnableCap.Blend);
            // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            var random = new Random();

            List<Matrix4> models = new List<Matrix4>();
            for (int i = 0; i < 5000; i++)
            {
                Matrix4 tempModel = Matrix4.Identity;
                tempModel *= Matrix4.CreateTranslation((float)random.NextDouble() * 400 - 200, (float)random.NextDouble() * 25 - 12.5f, (float)random.NextDouble() * 400 - 200);
                models.Add(tempModel);

                moveOffset.Add((float)random.NextDouble() * 100 - 50);
            }

            #region Kecoak
            // KAKI 1
            var cockroach = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 1, 1));
            var cube1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 0.5f, 0.5f));
            cube1.createCuboid_v2(0f, 0f, 0f, 0.1f, 1.5f, true);
            cube1.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube1.translate(0f, 0f, .5f);
            var cube2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1.5f, 0.5f, 0.5f));
            cube2.createCuboid_v2(0f, 0f, 0.75f, 0.1f, 1.5f, true);
            cube2.rotate(Vector3.Zero, Vector3.UnitX, 45);
            cube2.translate(0f, 0f, .5f);

            var cube7 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 0.5f, 0.5f));
            cube7.createCuboid_v2(0f, 0f, 0f, 0.1f, 1.5f, true);
            cube7.rotate(Vector3.Zero, Vector3.UnitX, 45);
            cube7.translate(0f, 0f, -1.5f);
            var cube8 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1.5f, 0.5f, 0.5f));
            cube8.createCuboid_v2(0f, 0f, -0.75f, 0.1f, 1.5f, true);
            cube8.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube8.translate(0f, 0f, -1.5f);

            // KAKI 2
            var cube3 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 0.5f, 0.5f));
            cube3.createCuboid_v2(1.5f, 0f, 0f, 0.1f, 1.5f, true);
            cube3.rotate(Vector3.Zero, Vector3.UnitX, -45);
            var cube4 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1.5f, 0.5f, 0.5f));
            cube4.createCuboid_v2(1.5f, 0f, 0.75f, 0.1f, 1.5f, true);
            cube4.rotate(Vector3.Zero, Vector3.UnitX, 45);

            var cube9 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 0.5f, 0.5f));
            cube9.createCuboid_v2(1.5f, 0f, 0f, 0.1f, 1.5f, true);
            cube9.rotate(Vector3.Zero, Vector3.UnitX, 45);
            cube9.translate(0f, 0f, -1f);
            var cube10 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1.5f, 0.5f, 0.5f));
            cube10.createCuboid_v2(1.5f, 0f, -0.75f, 0.1f, 1.5f, true);
            cube10.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube10.translate(0f, 0f, -1f);

            // KAKI 3
            var cube5 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 0.5f, 0.5f));
            cube5.createCuboid_v2(-1.5f, 0f, 0f, 0.1f, 1.5f, true);
            cube5.rotate(Vector3.Zero, Vector3.UnitX, -45);
            var cube6 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1.5f, 0.5f, 0.5f));
            cube6.createCuboid_v2(-1.5f, 0f, 0.75f, 0.1f, 1.5f, true);
            cube6.rotate(Vector3.Zero, Vector3.UnitX, 45);

            var cube11 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 0.5f, 0.5f));
            cube11.createCuboid_v2(-1.5f, 0f, 0f, 0.1f, 1.5f, true);
            cube11.rotate(Vector3.Zero, Vector3.UnitX, 45);
            cube11.translate(0f, 0f, -1f);
            var cube12 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1.5f, 0.5f, 0.5f));
            cube12.createCuboid_v2(-1.5f, 0f, -0.75f, 0.1f, 1.5f, true);
            cube12.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube12.translate(0f, 0f, -1f);

            // BADAN
            var ellips1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.6f, 0.4f, 0.2f));
            ellips1.createEllipsoid(0f, 0f, -0.5f, 1.75f, .5f, 1f, 50, 50, true);

            // KEPALA
            var ellips2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.39f, 0.26f, 0.12f));
            ellips2.createEllipsoid(1.75f, -0.15f, -0.5f, .15f, .5f, .25f, 50, 50, true);

            var wings = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.1f, 1f, 1f));
            wings.createHyper(-0.5f, 0f, 0f, 0.5f, 0.25f, 0.5f, 5, 100, true);
            wings.rotate(Vector3.Zero, Vector3.UnitY, -90);
            wings.rotate(Vector3.Zero, Vector3.UnitZ, -45);
            cockroach.child.Add(wings);

            var wings2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.1f, 1f, 1f));
            wings2.createHyper(-0.5f, 0f, 0f, 0.5f, 0.25f, 0.5f, 5, 100, true);
            wings2.rotate(Vector3.Zero, Vector3.UnitY, -90);
            wings2.rotate(Vector3.Zero, Vector3.UnitZ, -45);
            cockroach.child.Add(wings2);

            cockroach.child.Add(cube1);
            cockroach.child.Add(cube2);
            cockroach.child.Add(cube3);
            cockroach.child.Add(cube4);
            cockroach.child.Add(cube5);
            cockroach.child.Add(cube6);
            cockroach.child.Add(cube7);
            cockroach.child.Add(cube8);
            cockroach.child.Add(cube9);
            cockroach.child.Add(cube10);
            cockroach.child.Add(cube11);
            cockroach.child.Add(cube12);
            cockroach.child.Add(ellips1);
            cockroach.child.Add(ellips2);
            objectList.Add(cockroach);

            cockroach.rotate(Vector3.Zero, Vector3.UnitY, -75);
            #endregion

            #region Ant
            var ant = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(3, 1, 1));

            // KAKI 1 ANT
            var cube101 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Right Upper Leg
            cube101.createCuboid_v2(1.0f, 5.25f, -1.85f, 0.1f, 2f, true);
            cube101.rotate(Vector3.Zero, Vector3.UnitX, -75);
            cube101.translate(0f, 0f, 0.25f);
            var cube102 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Right Lower Leg
            cube102.createCuboid_v2(1.0f, 1.3f, -4.25f, 0.1f, 3.5f, true);
            cube102.rotate(Vector3.Zero, Vector3.UnitX, -25);
            cube102.translate(0f, 0f, 0f);

            var cube107 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Left Upper Leg
            cube107.createCuboid_v2(1.0f, -4.75f, 1.5f, 0.1f, 2f, true);
            cube107.rotate(Vector3.Zero, Vector3.UnitX, 75);
            cube107.translate(0f, 2.25f, -1.5f);
            var cube108 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f));
            cube108.createCuboid_v2(1.0f, -3.375f, -5.75f, 0.1f, 3.5f, true); // Left Lower Leg
            cube108.rotate(Vector3.Zero, Vector3.UnitX, 25);
            cube108.translate(0f, 0f, 0f);

            // KAKI 2 ANT
            var cube103 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Right Upper Leg
            cube103.createCuboid_v2(1.5f, 5.25f, -1.85f, 0.1f, 2f, true);
            cube103.rotate(Vector3.Zero, Vector3.UnitX, -75);
            cube103.translate(0f, 0f, 0.25f);

            var cube104 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Right Lower Leg
            cube104.createCuboid_v2(1.5f, 1.3f, -4.25f, 0.1f, 3.5f, true);
            cube104.rotate(Vector3.Zero, Vector3.UnitX, -25);
            cube104.translate(0f, 0f, 0f);


            var cube109 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Left Upper Leg
            cube109.createCuboid_v2(1.5f, -4.75f, 1.5f, 0.1f, 2f, true);
            cube109.rotate(Vector3.Zero, Vector3.UnitX, 75);
            cube109.translate(0f, 2.25f, -1.5f);

            var cube110 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f));
            cube110.createCuboid_v2(1.5f, -3.375f, -5.75f, 0.1f, 3.5f, true); // Left Lower Leg
            cube110.rotate(Vector3.Zero, Vector3.UnitX, 25);
            cube110.translate(0f, 0f, 0f);

            // KAKI 3 ANT
            var cube105 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Right Upper Leg
            cube105.createCuboid_v2(2.0f, 5.25f, -1.85f, 0.1f, 2f, true);
            cube105.rotate(Vector3.Zero, Vector3.UnitX, -75);
            cube105.translate(0f, 0f, 0.25f);

            var cube106 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Right Lower Leg
            cube106.createCuboid_v2(2.0f, 1.3f, -4.25f, 0.1f, 3.5f, true);
            cube106.rotate(Vector3.Zero, Vector3.UnitX, -25);
            cube106.translate(0f, 0f, 0f);


            var cube111 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Left Upper Leg
            cube111.createCuboid_v2(2.0f, -4.75f, 1.5f, 0.1f, 2f, true);
            cube111.rotate(Vector3.Zero, Vector3.UnitX, 75);
            cube111.translate(0f, 2.25f, -1.5f);

            var cube112 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f));
            cube112.createCuboid_v2(2.0f, -3.375f, -5.75f, 0.1f, 3.5f, true); // Left Lower Leg
            cube112.rotate(Vector3.Zero, Vector3.UnitX, 25);
            cube112.translate(0f, 0f, 0f);

            // BADAN ANT
            var ellips101 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(.64f, .16f, .16f));// Gaster
            ellips101.createEllipsoid(0f, -0.1f, -5.5f, .75f, .75f, .75f, 72, 24, true);

            var ellips102 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(.64f, .16f, .16f));// Gaster
            ellips102.createEllipsoid(-0.5f, -0.125f, -5.5f, .75f, .75f, .75f, 72, 24, true);

            var ellips103 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(.64f, .16f, .16f));// Gaster
            ellips103.createEllipsoid(-1.0f, -0.25f, -5.5f, .75f, .75f, .75f, 72, 24, true);

            var ellips104 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(.64f, .16f, .16f));// Thorax
            ellips104.createEllipsoid(1.5f, 0f, -5.5f, 1.25f, .5f, .5f, 72, 24, true);

            // Kepala ANT
            var ellips105 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(.64f, .16f, .16f));// Head
            ellips105.createEllipsoid(3.0f, 0f, -5.5f, .5f, .375f, .5f, 72, 24, true);

            var ellips106 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 0, 1));// Right Eye
            ellips106.createEllipsoid(3.25f, .25f, -5.25f, .125f, .125f, .125f, 72, 24, true);

            var ellips107 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 0, 1));// Left Eye
            ellips107.createEllipsoid(3.25f, .25f, -5.75f, .125f, .125f, .125f, 72, 24, true);

            // Stinger ANT
            var wings101 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.1f, 1f, 1f));
            wings101.createHyper(5.5f, -5.25f, -2.5f, 0.5f, 0.1f, 0.1f, 10, 10, true);
            wings101.rotate(Vector3.Zero, Vector3.UnitY, 90);
            wings101.rotate(Vector3.Zero, Vector3.UnitZ, -45);

            // Antena ANT
            var cube151 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Right Antenna
            cube151.createCuboid_v2(3.0f, -0.375f, -5.125f, 0.1f, 1.5f, true);
            cube151.rotate(Vector3.Zero, Vector3.UnitX, 15);

            var cube152 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.78f, 0.22f, 0.22f)); // Left Antenna
            cube152.createCuboid_v2(3.0f, 2.5f, -5.5f, 0.1f, 1.5f, true);
            cube152.rotate(Vector3.Zero, Vector3.UnitX, -15);

            var torus101 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.88f, 0.22f, 0.22f)); // Right Antenna Torus
            torus101.createTorus(3.0625f, -5.125f, 0.375f, 0.09f, -0.05f, 100, 100, true);
            torus101.rotate(Vector3.Zero, Vector3.UnitX, 105);

            var torus102 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.88f, 0.22f, 0.22f)); // Left Antenna Torus
            torus102.createTorus(3.0625f, 5.5f, 2.5f, 0.09f, -0.05f, 100, 100, true);
            torus102.rotate(Vector3.Zero, Vector3.UnitX, -105);

            var torus103 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.88f, 0.22f, 0.22f)); // Mouth
            torus103.createTorus_v2(3.5f, 0f, 5.65f, 0.1f, -0.05f, 100, 100, true);
            torus103.rotate(Vector3.Zero, Vector3.UnitX, -180);

            var torus104 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.88f, 0.22f, 0.22f)); // Mouth
            torus104.createTorus_v2(3.5f, 0f, -5.45f, 0.1f, -0.05f, 100, 100, true);

            ant.child.Add(cube101);
            ant.child.Add(cube102);
            ant.child.Add(cube103);
            ant.child.Add(cube104);
            ant.child.Add(cube105);
            ant.child.Add(cube106);
            ant.child.Add(cube107);
            ant.child.Add(cube108);
            ant.child.Add(cube109);
            ant.child.Add(cube110);
            ant.child.Add(cube111);
            ant.child.Add(cube112);
            ant.child.Add(ellips101);
            ant.child.Add(ellips102);
            ant.child.Add(ellips103);
            ant.child.Add(ellips104);
            ant.child.Add(ellips105);
            ant.child.Add(ellips106);
            ant.child.Add(ellips107);
            ant.child.Add(cube151);
            ant.child.Add(cube152);
            ant.child.Add(torus101);
            ant.child.Add(torus102);
            ant.child.Add(torus103);
            ant.child.Add(torus104);

            objectList.Add(ant);
            objectList[1].rotate(Vector3.Zero, Vector3.UnitY, 90);
            objectList[1].translate(2f, -9.5f, 20f);
            #endregion

            #region Ulat
            var ulat = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 1, 1));
            //badan
            var badan1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.5f, 0.1f));
            badan1.createEllipsoid(-5f, 2.0f, 1f, 1f, 1f, 1f, 30, 24, true);
            /*badan1.rotate(Vector3.Zero, Vector3.UnitX, -45);
            badan1.translate(0f, 0f, .5f);*/
            var badan2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.5f, 0.1f));
            badan2.createEllipsoid(-4f, 2.0f, 1f, 1f, 1f, 1f, 30, 24, true);
            /*badan2.rotate(Vector3.Zero, Vector3.UnitX, -45);
            badan2.translate(0f, 0f, .5f);*/
            var badan3 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.5f, 0.1f));
            badan3.createEllipsoid(-2.9f, 2.0f, 1f, 1f, 1f, 1f, 30, 24, true);

            var badan4 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 1.5f, 0.5f));
            badan4.createEllipsoid(-1.9f, 2.8f, 1f, 1f, 1f, 1f, 30, 24, true);
            /*badan y+2*/
            /*badan2.rotate(Vector3.Zero, Vector3.UnitX, -45);
            badan2.translate(0f, 0f, .5f);*/

            //kaca mata
            //mata gagang z-2
            var mata1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 1.5f, 1.5f));
            mata1.createTorus(-2f, 2f, -3f, 0.3f, 0.1f, 100, 100, true);
            mata1.rotate(Vector3.Zero, Vector3.UnitX, 90);
            /*   var mata2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 1.5f, 1.5f));
               mata2.createTorus(-3.7f, 2f, -3f, 0.3f, 0.1f, 100, 100);
               mata2.rotate(Vector3.Zero, Vector3.UnitX, 90);*/
            //
            var gagangpanjang1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.0f, 0.0f));
            gagangpanjang1.createCuboid_v2(-2.8f, 1.5f, -3f, 0.1f, 2f, true);
            gagangpanjang1.rotate(Vector3.Zero, Vector3.UnitX, 90);
            var gagangpanjang2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.0f, 0.0f));
            gagangpanjang2.createCuboid_v2(-1f, 1.5f, -3f, 0.1f, 2f, true);
            gagangpanjang2.rotate(Vector3.Zero, Vector3.UnitX, 90);
            //kecil y+2
            var gagangkecil1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.0f, 0.0f));
            gagangkecil1.createCuboid_v3(-2.6f, 3f, 1.8f, 0.2f, true);
            var gagangkecil2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.0f, 0.0f));
            gagangkecil2.createCuboid_v3(-1.2f, 3f, 1.8f, 0.2f, true);
            //gagangkecil1.rotate(Vector3.Zero, Vector3.UnitX,360);
            //kaki BELAKANG
            var kaki1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki1.createEllipsoid(-4.5f, 0.8f, -1f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki1.rotate(Vector3.Zero, Vector3.UnitX, 45);
            var kaki2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki2.createEllipsoid(-3.5f, 0.8f, -1f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki2.rotate(Vector3.Zero, Vector3.UnitX, 45);
            //KAKI DEPAN
            var kaki3 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki3.createEllipsoid(-4.5f, 2f, 0.5f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki3.rotate(Vector3.Zero, Vector3.UnitX, 40);
            var kaki4 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki4.createEllipsoid(-3.5f, 2f, 0.5f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki4.rotate(Vector3.Zero, Vector3.UnitX, 40);
            //TENGAH
            var kaki5 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki5.createEllipsoid(-4.5f, 1.5f, 0f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki5.rotate(Vector3.Zero, Vector3.UnitX, 35);
            var kaki6 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki6.createEllipsoid(-3.5f, 1.5f, 0f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki6.rotate(Vector3.Zero, Vector3.UnitX, 35);
            /*var kaki7 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki7.createEllipsoid(-4.5f, -0.2f, 1.5f, 0.2f, 0.5f, 0.2f, 10, 20);
            kaki7.rotate(Vector3.Zero, Vector3.UnitX, 35);
            var kaki8 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki8.createEllipsoid(-3.5f, -0.2f, 1.5f, 0.2f, 0.5f, 0.2f, 10, 20);
            kaki8.rotate(Vector3.Zero, Vector3.UnitX, 35);
            var kaki9 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki9.createEllipsoid(-4.5f, 0f, 2f, 0.2f, 0.5f, 0.2f, 10, 20);
            kaki9.rotate(Vector3.Zero, Vector3.UnitX, 35);
            var kaki10 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki10.createEllipsoid(-3.5f, 0f, 2f, 0.2f, 0.5f, 0.2f, 10, 20);
            kaki10.rotate(Vector3.Zero, Vector3.UnitX, 35);*/
            //mulut 
            var mulut = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.4f, 0.4f, 0.4f));
            mulut.createCone(-2f, -2f, 2.5f, 0.2f, 0f, 0.1f, 10, 20, true);
            mulut.rotate(Vector3.Zero, Vector3.UnitX, -90);

            ulat.child.Add(badan1);
            ulat.child.Add(badan2);
            ulat.child.Add(badan3);
            ulat.child.Add(badan4);
            ulat.child.Add(mata1);
            ulat.child.Add(gagangpanjang1);
            ulat.child.Add(gagangpanjang2);
            ulat.child.Add(gagangkecil1);
            ulat.child.Add(gagangkecil2);
            ulat.child.Add(mulut);
            ulat.child.Add(kaki1);
            ulat.child.Add(kaki2);
            ulat.child.Add(kaki5);
            ulat.child.Add(kaki6);
            objectList.Add(ulat);
            #endregion

            #region Daratan
            var land = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.196f, 0.721f, 0.023f));
            land.createCuboid_v3(0f, -28.5f, 0f, 50f, true);
            objectList.Add(land);
            #endregion

            #region Batu
            var rock = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 1, 1));

            var elips201 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.502f, 0.502f, 0.502f));
            elips201.createEllipsoid(1f, 0.1f, 1f, 0.3f, 0.1f, 0.3f, 72, 24, true);

            var elips202 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.502f, 0.502f, 0.502f));
            elips202.createEllipsoid(1f, 0f, 1.5f, 0.3f, 0.1f, 0.9f, 72, 24, true);

            var elips203 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.502f, 0.502f, 0.502f));
            elips203.createEllipsoid(1f, .25f, 1.6f, 0.2f, 0.35f, 0.7f, 72, 24, true);

            var elips204 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.502f, 0.502f, 0.502f));
            elips204.createEllipsoid(0.75f, .1f, 1.6f, 0.2f, 0.2f, 0.7f, 72, 24, true);

            var elips205 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.502f, 0.502f, 0.502f));
            elips205.createEllipsoid(0.85f, 0.1f, 2.25f, 0.1f, 0.1f, 0.1f, 72, 24, true);

            rock.child.Add(elips201);
            rock.child.Add(elips202);
            rock.child.Add(elips203);
            rock.child.Add(elips204);
            rock.child.Add(elips205);

            objectList.Add(rock);
            objectList[4].translate(5, -11.5f, 20);
            objectList[4].scale(10, 10, 10);
            objectList[4].rotate(Vector3.Zero, Vector3.UnitY, -90);
            #endregion

            #region Tree
            var tube101 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.588f, 0.294f, 0f));
            tube101.createCuboid_v4(1f, 1f, 1f, 1f, true);
            tube101.rotate(Vector3.Zero, Vector3.UnitX, 90);
            tube101.scale(1.25f, 2f, 1.25f);

            #region Bark Prime
            var tube102 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.588f, 0.294f, 0f));
            tube102.createCuboid_v3(-0.75f, 1f, 1f, 1f, true);
            tube102.scale(5f, 1f, 1f);
            #endregion

            #region Bark Secundus
            var tube105 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.588f, 0.294f, 0f));
            tube105.createCuboid_v3(1.25f, 6f, 1f, 1f, true);
            tube105.scale(5f, 1f, 1f);
            #endregion

            var leaf101 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf101.createEllipsoid(1f, 20f, 1f, 1f, 3.5f, 1f, 72, 24, true);
            leaf101.scale(3f, 3f, 3f);

            #region lower Leaf
            var leaf102 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf102.createEllipsoid(1f, 16.5f, 2f, 1f, 1f, 1f, 72, 24, true);
            leaf102.scale(3f, 3f, 3f);

            var leaf103 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf103.createEllipsoid(1f, 16.5f, 0.25f, 1f, 1, 1f, 72, 24, true);
            leaf103.scale(3f, 3f, 3f);

            var leaf104 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf104.createEllipsoid(1.75f, 16.5f, 1f, 1f, 1f, 1f, 72, 24, true);
            leaf104.scale(3f, 3f, 3f);

            var leaf105 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf105.createEllipsoid(0.25f, 16.5f, 1f, 1f, 1f, 1f, 72, 24, true);
            leaf105.scale(3f, 3f, 3f);
            #endregion

            #region lower Leaf+
            var leaf106 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf106.createEllipsoid(1f, 19.5f, 2.5f, 2f, 1f, 1.2f, 72, 24, true);
            leaf106.scale(3f, 3f, 3f);

            var leaf107 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf107.createEllipsoid(1f, 19.5f, -0.25f, 2f, 1f, 1.2f, 72, 24, true);
            leaf107.scale(3f, 3f, 3f);

            var leaf108 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf108.createEllipsoid(2.5f, 19.5f, 1f, 1f, 1f, 1.7f, 72, 24, true);
            leaf108.scale(3f, 3f, 3f);

            var leaf109 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf109.createEllipsoid(-0.5f, 19.5f, 1f, 1f, 1f, 1.7f, 72, 24, true);
            leaf109.scale(3f, 3f, 3f);
            #endregion

            #region Middle Leaf
            var leaf110 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf110.createEllipsoid(1f, 20.5f, 3f, 2f, 1f, 1.2f, 72, 24, true);
            leaf110.scale(3f, 3f, 3f);

            var leaf111 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf111.createEllipsoid(1f, 20.5f, -1f, 2f, 1f, 1.2f, 72, 24, true);
            leaf111.scale(3f, 3f, 3f);

            var leaf112 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf112.createEllipsoid(3.25f, 20.5f, 1f, 1f, 1f, 1.7f, 72, 24, true);
            leaf112.scale(3f, 3f, 3f);

            var leaf113 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf113.createEllipsoid(-1.25f, 20.5f, 1f, 1f, 1f, 1.7f, 72, 24, true);
            leaf113.scale(3f, 3f, 3f);
            #endregion

            #region Upper Leaf
            var leaf114 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf114.createEllipsoid(1f, 21f, 2.5f, 2f, 1f, 1.2f, 72, 24, true);
            leaf114.scale(3f, 3f, 3f);

            var leaf115 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf115.createEllipsoid(1f, 21f, -0.25f, 2f, 1f, 1.2f, 72, 24, true);
            leaf115.scale(3f, 3f, 3f);

            var leaf116 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf116.createEllipsoid(2.5f, 21f, 1f, 1f, 1f, 1.7f, 72, 24, true);
            leaf116.scale(3f, 3f, 3f);

            var leaf117 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf117.createEllipsoid(-0.5f, 21f, 1f, 1f, 1f, 1.7f, 72, 24, true);
            leaf117.scale(3f, 3f, 3f);
            #endregion

            var leaf118 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf118.createEllipsoid(1f, 23.75f, 2f, 1f, 1f, 1f, 72, 24, true);
            leaf118.scale(3f, 3f, 3f);

            var leaf119 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf119.createEllipsoid(1f, 23.75f, 0.25f, 1f, 1, 1f, 72, 24, true);
            leaf119.scale(3f, 3f, 3f);

            var leaf120 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf120.createEllipsoid(1.75f, 23.75f, 1f, 1f, 1f, 1f, 72, 24, true);
            leaf120.scale(3f, 3f, 3f);

            var leaf121 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 1f, 0f));
            leaf121.createEllipsoid(0.25f, 23.75f, 1f, 1f, 1f, 1f, 72, 24, true);
            leaf121.scale(3f, 3f, 3f);

            objectList.Add(tube101);

            objectList.Add(tube102);

            objectList.Add(tube105); ;

            objectList.Add(leaf101);

            objectList.Add(leaf102);
            objectList.Add(leaf103);
            objectList.Add(leaf104);
            objectList.Add(leaf105);

            objectList.Add(leaf106);
            objectList.Add(leaf107);
            objectList.Add(leaf108);
            objectList.Add(leaf109);

            objectList.Add(leaf110);
            objectList.Add(leaf111);
            objectList.Add(leaf112);
            objectList.Add(leaf113);

            objectList.Add(leaf114);
            objectList.Add(leaf115);
            objectList.Add(leaf116);
            objectList.Add(leaf117);

            objectList.Add(leaf118);
            objectList.Add(leaf119);
            objectList.Add(leaf120);
            objectList.Add(leaf121);
            #endregion

            objectList[0].translate(-5f, 3f, -10f);

            // MATAHARI
            light = new Asset3d("shader.vert", "shader.frag", new Vector3(1, 1, 1));
            light.createCuboid(10, 35, -10, 5f, false, false);

            // DARATAN
            /*var land = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.196f, 0.721f, 0.023f));
            land.createCuboid_v3(0f, -20f, 0f, 50f, true);
            objectList.Add(land);*/

            // THIRD PERSON
            var temp = new Asset3d("", "", new Vector3(1, 1, 1));
            bug1 = temp.createCockroach(-11f, -1.5f, .5f);
            bug1.child[0].scaleNew(0.1f, 0.1f, 0.1f, bug1.child[0].objectCenter);
            bug1.child[1].scaleNew(0.1f, 0.1f, 0.1f, bug1.child[1].objectCenter);
            bug1.rotate(Vector3.Zero, Vector3.UnitY, 90);

            // TIANG LAMPU
            var lightPole1 = temp.createLightPole(15, 0, 0);
            objectList.Add(lightPole1);

            var lightPole2 = temp.createLightPole(-15, 0, 0);
            objectList.Add(lightPole2);

            var lightPole3 = temp.createLightPole(-15, 0, -15);
            objectList.Add(lightPole3);

            var lightPole4 = temp.createLightPole(15, 0, -15);
            objectList.Add(lightPole4);

            // LAMPU (lebih dari 1)
            var pointLight1 = new Asset3d("shader.vert", "shader.frag", new Vector3(1, 1, 1));
            pointLight1.createEllipsoid(lightPole1.objectCenter.X + 15, lightPole1.objectCenter.Y, lightPole1.objectCenter.Z, 2.25f, 2.25f, 2.25f, 30, 30, false);
            pointLights.Add(pointLight1);

            var pointLight2 = new Asset3d("shader.vert", "shader.frag", new Vector3(1, 1, 1));
            pointLight2.createEllipsoid(lightPole2.objectCenter.X - 15, lightPole2.objectCenter.Y, lightPole2.objectCenter.Z, 2.25f, 2.25f, 2.25f, 30, 30, false);
            pointLights.Add(pointLight2);

            var pointLight3 = new Asset3d("shader.vert", "shader.frag", new Vector3(1, 1, 1));
            pointLight3.createEllipsoid(lightPole3.objectCenter.X - 15, lightPole3.objectCenter.Y, lightPole3.objectCenter.Z - 15, 2.25f, 2.25f, 2.25f, 30, 30, false);
            pointLights.Add(pointLight3);

            var pointLight4 = new Asset3d("shader.vert", "shader.frag", new Vector3(1, 1, 1));
            pointLight4.createEllipsoid(lightPole4.objectCenter.X + 15, lightPole4.objectCenter.Y, lightPole4.objectCenter.Z - 15, 2.25f, 2.25f, 2.25f, 30, 30, false);
            pointLights.Add(pointLight4);

            light.load();

            bug1.load();

            #region testObjects
            /*var cube1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 0.5f, 0.25f));
            cube1.createCuboid(0, 0, 0, 5, true, false);
            cube1.rotate(Vector3.Zero, Vector3.UnitX, 45);
            cube1.resetEuler();
            cube1.rotate(Vector3.Zero, Vector3.UnitZ, MathHelper.RadiansToDegrees(MathF.Acos(MathF.Sqrt(2) / MathF.Sqrt(3))));
            cube1.resetEuler();
            objectList.Add(cube1);*/

            /*var cube2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 0.5f, 0.25f));
            cube2.createCuboid(-10, 0, 0, 5, true, false);
            objectList.Add(cube2);
            Console.WriteLine("cube's position min: " + cube2.posMin);
            Console.WriteLine("cube's position max: " + cube2.posMax);*/

            /*var cube3 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 0.5f, 0.25f));
            cube3.createCuboid(10, 0, 0, 5, true, false);
            objectList.Add(cube3);*/

            /*var ellips1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 0.5f, 0.25f));
            ellips1.createEllipsoid(0, 0, -3, 5, 3, 5, 15, 15,true);
            objectList.Add(ellips1);*/

            /*var cube_v2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, .5f, .25f));
            cube_v2.createCuboid_v2(0, 0, -3, 3, 3, true);
            objectList.Add(cube_v2);*/

            /*var cube_v3 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, .5f, .25f));
            cube_v3.createCuboid_v3(0, 0, -3, 5, true);
            objectList.Add(cube_v3);*/

            /*var wings = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.1f, 1f, 1f));
            wings.createHyper(-0.5f, 0f, 0f, 0.5f, 0.25f, 0.5f, 5, 100, true);
            wings.rotate(Vector3.Zero, Vector3.UnitY, -90);
            wings.rotate(Vector3.Zero, Vector3.UnitZ, -45);
            objectList.Add(wings);*/

            /*var torus1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, .5f, .25f));
            torus1.createTorus(0, 0, -3, 5, 2, 15, 15, true);
            objectList.Add(torus1);*/

            /*var torus_v2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, .5f, .25f));
            torus_v2.createTorus_v2(0, 0, -3, 5, 2, 15, 15, true);
            objectList.Add(torus_v2);*/
            #endregion

            foreach (Asset3d i in objectList)
            {
                i.load();
            }

            foreach (Asset3d i in pointLights)
            {
                i.load();
            }

            Console.WriteLine(camera.Position);

            /*CursorGrabbed = true;*/
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            float time = (float)args.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            light.render(renderSetting, camera.GetViewMatrix(), camera.GetProjectionMatrix(), camera.Position, pointLights);

            bug1.render(renderSetting, camera.GetViewMatrix(), camera.GetProjectionMatrix(), camera.Position, pointLights);

            foreach (Asset3d i in objectList)
            {
                i.render(renderSetting, camera.GetViewMatrix(), camera.GetProjectionMatrix(), camera.Position, pointLights);
                /*i.rotate(i.objectCenter, i._euler[1], 45 * time);*/

                i.setFragVar(camera.Position);
                i.setDirectionalLight(new Vector3(-0.2f, -1.0f, -0.3f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.5f, 0.5f, 0.5f));
                i.setPointLight(pointLights, new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
            }

            foreach(Asset3d i in pointLights)
            {
                i.render(renderSetting, camera.GetViewMatrix(), camera.GetProjectionMatrix(), camera.Position, pointLights);
            }

            if (a >= 0 && a <= 500)
            {
                if (a <= 250)
                {
                    objectList[0].child[0].rotate(objectList[0].child[0].objectCenter, Vector3.UnitZ, -100 * time);
                    objectList[0].child[1].rotate(objectList[0].child[1].objectCenter, Vector3.UnitZ, 100 * time);
                    //ulat

                    // ANT
                    objectList[1].child[23].rotate(objectList[1].child[23].objectCenter, Vector3.UnitY, -100 * time);
                    objectList[1].child[24].rotate(objectList[1].child[24].objectCenter, Vector3.UnitY, 100 * time);
                    if (b > 0 + delay && b < 50 + delay)
                    {
                        objectList[0].translate(0, 0, 0.005f);

                    }
                    if (b > 5 + delay && b <= 21 + delay)
                    {
                        objectList[2].translate(0, -0.0025f, 0);
                    }
                    if (b >= 21 + delay && b < 58 + delay)
                    {
                        objectList[1].translate(0.0005f, 0, -0.001f);
                    }
                    if ((b >= 58 + delay && b < 59 + delay))
                    {
                        objectList[1].rotate(Vector3.Zero, Vector3.UnitY, -90);
                        objectList[1].translate(-6, 0, 5);
                        objectList[2].translate(0, 0.0125f, 0);
                    }
                    if ((b >= 59 + delay && b < 150 + delay))
                    {
                        objectList[1].translate(-0.001f, 0.001f, 0.0005f);
                        objectList[2].translate(-0.001f, -0.001f, 0.0005f);
                    }
                }
                else
                {
                    objectList[0].child[0].rotate(objectList[0].child[0].objectCenter, Vector3.UnitZ, 100 * time);
                    objectList[0].child[1].rotate(objectList[0].child[1].objectCenter, Vector3.UnitZ, -100 * time);
                    //ulat

                    // ANT
                    objectList[1].child[23].rotate(objectList[1].child[23].objectCenter, Vector3.UnitY, 100 * time);
                    objectList[1].child[24].rotate(objectList[1].child[24].objectCenter, Vector3.UnitY, -100 * time);
                    if (b > 0 + delay && b < 50 + delay)
                    {
                        objectList[0].translate(0, 0, 0.0025f);
                    }
                    if (b > 5 + delay && b <= 21 + delay)
                    {
                        objectList[2].translate(0, -0.0005f, 0);
                    }
                    if (b >= 21 + delay && b < 58 + delay)
                    {
                        objectList[1].translate(-0.0005f, 0, -0.001f);
                    }
                    if ((b >= 59 + delay && b < 150 + delay))
                    {
                        objectList[1].translate(-0.001f, -0.001f, -0.0005f);
                        objectList[2].translate(-0.001f, 0.001f, -0.0005f);
                    }
                }
            }
            a += 1;

            if (a > 500)
            {
                a = 0;
                b += 1;
            }
            if (b > 10)
            {
                c += 1;
            }
            if (b == 6 + delay)
            {
                objectList[2].rotate(objectList[2].objectCenter, Vector3.UnitX, 150);
            }

            GL.Disable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Lequal);
            
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);

            totalTime += time;

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            float time = (float)args.Time;

            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyReleased(Keys.Q))
            {
                Console.WriteLine(camera.Position);
            }

            #region cameraMovements
            if (input.IsKeyDown(Keys.W)) // forward
            {
                if (!isOverlaps() && camera.Position.Z > -28.25f)
                {
                    camera.Position += Vector3.Normalize(Vector3.Cross(camera.Up, camera.Right)) * cameraSpeed * time;
                    bug1.translate(0, 0, -cameraSpeed * (float)args.Time);
                }
                else
                {
                    camera.Position -= Vector3.Normalize(Vector3.Cross(camera.Up, camera.Right)) * cameraSpeed * .01f;
                    bug1.translate(0, 0, cameraSpeed * (float).01f);
                }
                
            }

            if (input.IsKeyDown(Keys.S)) // backward
            {
                if (!isOverlaps() && camera.Position.Z < 28.25f)
                {
                    camera.Position -= Vector3.Normalize(Vector3.Cross(camera.Up, camera.Right)) * cameraSpeed * time;
                    bug1.translate(0, 0, cameraSpeed * (float)args.Time);
                }
                else
                {
                    camera.Position += Vector3.Normalize(Vector3.Cross(camera.Up, camera.Right)) * cameraSpeed * .01f;
                    bug1.translate(0, 0, -cameraSpeed * (float).01f);
                }
            }
            if (input.IsKeyDown(Keys.A)) // left
            {
                if (!isOverlaps() && camera.Position.X > -48)
                {
                    camera.Position -= camera.Right * cameraSpeed * time;
                    bug1.translate(-cameraSpeed * (float)args.Time, 0f, 0f);
                }
                else
                {
                    camera.Position += camera.Right * cameraSpeed * .01f;
                    bug1.translate(cameraSpeed * (float).01f, 0f, 0f);
                }
            }

            if (input.IsKeyDown(Keys.D)) // right
            {
                if (!isOverlaps() && camera.Position.X < 48)
                {
                    camera.Position += camera.Right * cameraSpeed * time;
                    bug1.translate(cameraSpeed * (float)args.Time, 0f, 0f);
                }
                else
                {
                    camera.Position -= camera.Right * cameraSpeed * .01f;
                    bug1.translate(-cameraSpeed * (float).01f, 0f, 0f);
                }
            }

            if (input.IsKeyDown(Keys.Space)) // up
            {
                if (!isOverlaps() && camera.Position.Y < 35)
                {
                    camera.Position += camera.Up * cameraSpeed * time;
                    bug1.translate(0f, cameraSpeed * (float)args.Time, 0f);
                }
                else
                {
                    camera.Position -= camera.Up * cameraSpeed * .01f;
                    bug1.translate(0f, -cameraSpeed * (float).01f, 0f);
                }
            }

            if (input.IsKeyDown(Keys.Z)) // down
            {
                if (!isOverlaps() && camera.Position.Y > -9.25f)
                {
                    camera.Position -= camera.Up * cameraSpeed * time;
                    bug1.translate(0f, -cameraSpeed * (float)args.Time, 0f);
                }
                else
                {
                    camera.Position += camera.Up * cameraSpeed * .01f;
                    bug1.translate(0f, cameraSpeed * (float).01f, 0f);
                }
            }
            #endregion

            if (input.IsKeyPressed(Keys.LeftControl))
            {
                cameraSpeed += 5;
                camera.Fov += 10;
            }

            if (input.IsKeyReleased(Keys.LeftControl))
            {
                cameraSpeed -= 5;
                camera.Fov -= 10;
            }

            if (input.IsKeyPressed(Keys.GraveAccent))
            {
                renderSetting *= -1;
            }

            /*var mouse = MouseState;

            if (firstMove)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity;
            }*/
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);

            camera.AspectRatio = Size.X / (float)Size.Y;
        }

        public bool isOverlaps()
        {
            foreach (Asset3d i in objectList)
            {
                var cubeB_max = i.posMax;
                var cubeB_min = i.posMin;
                
                if (camera.Position.X >= cubeB_min.X - (cubeB_min.X / .5f) && camera.Position.X <= cubeB_max.X + (cubeB_min.X / 7.5f)) 
                {
                    if (camera.Position.Y >= cubeB_min.Y - (cubeB_min.Y / .5f) && camera.Position.Y <= cubeB_max.Y + (cubeB_min.Y / .75f))
                    {
                        if (camera.Position.Z <= cubeB_min.Z + 11.75f && camera.Position.Z >= cubeB_max.Z - 11.75f)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
