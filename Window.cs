using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using LearnOpenTK.Common;
using System.Drawing;

namespace objectFinal
{
    static class Constants
    {
        public const string path = "../../../Shaders/";
    }
    class Window : GameWindow
    {
        List<Asset3d> objectList = new List<Asset3d>();

        Camera _camera;

        bool _firstMove = true;
        Vector2 _lastPos;
        Vector3 _objectPos = new Vector3(0, 0, 0);
        float _rotationSpeed = 1f;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // background color
            GL.ClearColor(0.00f, 0.03f, 0.20f, 0);
            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(new Vector3(0, 0, 5), Size.X / Size.Y);

            var temp = new Asset3d(new Vector3(1,1,1));

            var land = new Asset3d(new Vector3(0.196f, 0.721f, 0.023f));
            land.createCuboid_v3(0f, -4f, 0f, 10f);
            objectList.Add(land);

            #region Kecoak
            /*var cockroach = new Asset3d(new Vector3(1, 1, 1));

            var angin = new Asset3d(new Vector3(0, 0, 0));
            angin.prepareVertices();
            angin.setControlCoordinate(-2.4f, -0.2f, -0.65f);
            angin.setControlCoordinate(-2.7f, -0.3f, -0.55f);
            angin.setControlCoordinate(-2.73f, -0.255f, -0.4f);
            angin.setControlCoordinate(-2.78f, -0.35f, -0.4f);
            angin.setControlCoordinate(-2.80f, -0.28f, -0.3f);
            angin.setControlCoordinate(-3.98f, -0.24f, -0.175f);
            List<Vector3> angin_bezier = angin.createCurveBazier();
            angin.setVertices(angin_bezier);
            angin.translate(4.2f, 0.25f, -2.2f);
            cockroach.child.Add(angin);

            // KAKI 1
            var cube1 = new Asset3d(new Vector3(0.5f, 0.5f, 0.5f));
            cube1.createCuboid_v2(0f, 0f, 0f, 0.1f, 1.5f);
            cube1.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube1.translate(0f, 0f, .5f);
            var cube2 = new Asset3d(new Vector3(1.5f, 0.5f, 0.5f));
            cube2.createCuboid_v2(0f, 0f, 0.75f, 0.1f, 1.5f);
            cube2.rotate(Vector3.Zero, Vector3.UnitX, 45);
            cube2.translate(0f, 0f, .5f);

            var cube7 = new Asset3d(new Vector3(0.5f, 0.5f, 0.5f));
            cube7.createCuboid_v2(0f, 0f, 0f, 0.1f, 1.5f);
            cube7.rotate(Vector3.Zero, Vector3.UnitX, 45);
            cube7.translate(0f, 0f, -1.5f);
            var cube8 = new Asset3d(new Vector3(1.5f, 0.5f, 0.5f));
            cube8.createCuboid_v2(0f, 0f, -0.75f, 0.1f, 1.5f);
            cube8.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube8.translate(0f, 0f, -1.5f);

            // KAKI 2
            var cube3 = new Asset3d(new Vector3(0.5f, 0.5f, 0.5f));
            cube3.createCuboid_v2(1.5f, 0f, 0f, 0.1f, 1.5f);
            cube3.rotate(Vector3.Zero, Vector3.UnitX, -45);
            var cube4 = new Asset3d(new Vector3(1.5f, 0.5f, 0.5f));
            cube4.createCuboid_v2(1.5f, 0f, 0.75f, 0.1f, 1.5f);
            cube4.rotate(Vector3.Zero, Vector3.UnitX, 45);

            var cube9 = new Asset3d(new Vector3(0.5f, 0.5f, 0.5f));
            cube9.createCuboid_v2(1.5f, 0f, 0f, 0.1f, 1.5f);
            cube9.rotate(Vector3.Zero, Vector3.UnitX, 45);
            cube9.translate(0f, 0f, -1f);
            var cube10 = new Asset3d(new Vector3(1.5f, 0.5f, 0.5f));
            cube10.createCuboid_v2(1.5f, 0f, -0.75f, 0.1f, 1.5f);
            cube10.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube10.translate(0f, 0f, -1f);

            // KAKI 3
            var cube5 = new Asset3d(new Vector3(0.5f, 0.5f, 0.5f));
            cube5.createCuboid_v2(-1.5f, 0f, 0f, 0.1f, 1.5f);
            cube5.rotate(Vector3.Zero, Vector3.UnitX, -45);
            var cube6 = new Asset3d(new Vector3(1.5f, 0.5f, 0.5f));
            cube6.createCuboid_v2(-1.5f, 0f, 0.75f, 0.1f, 1.5f);
            cube6.rotate(Vector3.Zero, Vector3.UnitX, 45);

            var cube11 = new Asset3d(new Vector3(0.5f, 0.5f, 0.5f));
            cube11.createCuboid_v2(-1.5f, 0f, 0f, 0.1f, 1.5f);
            cube11.rotate(Vector3.Zero, Vector3.UnitX, 45);
            cube11.translate(0f, 0f, -1f);
            var cube12 = new Asset3d(new Vector3(1.5f, 0.5f, 0.5f));
            cube12.createCuboid_v2(-1.5f, 0f, -0.75f, 0.1f, 1.5f);
            cube12.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube12.translate(0f, 0f, -1f);

            // BADAN
            var ellips1 = new Asset3d(new Vector3(0.6f, 0.4f, 0.2f));
            ellips1.createEllipsoid(0f, 0f, -0.5f, 1.75f, .5f, 1f, 50, 50);

            // KEPALA
            var ellips2 = new Asset3d(new Vector3(0.39f, 0.26f, 0.12f));
            ellips2.createEllipsoid(1.75f, -0.15f, -0.5f, .15f, .5f, .25f, 50, 50);

            var wings = new Asset3d(new Vector3(0.1f, 1f, 1f));
            wings.createHyper(-0.5f, 0f, 0f, 0.5f, 0.25f, 0.5f, 5, 100);
            wings.rotate(Vector3.Zero, Vector3.UnitY, -90);
            wings.rotate(Vector3.Zero, Vector3.UnitZ, -45);
            cockroach.child.Add(wings);

            var wings2 = new Asset3d(new Vector3(0.1f, 1f, 1f));
            wings2.createHyper(-0.5f, 0f, 0f, 0.5f, 0.25f, 0.5f, 5, 100);
            wings2.rotate(Vector3.Zero, Vector3.UnitY, -90);
            wings2.rotate(Vector3.Zero, Vector3.UnitZ, -45);
            cockroach.child.Add(wings2);

            var angin2 = new Asset3d(new Vector3(0, 0, 0));
            angin2.prepareVertices();
            angin2.setControlCoordinate(-2.4f, -0.2f, -0.65f);
            angin2.setControlCoordinate(-2.7f, -0.3f, -0.55f);
            angin2.setControlCoordinate(-2.73f, -0.255f, -0.4f);
            angin2.setControlCoordinate(-2.78f, -0.35f, -0.4f);
            angin2.setControlCoordinate(-2.80f, -0.28f, -0.3f);
            angin2.setControlCoordinate(-3.98f, -0.24f, -0.175f);
            List<Vector3> angin_bezier2 = angin2.createCurveBazier();
            angin2.setVertices(angin_bezier2);
            angin2.translate(4.2f, 0.25f, 2.2f);
            cockroach.child.Add(angin2);

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

            cockroach.rotate(Vector3.Zero, Vector3.UnitY, -75);*/
            #endregion

            #region Twig1
            var twig1 = new Asset3d(new Vector3(1, 1, 1));

            var twig_body = new Asset3d(new Vector3(0.75f, 0.6f, 0.42f));
            twig_body.createCuboid_v2(0f, .1f, 2.5f, .05f, 1.5f);
            twig1.child.Add(twig_body);

            var twig_branch1 = new Asset3d(new Vector3(0.75f, 0.6f, 0.42f));
            twig_branch1.createCuboid_v2(0f, .1f, 2.5f, .05f, .25f);
            twig_branch1.rotate(Vector3.Zero, Vector3.UnitZ, -37.5f);
            twig_branch1.translate(.025f, -.1f, 0f);
            twig1.child.Add(twig_branch1);

            objectList.Add(twig1);
            #endregion

            #region LightPole
            var lightPole = new Asset3d(new Vector3(1,1,1));

            var pole2 = new Asset3d(new Vector3(0.054901960784313725f, 0.054901960784313725f, 0.054901960784313725f));
            pole2.createCuboid_v2(0f, -1.5f, 2.5f, .25f, 1.5f);
            lightPole.child.Add(pole2);

            var pole1 = new Asset3d(new Vector3(0.12549019607843137f, 0.12549019607843137f, 0.12549019607843137f));
            pole1.createCuboid_v2(0f, .1f, 2.5f, .15f, 3f);
            lightPole.child.Add(pole1);

            var lampPlace = new Asset3d(new Vector3(0f, 0f, 0f));
            lampPlace.createEllipsoid(0f, 0.15f, 2.5f, .15f, .075f, .15f, 50, 50);
            lightPole.child.Add(lampPlace);

            var lamp = new Asset3d(new Vector3(1f, 1f, 1f));
            lamp.createEllipsoid(0f, 0.45f, 2.5f, .25f, .25f, .25f, 50, 50);
            lightPole.child.Add(lamp);

            lightPole.translate(0, 1.7f, 0);
            objectList.Add(lightPole);
            #endregion

            #region Cloud
            var awan = temp.createCloud(0,0,0);
            var awan2 = temp.createCloud(3, 0, 1);
            var awan3 = temp.createCloud(0, 0, 3);

            objectList.Add(awan);
            objectList.Add(awan2);
            objectList.Add(awan3);
            #endregion

            #region Rock
            var rock = new Asset3d(new Vector3(1, 1, 1));

            var elips201 = new Asset3d(new Vector3(0.502f, 0.502f, 0.502f));
            elips201.createEllipsoid(1f, 0.1f, 1f, 0.3f, 0.1f, 0.3f, 72, 24);

            var elips202 = new Asset3d(new Vector3(0.502f, 0.502f, 0.502f));
            elips202.createEllipsoid(1f, 0f, 1.5f, 0.3f, 0.1f, 0.9f, 72, 24);

            var elips203 = new Asset3d(new Vector3(0.502f, 0.502f, 0.502f));
            elips203.createEllipsoid(1f, .25f, 1.6f, 0.2f, 0.35f, 0.7f, 72, 24);

            var elips204 = new Asset3d(new Vector3(0.502f, 0.502f, 0.502f));
            elips204.createEllipsoid(0.75f, .1f, 1.6f, 0.2f, 0.2f, 0.7f, 72, 24);

            var elips205 = new Asset3d(new Vector3(0.502f, 0.502f, 0.502f));
            elips205.createEllipsoid(0.85f, 0.1f, 2.25f, 0.1f, 0.1f, 0.1f, 72, 24);

            rock.child.Add(elips201);
            rock.child.Add(elips202);
            rock.child.Add(elips203);
            rock.child.Add(elips204);
            rock.child.Add(elips205);

            rock.translate(0, -.6f, 0);
            objectList.Add(rock);

            #endregion



            foreach (Asset3d i in objectList)
            {
                i.load(Size.X, Size.Y);
            }
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // DepthBufferBit juga harus di clear karena kita memakai depth testing.

            foreach (Asset3d i in objectList)
            {
                i.render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            }

            SwapBuffers();
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov = _camera.Fov - e.OffsetY;
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            float time = (float)args.Time; //Deltatime ==> waktu antara frame sebelumnya ke frame berikutnya, gunakan untuk animasi

            if (!IsFocused)
            {
                return; //Reject semua input saat window bukan focus.
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            float cameraSpeed = 7.5f;

            #region CameraRotation
            if (KeyboardState.IsKeyDown(Keys.X)) // right
            {
                _camera.Yaw += cameraSpeed * (float)args.Time * 10;
            }
            if (KeyboardState.IsKeyDown(Keys.C)) // left
            {
                _camera.Yaw -= cameraSpeed * (float)args.Time * 10;
            }
            if (KeyboardState.IsKeyDown(Keys.V)) // down
            {
                _camera.Pitch -= cameraSpeed * (float)args.Time * 10;
            }
            if (KeyboardState.IsKeyDown(Keys.B)) // up
            {
                _camera.Pitch += cameraSpeed * (float)args.Time * 10;
            }
            #endregion

            #region CameraMovement
            if (KeyboardState.IsKeyDown(Keys.W)) // forward
            {
                /*objectList[1].translate(0f, 0f, -cameraSpeed * (float)args.Time);*/
                _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.S)) // backward
            {
                /*objectList[1].translate(0f, 0f, cameraSpeed * (float)args.Time);*/
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.A)) // left
            {
                /*objectList[1].translate(-cameraSpeed * (float)args.Time, 0f, 0f);*/
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.D)) // right
            {
                /*objectList[1].translate(cameraSpeed * (float)args.Time, 0f, 0f);*/
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.Space)) // down
            {
                /*objectList[1].translate(0f, -cameraSpeed * (float)args.Time, 0f);*/
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.Z)) // up
            {
                /*objectList[1].translate(0f, cameraSpeed * (float)args.Time, 0f);*/
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
            }
            #endregion

            /*var mouse = MouseState;
            var sensitivity = .1f;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }*/

            if (KeyboardState.IsKeyDown(Keys.N))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objectPos;
                _camera.Position = Vector3.Transform(
                    _camera.Position,
                    generateArbRotationMatrix(axis, _objectPos,
                    _rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;
                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
            }

            if (KeyboardState.IsKeyDown(Keys.Comma))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objectPos;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, -_rotationSpeed)
                    .ExtractRotation());
                _camera.Position += _objectPos;
                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
            }
        }
        public Matrix4 generateArbRotationMatrix(Vector3 axis, Vector3 center, float degree)
        {
            var rads = MathHelper.DegreesToRadians(degree);

            var secretFormula = new float[4, 4] {
                { (float)Math.Cos(rads) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(rads)), axis.X* axis.Y * (1 - (float)Math.Cos(rads)) - axis.Z * (float)Math.Sin(rads),    axis.X * axis.Z * (1 - (float)Math.Cos(rads)) + axis.Y * (float)Math.Sin(rads),   0 },
                { axis.Y * axis.X * (1 - (float)Math.Cos(rads)) + axis.Z * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(rads)), axis.Y * axis.Z * (1 - (float)Math.Cos(rads)) - axis.X * (float)Math.Sin(rads),   0 },
                { axis.Z * axis.X * (1 - (float)Math.Cos(rads)) - axis.Y * (float)Math.Sin(rads),   axis.Z * axis.Y * (1 - (float)Math.Cos(rads)) + axis.X * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(rads)), 0 },
                { 0, 0, 0, 1}
            };
            var secretFormulaMatix = new Matrix4
            (
                new Vector4(secretFormula[0, 0], secretFormula[0, 1], secretFormula[0, 2], secretFormula[0, 3]),
                new Vector4(secretFormula[1, 0], secretFormula[1, 1], secretFormula[1, 2], secretFormula[1, 3]),
                new Vector4(secretFormula[2, 0], secretFormula[2, 1], secretFormula[2, 2], secretFormula[2, 3]),
                new Vector4(secretFormula[3, 0], secretFormula[3, 1], secretFormula[3, 2], secretFormula[3, 3])
            );

            return secretFormulaMatix;
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            _camera.AspectRatio = Size.X / (float)Size.Y;

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                float _x = (MousePosition.X - Size.X / 2) / (Size.X / 2);
                float _y = -(MousePosition.Y - Size.X / 2) / (Size.Y / 2);

                Console.WriteLine("x = " + _x + "; y = " + _y + ";");
            }
        }
    }
}
