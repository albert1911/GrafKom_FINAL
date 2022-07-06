using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace ConsoleApp3
{
	class Program
	{
		static void Main(string[] args)
		{
			var ourWindow = new NativeWindowSettings()
			{
				Size = new Vector2i(600, 600),
				Title = "INI CERITANYA DI TAMAN"
			};

			using (var window = new Window(GameWindowSettings.Default, ourWindow))
			{
				window.Run();
			}
		}
	}
}
