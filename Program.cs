using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace objectFinal
{
	class Program
	{
		static void Main(string[] args)
		{
			var ourWindow = new NativeWindowSettings()
			{
				Size = new Vector2i(800, 800),
				Title = "Objects"
			};

			using (var window = new Window(GameWindowSettings.Default, ourWindow))
			{
				window.Run();
			}
		}
	}
}
