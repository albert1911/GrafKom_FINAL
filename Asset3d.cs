using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace ConsoleApp3
{
    class Asset3d
    {
        private readonly string path = "../../../";

        private List<Vector3> vertices = new List<Vector3>();
        private List<uint> indices = new List<uint>();
        private List<Vector3> normals = new List<Vector3>();
        private List<Vector3> texCoords = new List<Vector3>();

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        private Shader _shader;
        private Texture _texture;

        private Matrix4 model = Matrix4.Identity;
        private Matrix4 normalMat = Matrix4.Identity;

        public Vector4 color;

        private string vertName;
        private string fragName;

        public List<Vector3> _euler = new List<Vector3>();
        public Vector3 objectCenter = Vector3.Zero;

        public Vector3 posMin = new Vector3(0, 0, 0);
        public Vector3 posMax = new Vector3(0, 0, 0);

        public List<Asset3d> child = new List<Asset3d>();

        public Asset3d(string vertName, string fragName, Vector3 color, float alpha = 1)
        {
            this.color = new Vector4(color, alpha);
            this.vertName = vertName;
            this.fragName = fragName;
            _euler.Add(Vector3.UnitX);
            _euler.Add(Vector3.UnitY);
            _euler.Add(Vector3.UnitZ);
        }

        public void load()
        {
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            if (texCoords.Count == 0 && normals.Count == 0)
            {
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);
            }
            else if (texCoords.Count > 0)
            {
                var combinedData = new List<Vector3>();
                for (int i = 0; i < vertices.Count; i++)
                {
                    combinedData.Add(vertices[i]);
                    combinedData.Add(texCoords[i]);
                }

                GL.BufferData(BufferTarget.ArrayBuffer, combinedData.Count * Vector3.SizeInBytes, combinedData.ToArray(), BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);
            }
            else if (normals.Count > 0)
            {
                var combinedData = new List<Vector3>();
                for (int i = 0; i < vertices.Count; i++)
                {
                    combinedData.Add(vertices[i]);
                    combinedData.Add(normals[i]);
                }

                GL.BufferData(BufferTarget.ArrayBuffer, combinedData.Count * Vector3.SizeInBytes, combinedData.ToArray(), BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);
            }

            if (indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
            }

            _shader = new Shader(path + "Shaders/" + vertName, path + "Shaders/" + fragName);
            _shader.Use();

            if (texCoords.Count > 0)
            {
                _texture = Texture.LoadFromFile(path + "textures/top.png");
                _texture.Use(TextureUnit.Texture0);
            }

            foreach (var i in child)
            {
                i.load();
            }
        }

        public void setDirectionalLight(Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            _shader.SetVector3("dirLight.direction", direction);
            _shader.SetVector3("dirLight.ambient", ambient);
            _shader.SetVector3("dirLight.diffuse", diffuse);
            _shader.SetVector3("dirLight.specular", specular);
        }

        public void setPointLight(List<Asset3d> positions, Vector3 ambient, Vector3 diffuse, Vector3 specular, float constant, float linear, float quadratic)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                _shader.SetVector3($"pointLights[{i}].position", positions[i].objectCenter);
                _shader.SetVector3($"pointLights[{i}].ambient", ambient);
                _shader.SetVector3($"pointLights[{i}].diffuse", diffuse);

                _shader.SetVector3($"pointLights[{i}].specular", specular);
                _shader.SetFloat($"pointLights[{i}].constant", constant);
                _shader.SetFloat($"pointLights[{i}].linear", linear);
                _shader.SetFloat($"pointLights[{i}].quadratic", quadratic);
            }

        }

        public void setFragVar(Vector3 viewPos)
        {
            _shader.SetVector3("objectColor", color.Xyz);
            _shader.SetVector3("viewPos", viewPos);
        }

        public void render(int line, Matrix4 camera_view, Matrix4 camera_projection, Vector3 cameraPos, List<Asset3d> pointLights)
        {
            _shader.Use();

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", camera_view);
            _shader.SetMatrix4("projection", camera_projection);

            GL.BindVertexArray(_vertexArrayObject);

            if (texCoords.Count > 0)
            {
                _texture.Use(TextureUnit.Texture0);
            }

            if (normals.Count > 0)
            {
                _shader.SetMatrix4("normalMat", normalMat);

                /*_shader.SetVector3("lightPos", light.objectCenter);
                _shader.SetVector3("lightColor", light.color.Xyz);
                _shader.SetVector3("viewPos", cameraPos);*/
            }
            else
            {
                _shader.SetVector4("objectColor", color);
            }

            if (indices.Count != 0)
            {
                switch (line)
                {
                    case 1:
                        GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
                        break;
                    case -1:
                        GL.DrawElements(PrimitiveType.LineStrip, indices.Count, DrawElementsType.UnsignedInt, 0);
                        break;
                }
            }
            else
            {
                switch (line)
                {
                    case 1:
                        GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);
                        break;
                    case -1:
                        GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Count);
                        break;
                }
            }

            foreach (var i in child)
            {
                i.render(line, camera_view, camera_projection, cameraPos, pointLights);

                i.setFragVar(cameraPos);
                i.setDirectionalLight(new Vector3(-0.2f, -1.0f, -0.3f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.5f, 0.5f, 0.5f));
                i.setPointLight(pointLights, new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
            }
        }

        public void getPosition(bool parent)
        {
            List<Vector3> positions = new List<Vector3>();
            float Xmax, Xmin, Ymax, Ymin, Zmax, Zmin;

            if (!parent)
            {
                Xmax = vertices.Max(t => t.X);
                Xmin = vertices.Min(t => t.X);
                Ymax = vertices.Max(t => t.Y);
                Ymin = vertices.Min(t => t.Y);
                Zmax = vertices.Max(t => t.Z);
                Zmin = vertices.Min(t => t.Z);
            }
            else
            {
                List<Vector3> tempPos = new List<Vector3>();

                foreach (Asset3d i in child)
                {
                    /*Console.WriteLine("check");*/
                    Xmax = i.vertices.Max(t => t.X);
                    Xmin = i.vertices.Min(t => t.X);
                    Ymax = i.vertices.Max(t => t.Y);
                    Ymin = i.vertices.Min(t => t.Y);
                    Zmax = i.vertices.Max(t => t.Z);
                    Zmin = i.vertices.Min(t => t.Z);

                    positions.Add(new Vector3(Xmin, Ymin, Zmin));
                    positions.Add(new Vector3(Xmax, Ymax, Zmax));

                    i.posMin = positions[0];
                    i.posMax = positions[1];

                    tempPos.Add(i.posMin);
                    tempPos.Add(i.posMax);
                }

                Xmax = tempPos.Max(t => t.X);
                Xmin = tempPos.Min(t => t.X);
                Ymax = tempPos.Max(t => t.Y);
                Ymin = tempPos.Min(t => t.Y);
                Zmax = tempPos.Max(t => t.Z);
                Zmin = tempPos.Min(t => t.Z);
            }

            positions.Add(new Vector3(Xmin, Ymin, Zmin));
            positions.Add(new Vector3(Xmax, Ymax, Zmax));

            posMin = positions[0];
            posMax = positions[1];

            /*Console.WriteLine("objek max: " + posMax);
            Console.WriteLine("objek min: " + posMin);*/
        }

        #region solidObjects
        public void createCuboid(float x_, float y_, float z_, float length, bool useNormals, bool useTextures)
        {
            objectCenter = new Vector3(x_, y_, z_);

            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
				//Back
				1, 2, 0,
                2, 1, 3,
				
				//Top
				5, 0, 4,
                0, 5, 1,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				0, 6, 4,
                6, 0, 2,

				//Front
				4, 7, 5,
                7, 4, 6,

				//Bottom
				3, 6, 2,
                6, 3, 7
            };

            if (useNormals)
            {
                for (int i = 0; i < tempIndices.Count; i++)
                {
                    vertices.Add(tempVertices[(int)tempIndices[i]]);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitZ);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitY);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitX);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitX);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitZ);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitY);
                }
            }

            if (useTextures)
            {
                for (int i = 0; i < tempIndices.Count; i++)
                {
                    vertices.Add(tempVertices[(int)tempIndices[i]]);
                }

                texCoords = new List<Vector3>()
                {
                    (0, 1.0f, 0),
                    (1.0f, 0, 0),
                    (1.0f, 1.0f, 0),
                    (1.0f, 0, 0),
                    (0, 1.0f, 0),
                    (0, 0, 0),

                    (1.0f, 0, 0),
                    (0, 1.0f, 0),
                    (0, 0, 0),
                    (0, 1.0f, 0),
                    (1.0f, 0, 0),
                    (1.0f, 1.0f, 0),

                    (0, 1.0f, 0),
                    (1.0f, 0, 0),
                    (1.0f, 1.0f, 0),
                    (1.0f, 0, 0),
                    (0, 1.0f, 0),
                    (0, 0, 0),

                    (0, 1.0f, 0),
                    (1.0f, 0, 0),
                    (1.0f, 1.0f, 0),
                    (1.0f, 0, 0),
                    (0, 1.0f, 0),
                    (0, 0, 0),

                    (0, 1.0f, 0),
                    (1.0f, 0, 0),
                    (1.0f, 1.0f, 0),
                    (1.0f, 0, 0),
                    (0, 1.0f, 0),
                    (0, 0, 0),

                    (1.0f, 0, 0),
                    (0, 1.0f, 0),
                    (0, 0, 0),
                    (0, 1.0f, 0),
                    (1.0f, 0, 0),
                    (1.0f, 1.0f, 0)
                };
            }

            if (!useNormals && !useTextures)
            {
                vertices = tempVertices;
                indices = tempIndices;
            }

            getPosition(false);
        }
        public void createCuboid_v2(float x_, float y_, float z_, float length, float extra, bool useNormals)
        {
            objectCenter = new Vector3(x_, y_, z_);

            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - (length + extra) / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - (length + extra) / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - (length + extra) / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - (length + extra) / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
				//Back
				1, 2, 0,
                2, 1, 3,
				
				//Top
				5, 0, 4,
                0, 5, 1,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				0, 6, 4,
                6, 0, 2,

				//Front
				4, 7, 5,
                7, 4, 6,

				//Bottom
				3, 6, 2,
                6, 3, 7
            };

            if (useNormals)
            {
                for (int i = 0; i < tempIndices.Count; i++)
                {
                    vertices.Add(tempVertices[(int)tempIndices[i]]);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitZ);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitY);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitX);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitX);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitZ);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitY);
                }
            }

            if (!useNormals)
            {
                vertices = tempVertices;
                indices = tempIndices;
            }

            getPosition(false);
        }
        public void createCuboid_v3(float x_, float y_, float z_, float length, bool useNormals)
        {
            objectCenter = new Vector3(x_, y_, z_);

            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 1.0f;
            temp_vector.Y = y_ + length / 3.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 1.0f;
            temp_vector.Y = y_ + length / 3.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 1.0f;
            temp_vector.Y = y_ - length / 3.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 1.0f;
            temp_vector.Y = y_ - length / 3.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 1.0f;
            temp_vector.Y = y_ + length / 3.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 1.0f;
            temp_vector.Y = y_ + length / 3.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 1.0f;
            temp_vector.Y = y_ - length / 3.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 1.0f;
            temp_vector.Y = y_ - length / 3.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
				//Back
				1, 2, 0,
                2, 1, 3,
				
				//Top
				5, 0, 4,
                0, 5, 1,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				0, 6, 4,
                6, 0, 2,

				//Front
				4, 7, 5,
                7, 4, 6,

				//Bottom
				3, 6, 2,
                6, 3, 7
            };

            if (useNormals)
            {
                for (int i = 0; i < tempIndices.Count; i++)
                {
                    vertices.Add(tempVertices[(int)tempIndices[i]]);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitZ);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitY);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitX);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitX);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitZ);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitY);
                }
            }

            if (!useNormals)
            {
                vertices = tempVertices;
                indices = tempIndices;
            }

            getPosition(false);
        }
        public void createCuboid_v4(float x_, float y_, float z_, float length, bool useNormals)
        {
            objectCenter = new Vector3(x_, y_, z_);

            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2f;
            temp_vector.Y = y_ + length / 2f;
            temp_vector.Z = z_ - length / 0.1f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2f;
            temp_vector.Y = y_ + length / 2f;
            temp_vector.Z = z_ - length / 0.1f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2f;
            temp_vector.Y = y_ - length / 2f;
            temp_vector.Z = z_ - length / 0.1f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2f;
            temp_vector.Y = y_ - length / 2f;
            temp_vector.Z = z_ - length / 0.1f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2f;
            temp_vector.Y = y_ + length / 2f;
            temp_vector.Z = z_ + length / 0.2f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2f;
            temp_vector.Y = y_ + length / 2f;
            temp_vector.Z = z_ + length / 0.2f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2f;
            temp_vector.Y = y_ - length / 2f;
            temp_vector.Z = z_ + length / 0.2f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2f;
            temp_vector.Y = y_ - length / 2f;
            temp_vector.Z = z_ + length / 0.2f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
				//Back
				1, 2, 0,
                2, 1, 3,
				
				//Top
				5, 0, 4,
                0, 5, 1,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				0, 6, 4,
                6, 0, 2,

				//Front
				4, 7, 5,
                7, 4, 6,

				//Bottom
				3, 6, 2,
                6, 3, 7
            };

            if (useNormals)
            {
                for (int i = 0; i < tempIndices.Count; i++)
                {
                    vertices.Add(tempVertices[(int)tempIndices[i]]);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitZ);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitY);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitX);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitX);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(Vector3.UnitZ);
                }

                for (int i = 0; i < 6; i++)
                {
                    normals.Add(-Vector3.UnitY);
                }
            }

            if (!useNormals)
            {
                vertices = tempVertices;
                indices = tempIndices;
            }

            getPosition(false);
        }
        public void createEllipsoid(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount, bool useNormals)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Cos(stackAngle);
                tempZ = radZ * (float)Math.Sin(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = z + tempZ;

                    if (useNormals)
                    {
                        normals.Add(new Vector3(temp_vector.X, temp_vector.Y, temp_vector.Z));
                    }

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }

            getPosition(false);
        }
        public void createHyper(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount, bool useNormals)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)stackAngle;
                tempY = radY * (float)stackAngle;
                tempZ = radZ * (float)(stackAngle * stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)(Math.Sin(sectorAngle) / Math.Cos(sectorAngle));
                    temp_vector.Y = y + tempY * (float)(1 / Math.Cos(sectorAngle));
                    temp_vector.Z = z + tempZ;

                    vertices.Add(temp_vector);

                    if (useNormals)
                    {
                        normals.Add(temp_vector);
                    }
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }

            getPosition(false);
        }
        public void createTorus(float x, float y, float z, float radMajor, float radMinor, float sectorCount, float stackCount, bool useNormals)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            stackCount *= 2;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = 2 * pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radMajor + radMinor * (float)Math.Cos(stackAngle);
                tempY = radMinor * (float)Math.Sin(stackAngle);
                tempZ = radMajor + radMinor * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);

                    if (useNormals)
                    {
                        normals.Add(temp_vector);
                    }
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    indices.Add(k1);
                    indices.Add(k2);
                    indices.Add(k1 + 1);

                    indices.Add(k1 + 1);
                    indices.Add(k2);
                    indices.Add(k2 + 1);
                }
            }

            getPosition(false);
        }
        public void createTorus_v2(float x, float y, float z, float radMajor, float radMinor, float sectorCount, float stackCount, bool useNormals)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            stackCount *= 2;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = 2 * pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radMajor + radMinor * (float)Math.Cos(stackAngle);
                tempY = radMinor * (float)Math.Sin(stackAngle);
                tempZ = radMajor + radMinor * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);

                    if (useNormals)
                    {
                        normals.Add(temp_vector);
                    }
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount / 2; ++j, ++k1, ++k2)
                {
                    indices.Add(k1);
                    indices.Add(k2);
                    indices.Add(k1 + 1);

                    indices.Add(k1 + 1);
                    indices.Add(k2);
                    indices.Add(k2 + 1);
                }
            }

            getPosition(false);
        }
        public void createCone(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount, bool useNormals)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)(stackAngle);
                tempY = radY * (float)(stackAngle);
                tempZ = radZ * (float)(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = z + tempZ;

                    if (temp_vector.Z > 0)
                    {
                        vertices.Add(temp_vector);

                        if (useNormals)
                        {
                            normals.Add(temp_vector);
                        }
                    }
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k1);
                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }

            getPosition(false);
        }
        #endregion

        #region transforms
        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            var radAngle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4
                (
                new Vector4((float)(Math.Cos(radAngle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) + vector.Z * Math.Sin(radAngle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.Y * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) - vector.Z * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.X * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.Y * Math.Sin(radAngle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.X * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(radAngle))), 0),
                Vector4.UnitW
                );

            model *= Matrix4.CreateTranslation(-pivot);
            model *= arbRotationMatrix;
            model *= Matrix4.CreateTranslation(pivot);

            normalMat = Matrix4.Transpose(Matrix4.Invert(model));

            for (int i = 0; i < 3; i++)
            {
                _euler[i] = Vector3.Normalize(getRotationResult(pivot, vector, radAngle, _euler[i], true));
            }

            objectCenter = getRotationResult(pivot, vector, radAngle, objectCenter);

            foreach (var i in child)
            {
                i.rotate(pivot, vector, angle);
            }
        }

        public Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;

            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }

            newPosition.X =
                temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));

            newPosition.Y =
                temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));

            newPosition.Z =
                temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }

        public void translate(float x, float y, float z)
        {
            model *= Matrix4.CreateTranslation(x, y, z);

            normalMat = Matrix4.Transpose(Matrix4.Invert(model));

            objectCenter.X += x;
            objectCenter.Y += y;
            objectCenter.Z += z;

            posMin.X += x;
            posMin.Y += y;
            posMin.Z += z;

            posMax.X += x;
            posMax.Y += y;
            posMax.Z += z;

            foreach (var i in child)
            {
                i.translate(x, y, z);
            }
        }

        public void scaleNew(float scaleX, float scaleY, float scaleZ, Vector3 scaleCenter)
        {
            model *= Matrix4.CreateTranslation(-scaleCenter);
            model *= Matrix4.CreateScale(scaleX, scaleY, scaleZ);
            model *= Matrix4.CreateTranslation(scaleCenter);

            normalMat = Matrix4.Transpose(Matrix4.Invert(model));

            foreach (var i in child)
            {
                i.scaleNew(scaleX, scaleY, scaleZ, scaleCenter);
            }
        }

        public void scale(float scaleX, float scaleY, float scaleZ)
        {
            model *= Matrix4.CreateTranslation(-objectCenter);
            model *= Matrix4.CreateScale(scaleX, scaleY, scaleZ);
            model *= Matrix4.CreateTranslation(objectCenter);

            foreach (var i in child)
            {
                i.scale(scaleX, scaleY, scaleZ);
            }
        }

        public void resetEuler()
        {
            _euler.Clear();
            _euler.Add(Vector3.UnitX);
            _euler.Add(Vector3.UnitY);
            _euler.Add(Vector3.UnitZ);
        }
        #endregion

        #region complexObjects
        public Asset3d createCockroach(float x, float y, float z)
        {
            var cockroach = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 1, 1));

            // KAKI 1
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
            cube7.translate(0f, 0f, -1.25f);
            var cube8 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1.5f, 0.5f, 0.5f));
            cube8.createCuboid_v2(0f, 0f, -0.75f, 0.1f, 1.5f, true);
            cube8.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube8.translate(0f, 0f, -1.25f);

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
            cube9.translate(0f, 0f, -.75f);
            var cube10 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1.5f, 0.5f, 0.5f));
            cube10.createCuboid_v2(1.5f, 0f, -0.75f, 0.1f, 1.5f, true);
            cube10.rotate(Vector3.Zero, Vector3.UnitX, -45);
            cube10.translate(0f, 0f, -.75f);

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

            // MUKA
            var ellips2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.39f, 0.26f, 0.12f));
            ellips2.createEllipsoid(1.75f, 0f, -0.5f, .15f, .35f, .25f, 50, 50, true);

            var eye1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.8627450980392157f, 0.8627450980392157f, 0.8627450980392157f));
            eye1.createEllipsoid(1.75f, .25f, -.25f, .1f, .1f, .1f, 30, 30, true);
            var eye2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.8627450980392157f, 0.8627450980392157f, 0.8627450980392157f));
            eye2.createEllipsoid(1.75f, .25f, -.75f, .1f, .1f, .1f, 30, 30, true);

            // SAYAP
            var wings = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.1f, 1f, 1f));
            wings.createHyper(-0.5f, 0f, 0f, 0.5f, 0.25f, 0.5f, 5, 100, true);
            wings.rotate(Vector3.Zero, Vector3.UnitY, -90);
            wings.rotate(Vector3.Zero, Vector3.UnitZ, -45);
            
            var wings2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.1f, 1f, 1f));
            wings2.createHyper(-0.5f, 0f, 0f, 0.5f, -.25f, 0.5f, 5, 100, true);
            wings2.rotate(Vector3.Zero, Vector3.UnitY, -90);
            wings2.rotate(Vector3.Zero, Vector3.UnitZ, -45);

            cockroach.child.Add(wings);
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

            cockroach.child.Add(eye1);
            cockroach.child.Add(eye2);

            cockroach.child.Add(ellips1);
            cockroach.child.Add(ellips2);

            cockroach.translate(x, y, z);

            return cockroach;
        }
        public Asset3d createCaterpillar(float x, float y, float z)
        {
            var ulat = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 1, 1));

            // badan
            var badan1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.5f, 0.1f));
            badan1.createEllipsoid(-5f, 2.0f, 1f, 1f, 1f, 1f, 30, 24, true);
            var badan2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.5f, 0.1f));
            badan2.createEllipsoid(-4f, 2.0f, 1f, 1f, 1f, 1f, 30, 24, true);
            var badan3 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.5f, 0.1f));
            badan3.createEllipsoid(-2.9f, 2.0f, 1f, 1f, 1f, 1f, 30, 24, true);
            var badan4 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 1.5f, 0.5f));
            badan4.createEllipsoid(-1.9f, 2.8f, 1f, 1f, 1f, 1f, 30, 24, true);

            // kaca mata

            // mata gagang z-2
            var mata1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.5f, 1.5f, 1.5f));
            mata1.createTorus(-2f, 2f, -3f, 0.3f, 0.1f, 100, 100, true);
            mata1.rotate(Vector3.Zero, Vector3.UnitX, 90);
            
            var gagangpanjang1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.0f, 0.0f));
            gagangpanjang1.createCuboid_v2(-2.8f, 1.5f, -3f, 0.1f, 2f, true);
            gagangpanjang1.rotate(Vector3.Zero, Vector3.UnitX, 90);
            var gagangpanjang2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.0f, 0.0f));
            gagangpanjang2.createCuboid_v2(-1f, 1.5f, -3f, 0.1f, 2f, true);
            gagangpanjang2.rotate(Vector3.Zero, Vector3.UnitX, 90);

            // kecil y+2
            var gagangkecil1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.0f, 0.0f));
            gagangkecil1.createCuboid_v3(-2.6f, 3f, 1.8f, 0.2f, true);
            var gagangkecil2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.0f, 0.0f));
            gagangkecil2.createCuboid_v3(-1.2f, 3f, 1.8f, 0.2f, true);
            
            // kaki BELAKANG
            var kaki1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki1.createEllipsoid(-4.5f, 0.8f, -1f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki1.rotate(Vector3.Zero, Vector3.UnitX, 45);
            var kaki2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki2.createEllipsoid(-3.5f, 0.8f, -1f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki2.rotate(Vector3.Zero, Vector3.UnitX, 45);

            // KAKI DEPAN
            var kaki3 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki3.createEllipsoid(-4.5f, 2f, 0.5f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki3.rotate(Vector3.Zero, Vector3.UnitX, 40);
            var kaki4 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki4.createEllipsoid(-3.5f, 2f, 0.5f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki4.rotate(Vector3.Zero, Vector3.UnitX, 40);

            // TENGAH
            var kaki5 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki5.createEllipsoid(-4.5f, 1.5f, 0f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki5.rotate(Vector3.Zero, Vector3.UnitX, 35);
            var kaki6 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.0f, 0.8f, 0.3f));
            kaki6.createEllipsoid(-3.5f, 1.5f, 0f, 0.2f, 0.5f, 0.2f, 10, 20, true);
            kaki6.rotate(Vector3.Zero, Vector3.UnitX, 35);

            // mulut 
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

            ulat.translate(x, y, z);
            return ulat;
        }
        public Asset3d createCloud(float x, float y, float z)
        {
            var awan = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 1, 1));

            var awan1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.8f, 0.8f, 0.8f));
            awan1.createEllipsoid(-2.75f, 6f, 2f, 1f, 0.5f, 0.5f, 30, 24, true);

            var awan2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.8f, 0.8f, 0.8f));
            awan2.createEllipsoid(-2f, 6.5f, 2f, 1f, 0.5f, 0.5f, 30, 24, true);

            var awan3 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.8f, 0.8f, 0.8f));
            awan3.createEllipsoid(-1.75f, 6f, 2f, 1f, 0.5f, 0.5f, 30, 24, true);

            var awan4 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.8f, 0.8f, 0.8f));
            awan4.createEllipsoid(-1f, 6f, 2f, 1f, 0.5f, 0.5f, 30, 24, true);

            awan.child.Add(awan1);
            awan.child.Add(awan2);
            awan.child.Add(awan3);
            awan.child.Add(awan4);

            awan.translate(x, y, z);
            return awan;
        }
        public Asset3d createLightPole(float x, float y, float z)
        {
            var lightPole = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 1, 1));

            var pole2 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.054901960784313725f, 0.054901960784313725f, 0.054901960784313725f));
            pole2.createCuboid_v2(0f, -9.5f, 2.5f, 3.25f, 6.5f, true);
            lightPole.child.Add(pole2);

            var pole1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.12549019607843137f, 0.12549019607843137f, 0.12549019607843137f));
            pole1.createCuboid_v2(0f, 3f, 2.5f, 1.95f, 50f, true);
            lightPole.child.Add(pole1);

            var lampPlace = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0f, 0f, 0f));
            lampPlace.createEllipsoid(0f, 3.15f, 2.5f, 2.15f, 2.075f, 2.15f, 50, 50, true);
            lightPole.child.Add(lampPlace);

            var lamp = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1f, 1f, 1f));
            lamp.createEllipsoid(0f, 5f, 2.5f, 5f, 5f, 5f, 50, 50, true);
            /*lightPole.child.Add(lamp);*/

            lightPole.translate(x, y, z);
            lightPole.resetEuler();
            lightPole.scale(.5f, .5f, .5f);

            lightPole.objectCenter = lamp.objectCenter;

            return lightPole;
        }
        public Asset3d createRock(float x, float y, float z)
        {
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

            rock.translate(x, y, z);
            return rock;
        }
        public Asset3d createTwig(float x, float y, float z)
        {
            var twig1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(1, 1, 1));

            var twig_body = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.75f, 0.6f, 0.42f));
            twig_body.createCuboid_v2(0f, .1f, 2.5f, .05f, 1.5f, true);
            twig1.child.Add(twig_body);

            var twig_branch1 = new Asset3d("normalShader.vert", "normalShader.frag", new Vector3(0.75f, 0.6f, 0.42f));
            twig_branch1.createCuboid_v2(0f, .1f, 2.5f, .05f, .25f, true);
            twig_branch1.rotate(Vector3.Zero, Vector3.UnitZ, -37.5f);
            twig_branch1.translate(.025f, -.1f, 0f);
            twig1.child.Add(twig_branch1);

            twig1.translate(x, y, z);
            return twig1;
        }
        #endregion
    }
}
